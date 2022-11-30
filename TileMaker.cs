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
    private TileBase crackedTile; // The variation of the normal tile with a lesser chance of spawning.
    [SerializeField]
    private TileBase wall; // Wall tile
    [SerializeField]
    private TileBase veryCrackedTile; // The variation of the normal tile with a very small chance of spawning.

    public void PlaceFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PlaceTiles(floorPositions, floorTilemap, floorTiles);
    }

    private void PlaceTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tiles)
    {
       foreach(var pos in positions) // The PaintTiles method pretty much loops through the PlaceSingleTile method which places our tiles one at a time.
        {
            PlaceSingleTile(tilemap, tiles, pos,crackedTile,veryCrackedTile);
        }
    }

    private void PlaceSingleTile(Tilemap tilemap, TileBase tiles, Vector2Int pos, TileBase decoration, TileBase rareDecoration)
    {
        System.Random rd = new System.Random();
        int num = rd.Next(1, 100);
        if (num < 8)
        {
            tiles = decoration;
        }else if (num > 96)
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
    internal void PlaceSingleBasicWall(Vector2Int position)
    {
        PlaceSingleTile(wallTilemap, wall, position);
    }

    public void ClearTiles()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }
}
