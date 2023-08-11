using System.Collections.Generic;
using UnityEngine;
using MathF = System.MathF;

internal class ChunkManager
{
    public void MoveTo(Vector3 position)
    {
        Vector3Int chunkCoordinate = new(
            (int)MathF.Floor(position.x / IGenerator.ZONE_SIZE_FLOAT),
            (int)MathF.Floor(position.y / IGenerator.ZONE_SIZE_FLOAT),
            (int)MathF.Floor(position.z / IGenerator.ZONE_SIZE_FLOAT));

        if (chunkCoordinate == currentPos)
            return;

        foreach (GameObject item in CurrentBlocs)
            Object.Destroy(item);

        CurrentBlocs = InstanciateChunk(chunkCoordinate);

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

    private List<GameObject> CurrentBlocs = new();

    private readonly IGenerator generator = new AnneSoGeneratorCorrected();
}