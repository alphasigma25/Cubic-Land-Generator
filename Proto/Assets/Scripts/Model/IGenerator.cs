using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenerator
{
    const int ZONE_SIZE = 10;
    // mécanisme de mise en cache des zones
    public BlocType[,,] Generate(Vector3Int zone);
}

public enum BlocType
{
    Air, Earth, Stone
}

public class Generator1 : IGenerator
{
    public BlocType[,,] Generate(Vector3Int zone)
    {
        BlocType[,,] result = new BlocType[IGenerator.ZONE_SIZE, IGenerator.ZONE_SIZE, IGenerator.ZONE_SIZE];
        int MAX_HEIGHT = 10;
        int MAX_STONE_HEIGHT = 3;

        // génération de la carte des hauteurs
        int[,] heights = new int[IGenerator.ZONE_SIZE, IGenerator.ZONE_SIZE];
        for (int i = 0; i < IGenerator.ZONE_SIZE; ++i)
        {
            for (int j = 0; j < IGenerator.ZONE_SIZE; ++j)
            {
                heights[i, j] = (int)(Mathf.PerlinNoise(i / 10, j / 10) * MAX_HEIGHT);
            }
        }

        for (int x = 0; x < IGenerator.ZONE_SIZE; ++x)
        {
            for (int y = 0; y < IGenerator.ZONE_SIZE; ++y)
            {
                for (int z = 0; z < IGenerator.ZONE_SIZE; ++z)
                {
                    Vector3Int realCoord = new Vector3Int(x + zone.x * IGenerator.ZONE_SIZE, y + zone.y * IGenerator.ZONE_SIZE, z + zone.z * IGenerator.ZONE_SIZE);
                    if (realCoord.z < heights[x, y] - MAX_STONE_HEIGHT)
                    {
                        result[x, y, z] = BlocType.Stone;
                    } else if (realCoord.z < heights[x, y])
                    {
                        result[x, y, z] = BlocType.Earth;
                    }
                }
            }
        }

        return result;
    }
}