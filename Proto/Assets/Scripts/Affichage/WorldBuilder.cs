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
                    if (zone[i, k, j] != BlocType.Air)
                    {
                        GameObject newCube = Instantiate(Cube, new Vector3(i, j, k), Quaternion.identity);

                        newCube.GetComponent<MeshRenderer>().material = zone[i, k, j] switch
                        {
                            BlocType.Stone => Stone,
                            BlocType.Earth => Dirt,
                            _ => throw new System.Exception()
                        };
                    }
                }
            }
        }
    }
}