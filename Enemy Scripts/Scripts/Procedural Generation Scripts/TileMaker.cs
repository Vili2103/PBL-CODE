using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class TileMaker : MonoBehaviour
{
    [SerializeField]  //this alows us to see the tilemap in the unity inspector
    private Tilemap floorTilemap, wallTilemap; // Different Tilemaps so  we can impelement hitboxes.
    [SerializeField]
    private TileBase floorTiles, wallTop, wallSideRight, wallSiderLeft, wallBottom, wallFull,
        wallInnerCornerDownLeft, wallInnerCornerDownRight,
        wallDiagonalCornerDownRight, wallDiagonalCornerDownLeft, wallDiagonalCornerUpRight, wallDiagonalCornerUpLeft, chest, wallBottomColumn, wallMidColumn, wallTopColumn;
    [SerializeField]
    private GameObject leftSideHitbox, rightSideHitbox, basicHitBox, chestPrefab, wallOrganizer,prefabOrganizer, enemyOrganizer, barrel,candle;
    [SerializeField]
    private List<EnemyMaker> enemiesList = new List<EnemyMaker>();
    [SerializeField]
    private List<FloorMaker> floorList = new List<FloorMaker>();
    [SerializeField]
    private FloorMaker quadFloor;
    [SerializeField]
    private List<FlagMaker> flagList = new List<FlagMaker>();

    public static HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();

    public HashSet<Vector2Int> wallSet = new HashSet<Vector2Int>();
    public static HashSet<Vector2Int> halfSet = new HashSet<Vector2Int>();
    public Dictionary<Vector2Int, GameObject> wallDict = new Dictionary<Vector2Int, GameObject>();
    public static HashSet<Vector2Int> prefabPositions = new HashSet<Vector2Int>();
   

    public void PlaceFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PlaceTiles(floorPositions, floorTilemap, floorTiles);
    }

    private void PlaceTiles(IEnumerable<Vector2Int> positions, Tilemap tilemap, TileBase tiles)
    {

        foreach (var pos in positions)  // The PlaceTiles method pretty much loops through the PlaceSingleTile method which places our tiles one at a time.
        {
            PlaceSingleTile(tilemap, pos, floorList);

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
       /* else if (WallBytes.wallTopColumn.Contains(typeAsInt))
        {
            tile = wallTopColumn;
            hitbox = basicHitBox;
        }

        else if (WallBytes.wallBottomColumn.Contains(typeAsInt))
         {
             tile = wallBottomColumn;
             hitbox = basicHitBox;

         }
        

        else if (WallBytes.wallMidColumn.Contains(typeAsInt))
         {
             tile = wallMidColumn;
             hitbox = basicHitBox;

         } */
     
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
        else if (WallBytes.wallSideRight.Contains(typeAsInt))
        {
            tile = wallSideRight;
            hitbox = rightSideHitbox;
            halfSet.Add(position);
           
        }
        else if (WallBytes.wallSideLeft.Contains(typeAsInt))
        {
            tile = wallSiderLeft;
            hitbox = leftSideHitbox;
            halfSet.Add(position);

        }

        if (tile != null)
        {
            if (hitbox == basicHitBox)
                wallSet.Add(position);
            if (tile != wallTop)
                PlaceSingleTile(wallTilemap, tile, position);
           else PlaceSingleTile(wallTilemap, position, flagList);
            PlaceHitBox(hitbox, position);

        }

    }
    private void PlaceSingleTile(Tilemap tilemap, TileBase tile, Vector2Int position)
    {
        var tilePos = tilemap.WorldToCell((Vector3Int)position);
        tilemap.SetTile(tilePos, tile);

    }

    private void PlaceSingleTile(Tilemap tilemap, Vector2Int pos, List<FloorMaker> floorList)
    {
        Tile selectedFloor = null;
        var totalSpawnRate = 0;
        int fourFloor = 0;
        var posV3 = new Vector3Int(pos.x, pos.y, 0);

       /* foreach(var dir in Direction2D.dirList)
        {
            var dirV3 = new Vector3Int(dir.x, dir.y, 0);
            if (!floorTilemap.HasTile(posV3 + dirV3)){
                fourFloor++;
                break;
            }
        }

        if (fourFloor<2)
            floorList.Add(quadFloor);
       */
        foreach (var floor in floorList)
        {
            totalSpawnRate += floor.spawnRate;
        }
        var rng = UnityEngine.Random.Range(1, totalSpawnRate + 1);
        var processedWeight = 0;
        foreach (var floor in floorList)
        {
            processedWeight += floor.spawnRate;
            if (rng <= processedWeight)
            {
                selectedFloor = floor.floorTile;
                break;
            }
        }
        
        var tilePos = tilemap.WorldToCell((Vector3Int)pos);
       // if(selectedFloor!=quadFloor)
        tilemap.SetTile(tilePos, selectedFloor);
      /*  else
        {
          var offset = new Vector3(0.5f, -0.5f, 0);
            Instantiate(quadFloor, tilePos, transform.rotation, wallOrganizer.transform);
        } */
        floorList.Remove(quadFloor); // I have to change the pivot point of the quadFloor but it does the job for now.

    }
    private void PlaceSingleTile(Tilemap tilemap, Vector2Int pos, List<FlagMaker> flagList)
    {
        Tile selectedWall = null;
        var totalSpawnRate = 0;
        foreach (var wall in flagList)
        {
            totalSpawnRate += wall.spawnRate;
        }
        var rng = UnityEngine.Random.Range(1, totalSpawnRate + 1);
        var processedWeight = 0;
        foreach (var wall in flagList)
        {
            processedWeight += wall.spawnRate;
            if (rng <= processedWeight)
            {
                selectedWall = wall.wallTile;
                break;
            }
        }

        var tilePos = tilemap.WorldToCell((Vector3Int)pos);
        tilemap.SetTile(tilePos, selectedWall);
    }
    protected void PlaceChest(Vector2Int chestPos)
    {
        var offset = new Vector3(0.5f, 0.5f, -5f); //For some reason I need this because it places it in the middle of the tiles, not where it should.
        var chestPosV3 = wallTilemap.WorldToCell((Vector3Int)chestPos);
        Instantiate(chestPrefab, chestPosV3 + offset, transform.rotation, prefabOrganizer.transform);
        prefabPositions.Add(chestPos);
    }

    public void ClearTiles()
    {

        floorTilemap.ClearAllTiles();
        wallTilemap.ClearAllTiles();
        halfSet.Clear();
        
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
        prefabPositions.Clear();

        foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            DestroyImmediate(enemy);
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
            hitbox = leftSideHitbox;
            halfSet.Add(position);

        }
        else if (WallBytes.wallInnerCornerDownRight.Contains(typeASInt))
        {
            tile = wallInnerCornerDownRight;
            hitbox = rightSideHitbox;
            halfSet.Add(position);

        }
        else if (WallBytes.wallDiagonalCornerDownLeft.Contains(typeASInt)) // We need to check since the weirdWall messed up the hitboxes for a while
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
            if (hitbox == basicHitBox)
                wallSet.Add(position);
            PlaceSingleTile(wallTilemap, tile, position);
            PlaceHitBox(hitbox, position);

        }
    }
    private void PlaceHitBox(GameObject hitBox, Vector2Int position)
    {
        var wallPos = wallTilemap.WorldToCell((Vector3Int)position); // We just find where the tile where we should be placing the hitbox should be
                                                                     // For some reason the hitbox does not appear directly on top of the tile so I needed to fiddle with the offsets of the hitbox prefabs

        var box = Instantiate(hitBox, wallPos, transform.rotation, wallOrganizer.transform); // This makes the inspector very very crowded but it's the best thing I can think of right now.
        var comp = box.GetComponent<BoxCollider2D>();
     //   comp.usedByComposite = true; // This slows down generation by about 20 - 30 seconds but is really neat when we want to use the cinemachine confine tool so we'll keep it for now.
        
        if (wallDict.ContainsKey(position))
        {
            GameObject.DestroyImmediate(wallDict[position]);
            wallDict.Remove(position);
            wallDict.Add(position, box);
        }
        else
        {
            wallDict.Add(position, box);
        }

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
                int blockedTiles = 0;
                Vector2Int chestPos = getRandomPos(minX, maxX, minY, maxY);
                if (CorridorFirstDungeonGenerator.startRoomPos.Contains(chestPos) == true ||
                    CorridorFirstDungeonGenerator.bossRoomPos.Contains(chestPos) == true)
                {
                    break;
                }

                foreach (var dir in Direction2D.dirList)
                {
                    if (wallTilemap.HasTile((Vector3Int)(chestPos + dir)))
                        blockedTiles++; // So we dont spawn chests surrounded by walls. 

                }

                if (floorTilemap.HasTile((Vector3Int)chestPos) && CorridorFirstDungeonGenerator.corridorPositions.Contains(chestPos) == false) // check if the position where the chest is being spawned is a floor tile
                                                                                                                                               //Its also very important that we check that the chest isn't a part of the corridor, so we don't block off rooms unintetionaly.
                {
                    if (blockedTiles < 2)
                    {
                        PlaceChest(chestPos);
                        hasChest = true;
                    }

                }
            }
            hasChest = false;

        }
    }

    public static Vector2Int getRandomPos(int minX, int maxX, int minY, int maxY)
    {
        int x = UnityEngine.Random.Range(minX + 10, maxX - 10);
        int y = UnityEngine.Random.Range(minY + 10, maxY - 10);
        Vector2Int pos = new Vector2Int(x, y);
        return pos;
    }

    public void placBarrelsCandles()
    {

        var rand = new System.Random();
        int barrelNum = 0;
        int candleNum = 0;
        Vector2Int pos = new Vector2Int();
        var offset = new Vector3(0.5f, 0.5f, -5f); //For some reason I need this because it places it in the middle of the tiles, not where it should.


        foreach (var room in CorridorFirstDungeonGenerator.rooms)
        {
            HashSet<Vector2Int> roomTiles = room.Value;
            barrelNum = rand.Next(7, 20);//Should probably make it so that it scales with room size with (roomTiles.Count / x)  but don't know x for now.
            candleNum = rand.Next(1, 5);

            while (barrelNum > 0)
            {
                
                pos = roomTiles.ElementAt(rand.Next(0, roomTiles.Count - 1));

                if (CorridorFirstDungeonGenerator.startRoomPos.Contains(pos) == true ||
                    CorridorFirstDungeonGenerator.bossRoomPos.Contains(pos) == true)
                {
                    break;
                }
                
                if (!CorridorFirstDungeonGenerator.corridorPositions.Contains(pos) && !prefabPositions.Contains(pos))
                {
                    var barrelPosV3 = wallTilemap.WorldToCell((Vector3Int)pos);
                    Instantiate(barrel, barrelPosV3 + offset, transform.rotation, prefabOrganizer.transform);
                    barrelNum--;
                    prefabPositions.Add(pos);
                }

            }
            while (candleNum > 0)
            {
                pos = roomTiles.ElementAt(rand.Next(0, roomTiles.Count - 1));
                if (CorridorFirstDungeonGenerator.startRoomPos.Contains(pos) == true ||
                   CorridorFirstDungeonGenerator.bossRoomPos.Contains(pos) == true)
                {
                    break;
                }
    
                if (!CorridorFirstDungeonGenerator.corridorPositions.Contains(pos) && !prefabPositions.Contains(pos))
                {
                    var candlePosV3 = wallTilemap.WorldToCell((Vector3Int)pos);
                    Instantiate(candle, candlePosV3 + offset, transform.rotation, prefabOrganizer.transform);
                    candleNum--;
                    prefabPositions.Add(pos);
                }

            }

        }
    }
    
  

    public void PlacePrefabs()
    {
        PlaceChests();
         placBarrelsCandles();
        
        PlaceEnemies.placeEnemies(floorTilemap, wallTilemap, enemiesList, enemyOrganizer);
        //more to be added
    }


}
