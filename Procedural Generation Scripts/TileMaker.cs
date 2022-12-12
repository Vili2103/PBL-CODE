using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileMaker : MonoBehaviour
{
    [SerializeField]  //this alows us to see the tilemap in the unity inspector
    private Tilemap floorTilemap, wallTilemap; // Different Tilemap so  we can impelement hitboxes.
    [SerializeField]
    private TileBase floorTiles, crackedFloor, veryCrackedFloor, wallTop, wallSideRight, wallSiderLeft, wallBottom, wallFull,
        wallInnerCornerDownLeft, wallInnerCornerDownRight,
        wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft;
    [SerializeField]
    private GameObject leftSideHitbox, rightSideHitbox, basicHitBox;

    public void PlaceFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PlaceTiles(floorPositions, floorTilemap, floorTiles);
    }

    private void PlaceTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tiles)
    {
        foreach (var pos in positions)  // The PlaceTiles method pretty much loops through the PlaceSingleTile method which places our tiles one at a time.
        {
            PlaceSingleTile(tilemap, tiles, pos, crackedFloor, veryCrackedFloor);  // CALLS THE OVERLOADED METHOD (THE ONE THAT TAKES MORE PARAMETERS)
        }
    }

    internal void PaintSingleBasicWall(Vector2Int position, string binaryType)
    {
        int typeAsInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        GameObject hitbox = null;
        if (WallBytes.wallTop.Contains(typeAsInt))
        {
            tile = wallTop;
            hitbox = basicHitBox;
            
        }
        else if (WallBytes.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
            hitbox = rightSideHitbox;
            
        }
        else if (WallBytes.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSiderLeft;
            hitbox = leftSideHitbox;
        }
        else if (WallBytes.wallBottm.Contains(typeAsInt))
        {
            tile = wallBottom;
            hitbox = basicHitBox;
        }
        else if (WallBytes.wallFull.Contains(typeAsInt))
        {
            tile = wallFull;
            hitbox = basicHitBox;
        }

        if (tile != null)
        {
            PaintSingleTile(wallTilemap, tile, position);
            PlaceHitBox(hitbox, position);
        }
            
    }

    private void PaintSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePos = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePos, tile);
        
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


    public void ClearTiles()
    {
        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
    }

    public void DeleteHitboxes()
    {
        foreach (GameObject hitbox in GameObject.FindGameObjectsWithTag("Hitbox"))
        {
            DestroyImmediate(hitbox);
        }
    }

    internal void PlaceSingleCornerWall(Vector2Int position, string binaryType)
    {
        /* WE CHECK THE POSITIONS OF THE FLOORS AND WALLS THAT NEIGHBOUR OUR GIVEN TILE. THEN, WE SEE WHAT TILE WE SHOULD PLACE (FROM Wall Bytes) */

        int typeASInt = Convert.ToInt32(binaryType, 2);
        TileBase tile = null;
        GameObject hitbox = null;

        if (WallBytes.wallInnerCornerDownLeft.Contains(typeASInt))
        {
            tile = wallInnerCornerDownLeft;
            hitbox = basicHitBox;

        }
        else if (WallBytes.wallInnerCornerDownRight.Contains(typeASInt))
        {
            tile = wallInnerCornerDownRight;
            hitbox = basicHitBox;
        }
        else if (WallBytes.wallDiagonalCornerDownLeft.Contains(typeASInt))
        {
            tile = wallDiagonalCornerDownLeft;
            hitbox = basicHitBox;
        }
        else if (WallBytes.wallDiagonalCornerDownRight.Contains(typeASInt))
        {
            tile = wallDiagonalCornerDownRight;
            hitbox = basicHitBox;
        }
        else if (WallBytes.wallDiagonalCornerUpRight.Contains(typeASInt))
        {
            hitbox = basicHitBox;
            tile = wallDiagonalCornerUpRight;
        }
        else if (WallBytes.wallDiagonalCornerUpLeft.Contains(typeASInt))
        {
            tile = wallDiagonalCornerUpLeft;
            hitbox = basicHitBox;
        }
        else if (WallBytes.wallFullEightDirections.Contains(typeASInt))
        {
            tile = wallFull;
            hitbox = basicHitBox;
        }
        else if (WallBytes.wallBottmEightDirections.Contains(typeASInt))
        {
            tile = wallBottom;
            hitbox = basicHitBox;   
        }

        if (tile != null)
        {
            PaintSingleTile(wallTilemap, tile, position);
          PlaceHitBox(hitbox, position);

        }
           
    }

    private void PlaceHitBox(GameObject hitBox,Vector2Int position)
    {
        var wallPos = wallTilemap.WorldToCell((Vector3Int)position); // We just find where the tile where we should be placing the hitbbox should be
        // For some reason the hitbox does not appear directly on top of the tile so I needed to fiddle with the offsets of the hitbox prefabs
            Instantiate(hitBox,wallPos,transform.rotation); // This makes the inspector very very crowded but it's the best thing I can think of right now.

    }
}

