using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Math = System.Math;
using MathF = System.MathF;

internal class ChunkManager
{
    public ChunkManager(uint region, IGenerator generator)
    {
        chunks = new TriCache<BlocType[,,]>((x, y, z) => generator.Generate(new Vector3Int(x, y, z)));
        blocCache = new(() => Object.Instantiate(RessourcesManager.Cube, new Vector3(0, 0, 0), Quaternion.identity));

        regionSize = (int)region;
        int max = (regionSize * 2) + 1;
        generatedRegions = new Stack<GameObject>[max][][];
        for (int i = 0; i < max; i++)
        {
            int localRegion = regionSize - Math.Abs(regionSize - i);

            int localMax = (localRegion * 2) + 1;
            generatedRegions[i] = new Stack<GameObject>[localMax][];
            for (int j = 0; j < localMax; j++)
            {
                int subLocalRegion = localRegion - Math.Abs(localRegion - j);

                int subLocalMax = (subLocalRegion * 2) + 1;
                generatedRegions[i][j] = new Stack<GameObject>[subLocalMax];
                for (int k = 0; k < subLocalMax; k++)
                    generatedRegions[i][j][k] = new();
            }
        }
    }

    public void MoveTo(Vector3 position)
    {
        Vector3Int chunkCoordinate = new(
            (int)MathF.Floor(position.x / IGenerator.ZONE_SIZE_FLOAT),
            (int)MathF.Floor(position.y / IGenerator.ZONE_SIZE_FLOAT),
            (int)MathF.Floor(position.z / IGenerator.ZONE_SIZE_FLOAT));

        if (chunkCoordinate == currentPos)
            return;

        GenerateAll(chunkCoordinate);

        currentPos = chunkCoordinate;
    }

    private void GenerateAll(Vector3Int chunkCoordinate)
    {
        foreach (Stack<GameObject>[][] n1 in generatedRegions)
        {
            foreach (Stack<GameObject>[] n2 in n1)
            {
                foreach (Stack<GameObject> n3 in n2)
                    blocCache.Add(n3);
            }
        }

        int max = (regionSize * 2) + 1;
        for (int i = 0; i < max; i++)
        {
            int localRegion = regionSize - Math.Abs(regionSize - i);

            int localMax = (localRegion * 2) + 1;
            for (int j = 0; j < localMax; j++)
            {
                int subLocalRegion = localRegion - Math.Abs(localRegion - j);

                int subLocalMax = (subLocalRegion * 2) + 1;
                for (int k = 0; k < subLocalMax; k++)
                {
                    generatedRegions[i][j][k] = InstanciateChunk(new Vector3Int(
                        i - regionSize + chunkCoordinate.x, j - localRegion + chunkCoordinate.y, k - subLocalRegion + chunkCoordinate.z));
                }
            }
        }
    }

    /// <summary>Crée la bordure de la partie remplie de la zone</summary>
    /// <param name="chunkCoordinate"></param>
    /// <returns>Liste des cubes créés</returns>
    private Stack<GameObject> InstanciateChunk(Vector3Int chunkCoordinate)
    {
        Stack<GameObject> list = new();

        BlocType[,,] zone = chunks[chunkCoordinate.x, chunkCoordinate.y, chunkCoordinate.z];

        Vector3Int zonePos = new(
            chunkCoordinate.x * IGenerator.ZONE_SIZE, chunkCoordinate.y * IGenerator.ZONE_SIZE, chunkCoordinate.z * IGenerator.ZONE_SIZE);

        const int maxMoins = IGenerator.ZONE_SIZE - 1;

        for (int j = 0; j < IGenerator.ZONE_SIZE; j++)
        {
            for (int k = 0; k < IGenerator.ZONE_SIZE; k++)
            {
                GenerateBloc(list, zone, new Vector3Int(0, j, k), zonePos);
                GenerateBloc(list, zone, new Vector3Int(maxMoins, j, k), zonePos);
            }
        }

        for (int i = 1; i < maxMoins; i++)
        {
            for (int k = 0; k < IGenerator.ZONE_SIZE; k++)
            {
                GenerateBloc(list, zone, new Vector3Int(i, 0, k), zonePos);
                GenerateBloc(list, zone, new Vector3Int(i, maxMoins, k), zonePos);
            }

            for (int j = 1; j < maxMoins; j++)
            {
                GenerateBloc(list, zone, new Vector3Int(i, j, 0), zonePos);
                GenerateBloc(list, zone, new Vector3Int(i, j, maxMoins), zonePos);

                for (int k = 1; k < maxMoins; k++)
                {
                    if (zone[i, j, k] != BlocType.Air)
                    {
                        bool nearAir = zone[i + 1, j, k] == BlocType.Air || zone[i - 1, j, k] == BlocType.Air ||
                            zone[i, j + 1, k] == BlocType.Air || zone[i, j - 1, k] == BlocType.Air ||
                            zone[i, j, k + 1] == BlocType.Air || zone[i, j, k - 1] == BlocType.Air;

                        if (nearAir)
                            GenerateBloc(list, zone, new Vector3Int(i, j, k), zonePos);
                    }
                }
            }
        }

        return list;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GenerateBloc(Stack<GameObject> list, BlocType[,,] zone, Vector3Int localPos, Vector3Int zonePos)
    {
        BlocType type = zone[localPos.x, localPos.y, localPos.z];
        if (type == BlocType.Air)
            return;

        Vector3Int finalPos = localPos + zonePos;

        GameObject newCube = blocCache.Get();
        newCube.transform.position = new Vector3(finalPos.x, finalPos.y, finalPos.z);
        newCube.GetComponent<MeshRenderer>().material = RessourcesManager.GetMaterial(type);
        list.Push(newCube);
    }

    private Vector3Int currentPos = new(int.MinValue, int.MinValue, int.MinValue);

    private readonly Stack<GameObject>[][][] generatedRegions;

    private readonly int regionSize;

    private readonly TriCache<BlocType[,,]> chunks;

    private readonly MutliStackCache<GameObject> blocCache;
}