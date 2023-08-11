using System.Collections.Generic;
using UnityEngine;
using Math = System.Math;
using MathF = System.MathF;

internal class ChunkManager
{
    public ChunkManager(uint region)
    {
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

    public void GenerateAll(Vector3Int chunkCoordinate)
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

    private List<GameObject> InstanciateChunk(Vector3Int chunkCoordinate)
    {
        List<GameObject> list = new();

        BlocType[,,] zone = generator.Generate(chunkCoordinate);

        int decalX = chunkCoordinate.x * IGenerator.ZONE_SIZE;
        int decalY = chunkCoordinate.y * IGenerator.ZONE_SIZE;
        int decalZ = chunkCoordinate.z * IGenerator.ZONE_SIZE;

        for (int i = 0; i < IGenerator.ZONE_SIZE; i++)
        {
            for (int j = 0; j < IGenerator.ZONE_SIZE; j++)
            {
                for (int k = 0; k < IGenerator.ZONE_SIZE; k++)
                {
                    BlocType type = zone[i, j, k];
                    if (type != BlocType.Air)
                    {
                        GameObject newCube
                            = Object.Instantiate(
                                RessourcesManager.Cube, new Vector3(i + decalX, j + decalY, k + decalZ), Quaternion.identity);
                        newCube.GetComponent<MeshRenderer>().material = RessourcesManager.GetMaterial(type);
                        list.Add(newCube);
                    }
                }
            }
        }

        return list;
    }

    private Vector3Int currentPos = new(int.MinValue, int.MinValue, int.MinValue);

    private readonly IGenerator generator = new AnneSoGeneratorCorrected();

    private readonly List<GameObject>[][][] generatedRegions;

    private readonly int regionSize;
}