using UnityEngine;

internal class AnneSoGeneratorCorrected : IGenerator
{
    public AnneSoGeneratorCorrected()
    {
        heigthsCache = new((x, y) =>
        {
            // génération de la carte des hauteurs
            int[,] heights = new int[IGenerator.ZONE_SIZE, IGenerator.ZONE_SIZE];
            for (int i = 0; i < IGenerator.ZONE_SIZE; ++i)
            {
                for (int j = 0; j < IGenerator.ZONE_SIZE; ++j)
                {
                    heights[i, j]
                        = (int)(IGenerator.ZONE_SIZE * Mathf.PerlinNoise(
                            x + (i / IGenerator.ZONE_SIZE_FLOAT), y + (j / IGenerator.ZONE_SIZE_FLOAT)));
                }
            }
            return heights;
        });
    }

    public BlocType[,,] Generate(Vector3Int zone)
    {
        BlocType[,,] result = new BlocType[IGenerator.ZONE_SIZE, IGenerator.ZONE_SIZE, IGenerator.ZONE_SIZE];
        const int MAX_DIRT_HEIGHT = 1;

        int[,] heights = heigthsCache[zone.x, zone.z];

        int YCorrection = zone.y * IGenerator.ZONE_SIZE;

        for (int x = 0; x < IGenerator.ZONE_SIZE; ++x)
        {
            for (int y = 0; y < IGenerator.ZONE_SIZE; ++y)
            {
                for (int z = 0; z < IGenerator.ZONE_SIZE; ++z)
                {
                    int realY = y + YCorrection;
                    if (realY < heights[x, z] - MAX_DIRT_HEIGHT)
                        result[x, y, z] = BlocType.Stone;
                    else if (realY < heights[x, z])
                        result[x, y, z] = BlocType.Dirt;
                }
            }
        }

        return result;
    }

    private readonly BiCache<int[,]> heigthsCache;
}