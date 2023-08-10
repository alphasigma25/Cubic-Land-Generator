using System;
using UnityEngine;

public class WorldBuilder : MonoBehaviour
{
    public GameObject Cube;

    public Material Leaves;

    public Material Log;

    public Material Stone;

    public Material Sand;

    public Material Water;

    public Material Grass;

    public Material Cactus;

    public Material Diamond;

    public Material Dirt;

    public Material Gravel;

    public Material Snow;

    public void Start()
    {
        IGenerator generator = new AnneSoGenerator1();
        BlocType[,,] zone = generator.Generate(new Vector3Int(0, 0, 0));

        for (int i = 0; i < IGenerator.ZONE_SIZE; i++)
        {
            for (int j = 0; j < IGenerator.ZONE_SIZE; j++)
            {
                for (int k = 0; k < IGenerator.ZONE_SIZE; k++)
                {
                    BlocType type = zone[i, j, k];
                    if (type != BlocType.Air)
                    {
                        GameObject newCube = Instantiate(Cube, new Vector3(i, j, k), Quaternion.identity);

                        newCube.GetComponent<MeshRenderer>().material = type switch
                        {
                            BlocType.Leaves => Leaves,
                            BlocType.Log => Log,
                            BlocType.Stone => Stone,
                            BlocType.Sand => Sand,
                            BlocType.Water => Water,
                            BlocType.Grass => Grass,
                            BlocType.Cactus => Cactus,
                            BlocType.Diamond => Diamond,
                            BlocType.Dirt => Dirt,
                            BlocType.Gravel => Gravel,
                            BlocType.Snow => Snow,
                            _ => throw new InvalidOperationException()
                        };
                    }
                }
            }
        }
    }
}