/*
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile
{
    //Each tile is store in a row then collumn

    //x position in tiles array
    private float x;
    public float X
    { get { return x; } }

    //y position in tiles array
    private float y;
    public float Y
    { get { return y; } }

    //array for if a direction is open
    //TOP, RIGHT, BOTTOM, LEFT
    private bool[] openDirections;
    public bool[] OpenDirections
    { get { return openDirections; } set { openDirections = value; } }

    //store a ref to the gameObejct this tile represents 
    private GameObject gameObject;
    public GameObject GameObject
    { get { return gameObject; } }

    /// <summary>
    /// Constructor
    /// </summary>
    public Tile(int a_index)
    {
        x = a_index / 6;
        y = a_index % 6;

        openDirections = new bool[4];
        for (int i = 0; i < openDirections.Length; i++)
        {
            openDirections[i] = true;
        }

        gameObject = GameObject.Find("IMG_Tile_" + a_index);
    }

    /// <summary>
    /// Rotate the tile. This will rotate 90 deg clockwise
    /// The openDirections must be change to acomadate this
    /// </summary>
    public void RotateTile()
    {
        bool[] newOpenDirections = new bool[4]
        {false, false, false, false };

        for (int i = 1; i < openDirections.Length; i++)
        {
            if(openDirections[i - 1] == true)
            {
                newOpenDirections[i] = true;
            }
        }

        //check position 0 
        if (openDirections[3] == true)
        {
            newOpenDirections[0] = true;
        }

        openDirections = newOpenDirections;
    }

    /// <summary>
    /// Change the colour of this tile's GameObject
    /// </summary>
    /// <param name="a_colour"></param>
    public void ChangeColour(Color a_colour)
    {
        gameObject.GetComponent<Image>().color = a_colour;
    }
}
