using UnityEngine;

internal static class VectorHelper
{
    public static Vector3 WithX(this Vector3 v, float x) => new(x, v.y, v.z);

    public static Vector3 WithY(this Vector3 v, float y) => new(v.x, y, v.z);

    public static Vector3 WithZ(this Vector3 v, float z) => new(v.x, v.y, z);
}