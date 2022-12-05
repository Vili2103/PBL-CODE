using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMaker : MonoBehaviour
{
    [SerializeField] //this alows us to see the tilemap in the unity inspector
    private Tilemap floorTilemap;
    [SerializeField]
    private Tilemap wallTilemap; // Different Tilemap so  we can impelement hitboxes.
    [SerializeField]
    private TileBase floorTiles; // Basic floor tiles
    [SerializeField]
    protected TileBase crackedTile; // The variation of the normal tile with a lesser chance of spawning.
    [SerializeField]
    private TileBase wall; // Wall tile
    [SerializeField]
    protected TileBase veryCrackedTile; // The variation of the normal tile with a very small chance of spawning.
    [SerializeField]
    protected TileBase wallSideRight, wallSideLeft, wallCornerDownLeft, wallCornerUpLeft, wallCornerDownRight, wallCornerUpRight, wallFliped;
    //[SerializeField]
    // protected TileBase ruleTile;


    public void PlaceFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PlaceTiles(floorPositions, floorTilemap, floorTiles);
    }

    private void PlaceTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tiles)
    {
        foreach (var pos in positions) // The PaintTiles method pretty much loops through the PlaceSingleTile method which places our tiles one at a time.
        {
            PlaceSingleTile(tilemap, tiles, pos, crackedTile, veryCrackedTile); // CALLS THE OVERLOADED METHOD (THE ONE THAT TAKES MORE PARAMETERS)
        }
    }

    internal void PlaceSingleCornerWall(Vector2Int pos, string neighboursType)
    {
        /* WE CHECK THE POSITIONS OF THE FLOORS AND WALLS THAT NEIGHBOUR OUR GIVEN TILE. THEN, WE SEE WHAT
  * TILE WE SHOULD PLACE (FROM Wall Bytes) */
        int byteInInt = Convert.ToInt32(neighboursType, 2);
        TileBase tile = null;
        if (WallBytes.wallDiagonalCornerUpLeft.Contains(byteInInt))
        {
            tile = wallCornerUpLeft;
        }
        else if (WallBytes.wallDiagonalCornerDownLeft.Contains(byteInInt))
        {

            tile = wall;
        }
        else if (WallBytes.wallDiagonalCornerDownRight.Contains(byteInInt))
        {

            tile = wall;

        }
        else if (WallBytes.wallDiagonalCornerUpRight.Contains(byteInInt))
        {
            tile = wallCornerUpRight;
        }
        else if (WallBytes.wallSideLeft.Contains(byteInInt))
        {
            tile = wallSideLeft;
        }
        else if (WallBytes.wallSideRight.Contains(byteInInt))
        {
            tile = wallSideRight;
        }
        else if (WallBytes.wallBottmEightDirections.Contains(byteInInt))
        {
            tile = wall;
        }
        else if (WallBytes.wallBottm.Contains(byteInInt))
        {
            tile = wall;
        }
    if (tile != null)
            PlaceSingleTile(wallTilemap, tile, pos);
      
    }
    

    private void PlaceSingleTile(Tilemap tilemap, TileBase tiles, Vector2Int pos, TileBase decoration, TileBase rareDecoration)
    {
        System.Random rd = new System.Random();
        int num = rd.Next(1, 100);
        if (num < 4)
        {
            tiles = decoration;
        }
        else if (num > 98) // GOOFY WAY OF DOING WEIGHTED RANDOM TILES BUT IT WORKS :)
        {
            tiles = rareDecoration;
        }
        var tilePos = tilemap.WorldToCell((Vector3Int)pos);
        tilemap.SetTile(tilePos, tiles);
    }

    private void PlaceSingleTile(Tilemap tilemap, TileBase tiles, Vector2Int pos) //overloading the method for tiles which dont have any decoration
                                                                                 // like walls, for now.
    {

        var tilePos = tilemap.WorldToCell((Vector3Int)pos);
        tilemap.SetTile(tilePos, tiles);
    }
    internal void PlaceSingleBasicWall(Vector2Int position, string binaryType)
    {
        /* WE CHECK THE POSITIONS OF THE FLOORS AND WALLS THAT NEIGHBOUR OUR GIVEN TILE. THEN, WE SEE WHAT
         * TILE WE SHOULD PLACE (FROM Wall Bytes) */
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        if (WallBytes.wallTop.Contains(typeAsInt))
        {
            tile = wall;
        }
        else if (WallBytes.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
        }
        else if (WallBytes.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSideLeft;
        }
        else if (WallBytes.wallBottm.Contains(typeAsInt))
        {
            tile = wall;
        }
        else if (WallBytes.wallFull.Contains(typeAsInt))
        {
            tile = wall;
        }
        if (tile != null)
            PlaceSingleTile(wallTilemap, tile, position);
    }

    public void ClearTiles()
    {
        floorTilemap.ClearAllTiles(); // DELETES ALL TILES FROM TILEMAPS WHEN CREATING A NEW DUNGEON 
        wallTilemap.ClearAllTiles();
    }
}
    