/*
 * 
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HackingPuzzleManager : MonoBehaviour
{
    //private tile array for all the tiles in the puzzle
    private Tile[,] tiles;

    //define the start and end tiles 
    private Tile startTile;
    private Tile endTile;

    // Use this for initialization
    void Start()
    {
        //create a new tile array and assign each tile to a new tile
        tiles = new Tile[4, 6];
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                tiles[i, j] = new Tile(i * 6 + j);
            }
        }

        startTile = tiles[0, 0];
        endTile = tiles[3, 5];

        startTile.ChangeColour(Color.blue);
        endTile.ChangeColour(Color.red);

        startTile.OpenDirections = new bool []
        { false, true, false, false };
        tiles[0, 1].OpenDirections = new bool[]
        {false, false, false, true};
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartPuzzle();
        }
    }

    /// <summary>
    /// Start the puzzle. This will go though all the tiles and check for a path to the 
    /// end tile
    /// </summary>
    private void StartPuzzle()
    {
        //Store all the tiles which need to be updated
        List<Tile> openTiles = new List<Tile>();
        //Store all the tiles that have been updated
        List<Tile> closedTiles = new List<Tile>();

        openTiles.Add(startTile);

        while(openTiles.Count > 0)
        {
            Tile cTile = openTiles[0];
            openTiles.Remove(cTile);
            closedTiles.Add(cTile);

            List<Tile> neighbours = GetNeighbours(cTile);
            for (int i = 0; i < neighbours.Count; i++)
            {
                if(!openTiles.Contains(neighbours[i]))
                {
                    openTiles.Add(neighbours[i]);
                }
            }
        }

        for (int i = 0; i < closedTiles.Count; i++)
        {
            closedTiles[i].ChangeColour(Color.green);
        }
    }

    /// <summary>
    /// Return a list of tiles which are valid 
    /// </summary>
    /// <param name="a_tile"></param>
    /// <returns></returns>
    private List<Tile> GetNeighbours(Tile a_tile)
    {
        List<Tile> returnNeighbours = new List<Tile>();
        float tileX = 0;
        float tileY = 0;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0 || x == y)
                {
                    continue;
                }

                tileX = a_tile.X + x;
                tileY = a_tile.Y + y;

                //check top
                if (tileY >= 0 && tileY < 4 && tileX >= 0 && tileX < 5)
                {
                    if (a_tile.OpenDirections[0] == true && tiles[x, y].OpenDirections[2] == true)
                    {
                        returnNeighbours.Add(tiles[x, y]);
                    }

                    //check right
                    if (a_tile.OpenDirections[1] == true && tiles[x, y].OpenDirections[3] == true)
                    {
                        returnNeighbours.Add(tiles[x, y]);
                    }
                }
            }
        }
        return returnNeighbours;
    }

    /// <summary>
    /// Call when a tile has been clicked
    /// </summary>
    public void TileClicked(GameObject a_tile)
    {
        Tile t = FindTileFromGameObject(a_tile);
        t.RotateTile();


        Debug.Log("Tile was clicked: " + t.X + " : " + t.Y);
    }

    /// <summary>
    /// Find a tile in the tiles array from a GameObject
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    private Tile FindTileFromGameObject(GameObject a_gameObject)
    {
        for (int i = 0; i < tiles.GetLength(0); i++)
        {
            for (int j = 0; j < tiles.GetLength(1); j++)
            {
                if(tiles[i, j].GameObject == a_gameObject)
                {
                    return tiles[i, j];
                }
            }
        }

        //no tile with a_gameObject was found
        return null;
    }
}
