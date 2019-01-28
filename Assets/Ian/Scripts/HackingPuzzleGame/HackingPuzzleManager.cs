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
        Tile cTile = openTiles[0];

        while (openTiles.Count > 0)
        {
            openTiles.Remove(cTile);
            closedTiles.Add(cTile);

            if(cTile == endTile)
            {
                Debug.Break();
            }

            List<Tile> neighbours = GetNeighbours(cTile);
            for (int i = 0; i < neighbours.Count; i++)
            {
                if(!closedTiles.Contains(neighbours[i]) && 
                    !openTiles.Contains(neighbours[i]))
                {
                    openTiles.Add(neighbours[i]);
                }
            }

            if (openTiles.Count > 0)
            {
                cTile = openTiles[0];
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
        int tileX = (int)a_tile.X;
        int tileY = (int)a_tile.Y;

        if (tileY - 1 >= 0)
        {
            //check top
            if (a_tile.OpenDirections[0] == true && tiles[tileX, tileY - 1].OpenDirections[2] == true)
            {
                returnNeighbours.Add(tiles[tileX, tileY - 1]);
            }
        }

        if (tileY + 1 < 4)
        {
            //check bottom
            if (a_tile.OpenDirections[2] == true && tiles[tileX, tileY + 1].OpenDirections[0] == true)
            {
                returnNeighbours.Add(tiles[tileX, tileY + 1]);
            }
        }

        if (tileX - 1 >= 0)
        {
            //check left
            if (a_tile.OpenDirections[3] == true && tiles[tileX - 1, tileY].OpenDirections[1] == true)
            {
                returnNeighbours.Add(tiles[tileX - 1, tileY]);
            }
        }

        if (tileX + 1 < 4)
        {
            //check right
            if (a_tile.OpenDirections[1] == true && tiles[tileX + 1, tileY].OpenDirections[3] == true)
            {
                returnNeighbours.Add(tiles[tileX + 1, tileY]);
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
