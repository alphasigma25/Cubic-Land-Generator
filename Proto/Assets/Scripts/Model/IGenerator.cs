using UnityEngine;

public interface IGenerator
{
    const int ZONE_SIZE = 10;
    // m�canisme de mise en cache des zones
    public BlocType[,,] Generate(Vector3Int zone);
}

public enum BlocType
{
    Air, Earth, Stone
}