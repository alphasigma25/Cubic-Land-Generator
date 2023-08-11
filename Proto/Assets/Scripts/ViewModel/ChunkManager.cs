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

        regionSize = (int)region;
        int max = (regionSize * 2) + 1;
        generatedRegions = new List<GameObject>[max][][];
        for (int i = 0; i < max; i++)
        {
            int localRegion = regionSize - Math.Abs(regionSize - i);

            int localMax = (localRegion * 2) + 1;
            generatedRegions[i] = new List<GameObject>[localMax][];
            for (int j = 0; j < localMax; j++)
            {
                int subLocalRegion = localRegion - Math.Abs(localRegion - j);

                int subLocalMax = (subLocalRegion * 2) + 1;
                generatedRegions[i][j] = new List<GameObject>[subLocalMax];
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
        foreach (List<GameObject>[][] n1 in generatedRegions)
        {
            foreach (List<GameObject>[] n2 in n1)
            {
                foreach (List<GameObject> n3 in n2)
                {
                    foreach (GameObject n4 in n3)
                        Object.Destroy(n4);
                }
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

    /// <summary>
    /// Crée la bordure de la partie remplie de la zone
    /// </summary>
    /// <param name="chunkCoordinate"></param>
    /// <returns>Liste des cubes créés</returns>
    private List<GameObject> InstanciateChunk(Vector3Int chunkCoordinate)
    {
        List<GameObject> list = new();

        BlocType[,,] zone = chunks[chunkCoordinate.x, chunkCoordinate.y, chunkCoordinate.z];

        int decalX = chunkCoordinate.x * IGenerator.ZONE_SIZE;
        int decalY = chunkCoordinate.y * IGenerator.ZONE_SIZE;
        int decalZ = chunkCoordinate.z * IGenerator.ZONE_SIZE;

        const int maxMoins = IGenerator.ZONE_SIZE - 1;

        for (int j = 0; j < IGenerator.ZONE_SIZE; j++)
        {
            for (int k = 0; k < IGenerator.ZONE_SIZE; k++)
            {
                GenerateBloc(list, zone, 0, j, k, decalX, decalY, decalZ);
                GenerateBloc(list, zone, maxMoins, j, k, decalX, decalY, decalZ);
            }
        }

        for (int i = 1; i < maxMoins; i++)
        {
            for (int k = 0; k < IGenerator.ZONE_SIZE; k++)
            {
                GenerateBloc(list, zone, i, 0, k, decalX, decalY, decalZ);
                GenerateBloc(list, zone, i, maxMoins, k, decalX, decalY, decalZ);
            }

            for (int j = 1; j < maxMoins; j++)
            {
                GenerateBloc(list, zone, i, j, 0, decalX, decalY, decalZ);
                GenerateBloc(list, zone, i, j, maxMoins, decalX, decalY, decalZ);

                for (int k = 1; k < maxMoins; k++)
                {
                    bool nearAir = zone[i + 1, j, k] == BlocType.Air || zone[i - 1, j, k] == BlocType.Air ||
                        zone[i, j + 1, k] == BlocType.Air || zone[i, j - 1, k] == BlocType.Air ||
                        zone[i, j, k + 1] == BlocType.Air || zone[i, j, k - 1] == BlocType.Air;

                    if (nearAir)
                        GenerateBloc(list, zone, i, j, k, decalX, decalY, decalZ);
                }
            }
        }

        return list;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void GenerateBloc(List<GameObject> list, BlocType[,,] zone, int i, int j, int k, int decalX, int decalY, int decalZ)
    {
        BlocType type = zone[i, j, k];
        if (type == BlocType.Air)
            return;

        GameObject newCube
            = Object.Instantiate(
                RessourcesManager.Cube, new Vector3(i + decalX, j + decalY, k + decalZ), Quaternion.identity);
        newCube.GetComponent<MeshRenderer>().material = RessourcesManager.GetMaterial(type);
        list.Add(newCube);
    }

    private Vector3Int currentPos = new(int.MinValue, int.MinValue, int.MinValue);

    private readonly List<GameObject>[][][] generatedRegions;

    private readonly int regionSize;

    private readonly TriCache<BlocType[,,]> chunks;
}