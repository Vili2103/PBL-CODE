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
    private TileBase floorTiles; // we will later use an array in order to generate a random tile

    public void PlaceFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PlaceTiles(floorPositions, floorTilemap, floorTiles);
    }

    private void PlaceTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tiles)
    {
       foreach(var pos in positions) // The PaintTiles method pretty much loops through the PlaceSingleTile method which places our tiles one at a time.
        {
            PlaceSingleTile(tilemap, tiles, pos);
        }
    }

    private void PlaceSingleTile(Tilemap tilemap, TileBase tiles, Vector2Int pos)
    {
        var tilePos = tilemap.WorldToCell((Vector3Int)pos);
        tilemap.SetTile(tilePos, tiles);
    }
    public void ClearTiles()
    {
        floorTilemap.ClearAllTiles();
    }
}
