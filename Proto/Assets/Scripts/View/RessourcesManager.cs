using System;
using UnityEngine;

public class RessourcesManager : MonoBehaviour
{
    public GameObject cube;

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

    private static RessourcesManager Instance;

    public static GameObject Cube => Instance.cube;

    public static Material GetMaterial(BlocType type)
    {
        return type switch
        {
            BlocType.Leaves => Instance.Leaves,
            BlocType.Log => Instance.Log,
            BlocType.Stone => Instance.Stone,
            BlocType.Sand => Instance.Sand,
            BlocType.Water => Instance.Water,
            BlocType.Grass => Instance.Grass,
            BlocType.Cactus => Instance.Cactus,
            BlocType.Diamond => Instance.Diamond,
            BlocType.Dirt => Instance.Dirt,
            BlocType.Gravel => Instance.Gravel,
            BlocType.Snow => Instance.Snow,
            _ => throw new InvalidOperationException()
        };
    }

    public void Awake() => Instance = this;
}