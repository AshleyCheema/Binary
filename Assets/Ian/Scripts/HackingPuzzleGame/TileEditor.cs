/*
 * 
 * Tile Editor class. Used in the editor to setup the hacking mini game.
 * 
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileEditor : MonoBehaviour
{
    public bool TopOpening;
    public bool RightOpening;
    public bool BottomOpening;
    public bool LeftOpening;

    public void SetValues(bool[] a_directions)
    {
        TopOpening = a_directions[0];
        RightOpening = a_directions[1];
        BottomOpening = a_directions[2];
        LeftOpening = a_directions[3];
    }
}
