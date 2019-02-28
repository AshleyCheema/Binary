using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionFunctions
{
    public static void Print(this Debug a_debug, string a_message)
    {
#if UNITY_EDITOR
        Debug.Log(a_message);
#endif
    }

    /// <summary>
    /// Extension Method for material to change the colour of an object
    /// </summary>
    /// <param name="a_material"></param>
    /// <param name="a_color"></param>
    public static void SetColor(this Material a_material, Color a_color)
    {
        if (a_material.HasProperty("_BaseColor"))
        {
            a_material.SetColor("_BaseColor", a_color);
        }
    }

    public static Vector3 RandomPointInBounds(Bounds a_bounds)
    {
        return new Vector3(
            UnityEngine.Random.Range(a_bounds.min.x, a_bounds.max.x),
            UnityEngine.Random.Range(a_bounds.min.y, a_bounds.max.y),
            UnityEngine.Random.Range(a_bounds.min.z, a_bounds.max.z)
            );
    }

    public static Vector3 GetVector3(this Vector3 a_vector3, LLAPI.SpawnableObject a_object)
    {
        return new Vector3(a_object.XPos, a_object.YPos, a_object.ZPos);
    }
}
