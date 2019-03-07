/*
* Author: Ian Hudson
* Description: Tile object for the hacking mini game.
* Created: 05/02/2019
* Edited By: Ian
*/
using UnityEngine;
using UnityEngine.UI;

public class Tile
{
    //Each tile is store in a row then column

    //x position in tiles array (row)
    private float x;
    public float X
    { get { return x; } }

    //y position in tiles array (column)
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
    //AStar - Cost from start tile to this tile
    private int gCost;
    public int GCost
    { get { return gCost; } set { gCost = value; } }

    //AStar - Cost from this tile to the end tile
    private int hCost;
    public int HCost
    { get { return hCost; } set { hCost = value; } }

    //AStar - Total cost of this tile
    public int FCost
    { get { return GCost + HCost; } }

    //AStar - If on the path set a parent to retrace the path
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
    /// The openDirections are changed to acomadate this
    /// </summary>
    public void RotateTile()
    {
        //setup a new bool array for the new directions
        bool[] newOpenDirections = new bool[4]
        {false, false, false, false };

        //Loop though RIGHT, BOTTOM and LEFT directions
        //If TOP direction is true then set RIGHT to true etc..
        for (int i = 1; i < openDirections.Length; i++)
        {
            if(openDirections[i - 1] == true)
            {
                newOpenDirections[i] = true;
            }
        }

        //check if LEFT is true then set TOP to true
        if (openDirections[3] == true)
        {
            newOpenDirections[0] = true;
        }

        //Set openDirections to newOpenDirections
        openDirections = newOpenDirections;
        //Rotate the gameObejct's rotation by 90 deg
        gameObject.transform.rotation *= Quaternion.Euler(0, 0, -90);
        //Assign the editor script values to openDirections
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
