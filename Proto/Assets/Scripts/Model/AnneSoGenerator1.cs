using UnityEngine;
using UnityEngine.XR;

internal class AnneSoGenerator1 : IGenerator
{

    public AnneSoGenerator1()
    {
        heightMap = new(getHeight);
    }
    private readonly BiCache<int[,]> heightMap;
    private int[,] getHeight(int x, int y)
    {
        int[,] heights = new int[IGenerator.ZONE_SIZE, IGenerator.ZONE_SIZE];
        for (int i = 0; i < IGenerator.ZONE_SIZE; ++i)
        {
            for (int j = 0; j < IGenerator.ZONE_SIZE; ++j)
            {
                heights[i, j]
                    = (int)(Mathf.PerlinNoise(
                        i / IGenerator.ZONE_SIZE_FLOAT + x,
                        j / IGenerator.ZONE_SIZE_FLOAT + y)
                    * IGenerator.ZONE_SIZE);
            }
        }
        return heights;
    }

    public BlocType[,,] Generate(Vector3Int zone)
    {
        BlocType[,,] result = new BlocType[IGenerator.ZONE_SIZE, IGenerator.ZONE_SIZE, IGenerator.ZONE_SIZE];
        const int MAX_STONE_HEIGHT = 1;

        // génération de la carte des hauteurs
        int[,] heights = heightMap[zone.x, zone.z];

        for (int x = 0; x < IGenerator.ZONE_SIZE; ++x)
        {
            for (int y = 0; y < IGenerator.ZONE_SIZE; ++y)
            {
                for (int z = 0; z < IGenerator.ZONE_SIZE; ++z)
                {
                    Vector3Int realCoord = new(
                        x + (zone.x * IGenerator.ZONE_SIZE),
                        y + (zone.y * IGenerator.ZONE_SIZE),
                        z + (zone.z * IGenerator.ZONE_SIZE));
                    if (realCoord.y < heights[x, z] - MAX_STONE_HEIGHT)
                        result[x, y, z] = BlocType.Stone;
                    else if (realCoord.y < heights[x, z])
                        result[x, y, z] = BlocType.Dirt;
                }
            }
        }

        return result;
    }
}