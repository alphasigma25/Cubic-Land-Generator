using UnityEngine;

internal class FullStoneGenerator : IGenerator
{
    public BlocType[,,] Generate(Vector3Int zone)
    {
        BlocType[,,] result = new BlocType[IGenerator.ZONE_SIZE, IGenerator.ZONE_SIZE, IGenerator.ZONE_SIZE];
        result.Fill(BlocType.Stone);
        return result;
    }
}