using UnityEngine;

public interface IGenerator
{
    const int ZONE_SIZE = (int)ZONE_SIZE_FLOAT;

    const float ZONE_SIZE_FLOAT = 16f;

    public BlocType[,,] Generate(Vector3Int zone);
}

public enum BlocType
{
    Air = 0,
    Leaves = 1,
    Log = 2,
    Stone = 3,
    Sand = 4,
    Water = 5,
    Grass = 6,
    Cactus = 7,
    Diamond = 8,
    Dirt = 9,
    Gravel = 10,
    Snow = 11
}