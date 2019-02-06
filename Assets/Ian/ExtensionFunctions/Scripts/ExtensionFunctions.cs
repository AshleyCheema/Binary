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
}
