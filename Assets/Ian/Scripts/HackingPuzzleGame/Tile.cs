/*
 * 
 * Tile object for the hacking mini game.
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

    //Is this tile the start tile for the puzzle
    private bool isStart;
    public bool IsStart
    { get { return isStart; } }

    //Is this tile the end tile for the puzzle
    private bool isEnd;
    public bool IsEnd
    { get { return isEnd; } }

    //array for if a direction is open
    //TOP, RIGHT, BOTTOM, LEFT
    private bool[] openDirections;
    public bool[] OpenDirections
    { get { return openDirections; } set { openDirections = value; } }

    //store a ref to the gameObejct this tile represents 
    private GameObject gameObject;
    public GameObject GameObject
    { get { return gameObject; } }

    //Variables for pathfind on the grid
    private int gCost;
    public int GCost
    { get { return gCost; } set { gCost = value; } }

    private int hCost;
    public int HCost
    { get { return hCost; } set { hCost = value; } }

    public int FCost
    { get { return GCost + HCost; } }

    private Tile parent;
    public Tile Parent
    { get { return parent; } set { parent = value; } }

    /// <summary>
    /// Constructor
    /// </summary>
    public Tile(int a_index)
    {
        x = a_index / 6;
        y = a_index % 6;

        gameObject = GameObject.Find("IMG_Tile_" + a_index);

        openDirections = new bool[4];
        for (int i = 0; i < openDirections.Length; i++)
        {
            openDirections[i] = false;
        }
        TileEditor te = gameObject.GetComponent<TileEditor>();

        isStart = te.IsStart;
        isEnd = te.IsEnd;

        if (te.TopOpening)
        {
            openDirections[0] = true;
        }
        if (te.RightOpening)
        {
            openDirections[1] = true;
        }
        if (te.BottomOpening)
        {
            openDirections[2] = true;
        }
        if (te.LeftOpening)
        {
            openDirections[3] = true;
        }

        int randomSteps = Random.Range(0, 4);
        for (int i = 0; i < randomSteps; i++)
        {
            RotateTile();
        }
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
        gameObject.transform.rotation *= Quaternion.Euler(0, 0, -90);
        gameObject.GetComponent<TileEditor>().SetValues(openDirections);
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
