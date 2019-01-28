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
}
