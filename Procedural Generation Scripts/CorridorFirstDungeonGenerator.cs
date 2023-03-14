using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CorridorFirstDungeonGenerator : RandomWalkMapGen
{
    [SerializeField]
    private Grid grid;
    [SerializeField]
    Tilemap spawnRoom, bossRoom, wallTilemap;
    [SerializeField]
    private int length, corridorCount;
    [SerializeField]
    GameObject doorPrefab, organizer, bigDoorPrefab;

    public static HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();

    public static HashSet<Vector2Int> doorPositions = new HashSet<Vector2Int>();
    public static HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();
    public static HashSet<Vector2Int> rotatedDoorPositions = new HashSet<Vector2Int>();

    public static Dictionary<Vector2Int, HashSet<Vector2Int>> rooms = new Dictionary<Vector2Int, HashSet<Vector2Int>>();
    public static HashSet<Vector2Int> corridorPositions = new HashSet<Vector2Int>();
    public static HashSet<Vector2Int> startRoomPos = new HashSet<Vector2Int>();
    public static HashSet<Vector2Int> bossRoomPos = new HashSet<Vector2Int>();


    protected override void RunPPG()
    {
        CorridorFirstGeneration();
     //   getPotentialDoorPositions(); // Needs major changes so that it can finally work. Work in progress!!

    }

    private void CorridorFirstGeneration()
    {

        HashSet<Vector2Int> potentialRoomPos = new HashSet<Vector2Int>();


        makeCorridors(floorPos, potentialRoomPos, spawnRoom);

        HashSet<Vector2Int> roomPos = CreateRooms(potentialRoomPos);

        List<Vector2Int> deadEnds = FindDeadEnds(floorPos);

        CreateRoomsAtDeadEnd(deadEnds, roomPos);

        floorPos.UnionWith(roomPos);

        tileMaker.PlaceFloorTiles(floorPos);
        WallMaker.CreateWalls(floorPos, tileMaker);

    }

    private void CreateRoomsAtDeadEnd(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomPositions)
    {
        foreach (var pos in deadEnds)
        {
            if (roomPositions.Contains(pos) == false)
            {
                var room = RunRandomWalk(randomWalkParams, pos);
                roomPositions.UnionWith(room);
                rooms[pos] = room;

            }
        }
    }

    private List<Vector2Int> FindDeadEnds(HashSet<Vector2Int> floorPos)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();

        foreach (var pos in floorPos)
        {
            int neighboursCount = 0;
            foreach (var dir in Direction2D.dirList)
            {
                if (floorPos.Contains(pos + dir))
                {
                    neighboursCount++;
                }

            }
            if (neighboursCount == 1)
            {
                deadEnds.Add(pos);
            }

        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPos)
    {
        setStartRoomPos();

        foreach (var pos in startRoomPos)
        {
            roomPositions.UnionWith(startRoomPos);
            rooms[pos] = startRoomPos;
            if (potentialRoomPos.Contains(pos))
                potentialRoomPos.Remove(pos); // To ensure that we dont spawn another room on top of our spawn. 
        }

        int roomCount = Mathf.RoundToInt(potentialRoomPos.Count);

        List<Vector2Int> roomsToCreate = potentialRoomPos.OrderBy(x => Guid.NewGuid()).Take(roomCount).ToList(); //Gui = Globaly Unique Identifier x=> is a LAMBDA expression 

        Vector2Int bossPos = getMaxDistance(roomsToCreate);
        Vector3 bossRoomPosV3 = new Vector3(bossPos.x, bossPos.y, 0);
        roomsToCreate.Remove(bossPos);
        bossRoom.transform.position = bossRoomPosV3; //We move the bossroom and then draw over it so that it connects to corridors etc.
        // Instantiate(bossRoom,bossRoomPosV3, transform.rotation, grid.transform); // We set grid as the parent or else the room doesn't spawn (unity requires it)
        setBossRoomPos();


        foreach (var pos in bossRoomPos)
        {
            roomPositions.UnionWith(bossRoomPos);
            rooms[pos] = bossRoomPos;
            if (roomsToCreate.Contains(pos))
                roomsToCreate.Remove(pos); //So we don't have multiple rooms on top of each other
        }

        foreach (var roomPos in roomsToCreate)
        {

            var room = RunRandomWalk(randomWalkParams, roomPos);
            rooms[roomPos] = room; //Dictionary that stores Vector2Int values as the keys to our rooms. Very neat very nice this is how we divide each room!
            roomPositions.UnionWith(room); // BUILT IN HASHSET METHOD!!!! WOW!!!
        }

        return roomPositions;
    }


    private void makeCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPos, Tilemap startRoomTilemap)
    {
        //RANDOM WALK CORRIDORS WITH THE STARTING POINT BEING startPos
        var pos = startPos;

        potentialRoomPos.Add(pos);

        for (int i = 0; i < corridorCount; i++)
        {

            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(pos, length);
            corridorPositions.UnionWith(corridor);
            pos = corridor[corridor.Count - 1];

            potentialRoomPos.Add(pos);

            floorPositions.UnionWith(corridor); //HASHSET BUILT IN METHOD WOW!!!
        }

    }

    private void setStartRoomPos()
    {
        for (int x = -50; x < 50; x++)
        {
            for (int y = -50; y < 50; y++)
            {
                Vector3Int selectedTile = (new Vector3Int(x, y, (int)spawnRoom.transform.position.y));
                Vector3 place = spawnRoom.CellToWorld(selectedTile);
                if (spawnRoom.HasTile(selectedTile))
                {
                    //Tile at "place"
                    Vector2 pos = (Vector2)place;
                    Vector2Int posInt = Vector2Int.RoundToInt(pos);
                    startRoomPos.Add(posInt);
                }
            }
        }
    }

    private void setBossRoomPos()
    {
        foreach (var position in bossRoom.cellBounds.allPositionsWithin) // might be a bit inefficient but it works so yeah.
        {

            if (bossRoom.HasTile(position))
            {
                Vector3 pos = bossRoom.CellToWorld(position);
                Vector2 pos2 = pos;
                Vector2Int pos2Int = Vector2Int.RoundToInt(pos2);
                bossRoomPos.Add(pos2Int);
            }
        }

    }

    private Vector2Int getMaxDistance(List<Vector2Int> roomsToCreate) // We find the most distant room from the spawn. We make it the boss room later in the script
    {
        Vector2Int startPos = new Vector2Int(0, 0);
        float[] dist = new float[roomsToCreate.Count];
        int i = 0;
        Dictionary<float, Vector2Int> distances = new Dictionary<float, Vector2Int>();
        foreach (var roomPos in roomsToCreate)
        {
            dist[i] = Vector2Int.Distance(roomPos, startPos);
            distances[dist[i]] = roomPos;
            i++;
        }
        Array.Sort(dist);
        Array.Reverse(dist);
        return distances[dist[0]];

    }
    public void getPotentialDoorPositions()
    {
        Vector2Int up = new Vector2Int(0, 1);

        Vector2Int down = new Vector2Int(0, -1);

        Vector2Int left = new Vector2Int(-1, 0);

        Vector2Int right = new Vector2Int(1, 0);

        var offset = new Vector3(0.5f, 0.5f, 0);

        foreach (var pos in corridorPositions)
        {
            Vector3 posV3 = new Vector3(pos.x, pos.y, 0);

            if ((!floorPos.Contains(pos + up) && !floorPos.Contains(pos + down)) && (!roomPositions.Contains(pos + up) || !roomPositions.Contains(pos + down)))
            {
                if (roomPositions.Contains(pos + left + down) || roomPositions.Contains(pos + left + up) || roomPositions.Contains(pos + right + down)

                    || roomPositions.Contains(pos + right + up))//This exists so we don't spawn a door on every corridor tile that is not in a room.
                {
                    if (!doorPositions.Contains(pos + down + down) && !doorPositions.Contains(pos + up + up) && !doorPositions.Contains(pos + left + left)

                        && !doorPositions.Contains(pos + right + right))
                        doorPositions.Add(pos);


                }
            }
            else if ((!floorPos.Contains(pos + left) && !floorPos.Contains(pos + right)) && (!roomPositions.Contains(pos + left) || !roomPositions.Contains(pos + right)))
            {
                if (roomPositions.Contains(pos + left + down) || roomPositions.Contains(pos + left + up) || roomPositions.Contains(pos + right + down)
                   || roomPositions.Contains(pos + right + up))
                    doorPositions.Add(pos);


            }

        }
        PlaceDoors();
    }

    public void PlaceDoors()
    {
        var set = new HashSet<Vector2Int>();
        var actualDoorPos = new HashSet<Vector2Int>();

        Vector2Int left = new Vector2Int(-1, 0);
        Vector2Int up = new Vector2Int(0, 1);
        Vector2Int right = new Vector2Int(1, 0);
        Vector2Int down = new Vector2Int(0, -1);
        bool leftMost = false;
        bool rightMost = false;
        bool downMost = false;
        bool upMost = false;

        var offset = new Vector3(0.5f, 0.5f, 0);
        foreach (var startPos in rooms.Keys)
        {
            leftMost = false;
            rightMost = false;
            downMost = false;
            upMost = false;
            set.Clear();
            foreach (var dir in Direction2D.dirList)
            {
                if (corridorPositions.Contains(startPos + dir))
                {
                    if (dir == left)
                        leftMost = true;
                    else if (dir == up)
                        upMost = true;
                    else if (dir == down)
                        downMost = true;
                    else rightMost = true;

                    foreach (var pos in doorPositions)
                    {
                        foreach (var direction in Direction2D.dirList)
                        {
                            if (rooms[startPos].Contains(pos + dir))
                            {
                                set.Add(pos);
                                break;
                            }
                        }
                    }
                }
            }
            if (leftMost == true)
            {
                var doorpos = findMostPos(set, left);
                actualDoorPos.Add(doorpos);
            }
            if (rightMost == true)
            {
                var doorpos = findMostPos(set, right);
                actualDoorPos.Add(doorpos);
            }
            if (upMost == true)
            {
                var doorPos = findMostPos(set, up);
                actualDoorPos.Add(doorPos);
            }
            if (downMost == true)
            {
                var doorPos = findMostPos(set, down);
                actualDoorPos.Add(doorPos);
            }


        }


        foreach (var pos in actualDoorPos)
        {

            Vector3 posV3 = new Vector3(pos.x, pos.y, 0);
            if (TileMaker.halfSet.Contains(pos + left - up) && TileMaker.halfSet.Contains(pos + right - up))
                Instantiate(bigDoorPrefab, posV3 + offset, transform.rotation, organizer.transform);
            else
                Instantiate(doorPrefab, posV3 + offset, Quaternion.Euler(0f, 0f, 90), organizer.transform);


        }

    }

    public static Vector2Int findMostPos(HashSet<Vector2Int> set, Vector2Int dir)
    {
        Vector2Int max = new Vector2Int(0, 0);
        if (dir == Vector2Int.left)
        {
            max = new Vector2Int(0, 5000);
            foreach (var pos in set)
            {
                if (pos.x < max.x)
                    max = pos;
            }
        }
        else if (dir == Vector2Int.right)
        {
            max = new Vector2Int(0, -5000);
            foreach (var pos in set)
            {
                if (pos.x > max.x)
                    max = pos;
            }
        }
        else if (dir == Vector2Int.up)
        {
            max = new Vector2Int(-5000, 0);
            foreach (var pos in set)
            {
                if (pos.y > max.y)
                    max = pos;

            }
        }
        else
        {
            max = new Vector2Int(5000, 0);
            foreach (var pos in set)
            {
                if (pos.y < max.y)
                    max = pos;
            }
        }

        return max;
    }



}
