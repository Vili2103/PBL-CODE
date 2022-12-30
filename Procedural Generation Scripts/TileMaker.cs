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
        wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft, chest;
    [SerializeField]
    private GameObject leftSideHitbox, rightSideHitbox, basicHitBox, chestPrefab,enemyPrefab,organizer;
    [SerializeField]
    private List<EnemyMaker> enemiesList = new List<EnemyMaker>();


    public HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

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
        PlacePrefabs();
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
            PlaceSingleTile(wallTilemap, tile, position);
            PlaceHitBox(hitbox, position);
            wallPositions.Add(position);
        }

    }
    private void PlaceSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
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
    protected void PlaceChest(Vector2Int chestPos)
    {
        var offset = new Vector3(0.5f, 0.5f, 0); //For some reason I need this because it places it in the middle of the tiles, not where it should.
        var chestPosV3 = wallTilemap.WorldToCell((Vector3Int)chestPos);
        Instantiate(chestPrefab, chestPosV3 + offset, transform.rotation,organizer.transform);
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
    public void DeletePrefabs()
    {
        foreach (GameObject prefab in GameObject.FindGameObjectsWithTag("Prefab"))
        {
            DestroyImmediate(prefab);
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
            PlaceSingleTile(wallTilemap, tile, position);
            PlaceHitBox(hitbox, position);

        }
    }
    private void PlaceHitBox(GameObject hitBox, Vector2Int position)
    {
        var wallPos = wallTilemap.WorldToCell((Vector3Int)position); // We just find where the tile where we should be placing the hitbox should be
        // For some reason the hitbox does not appear directly on top of the tile so I needed to fiddle with the offsets of the hitbox prefabs
        Instantiate(hitBox, wallPos, transform.rotation,organizer.transform); // This makes the inspector very very crowded but it's the best thing I can think of right now.
     
    }
    public void PlaceChests()
    {
        foreach (var room in CorridorFirstDungeonGenerator.rooms)
        {
    
            HashSet<Vector2Int> roomTiles = room.Value;
            bool hasChest = false;
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            int maxX = int.MinValue;
            foreach (var tile in roomTiles)
            {
                if (tile.x < minX)
                    minX = tile.x;

                if (tile.y < minY)
                    minY = tile.y;

                if (tile.x > maxX)
                    maxX = tile.x;

                if (tile.y > maxY)
                    maxY = tile.y;
            }

            while (hasChest == false)
            {
                Vector2Int chestPos = getRandomPos(minX, maxX, minY, maxY);
                if (CorridorFirstDungeonGenerator.startRoomPos.Contains(chestPos) == true || 
                    CorridorFirstDungeonGenerator.bossRoomPos.Contains(chestPos)==true)
                {
                    break;
                }
                if (floorTilemap.HasTile((Vector3Int)chestPos) && CorridorFirstDungeonGenerator.corridorPositions.Contains(chestPos) == false) // check if the position where the chest is being spawned is a floor tile
                 //Its also very important that we check that the chest isn't a part of the corridor, so we don't block off rooms unintetionaly.
                {
                    PlaceChest(chestPos);
                    hasChest = true;
                }
            }
            hasChest = false;
            //This is a bit clunky but recursion causes stackoverflow and this works sooo yeah.
        }
    }

     public static Vector2Int getRandomPos(int minX, int maxX,int minY, int maxY)
    {
        int x = UnityEngine.Random.Range(minX, maxX);
        int y = UnityEngine.Random.Range(minY, maxY);
        Vector2Int pos = new Vector2Int(x, y);
        return pos;
    }

    public void PlacePrefabs()
    {
        PlaceChests();
        // PlaceEnemies.placeEnemies(floorTilemap, wallTilemap, enemiesList); //Unity dies after spawing enemies because it isnt very optimmized!
        //more to be added
    }
   
}
                                                                                                