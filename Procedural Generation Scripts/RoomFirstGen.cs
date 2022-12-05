using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomFirstGen : RandomWalkMapGen
{
    [SerializeField]
    private int minRoomWidth = 8, minRoomHeight = 8;
    [SerializeField]
    private int dungeonWidth = 100, dungeonHeight = 100;
    [SerializeField]
    [Range(1, 10)]
    private int offset = 5; // OFFSET EXSISTS SO OUR ROOMS DON'T OVERLAP. IT'S THE MIN DISTANCE BETWEEN TWO ROOMS
    [SerializeField]
    private bool randomWalkRooms = false;

    protected override void RunPPG()
    {
        // SINCE RUNPPG IS AN ABSTRACT METHOD WE CAN JUST OVERRIDE IT. THIS IS LIKE OVERLOADING BUT COOLER BECAUSE WE CHANGE THE
        // WHOLE METHOD AND DON'T NEED TO RECEIVE MORE OR LESS PARAMETERS
        CreateRooms();
    }

    private void CreateRooms()
    {
        var roomList = RandomWalk.BSP(new BoundsInt((Vector3Int)startPos, new Vector3Int(dungeonWidth, dungeonHeight, 0)),minRoomWidth,minRoomHeight);
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();

        if (randomWalkRooms)
        {
            floor = CreateRandomRooms(roomList); //ROOMS ARE MADE USING THE RANDOMWALK ALGORITHM
        }
        else
        {
            floor = CreateSimpleRooms(roomList); // ROOMS ARE MADE USING THE BINARY SPACE PARTITIONING ALGORITHM
        }
        
        List<Vector2Int> roomCenters = new List<Vector2Int>();
        foreach(var room in roomList)
        {
            roomCenters.Add((Vector2Int)Vector3Int.RoundToInt(room.center)); // WE USE THIS TO MAKE CORRIDORS TO EACH ROOM CENTERR, TO ENSURE THAT EVERY ROOM IS CONNECTED TO ANOTHER.

        }
        HashSet<Vector2Int> corridors = ConnectRooms(roomCenters);
        floor.UnionWith(corridors); //UnionWith IS A HASHSET BUILT IN METHOD. HERE WE USE IT TO MAKE THE CORRIDOR TILES A PART OF THE FLOOR POSITIONS HASHSET

        tileMaker.PlaceFloorTiles(floor); // WE PLACE THE FLOOR TILES, INCLUDING THE CORRIDORS.
        WallMaker.MakeWalls(floor, tileMaker);  //WE ADD WALLS
    }

    private HashSet<Vector2Int> CreateRandomRooms(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        for (int i = 0; i < roomList.Count; i++)
        {
            var roomBounds = roomList[i];
            var roomCenter = new Vector2Int(Mathf.RoundToInt(roomBounds.center.x), Mathf.RoundToInt(roomBounds.center.y));
            var roomFloor = RunRandomWalk(randomWalkParams,roomCenter);
            foreach (var pos in roomFloor)
            {
                if (pos.x >= (roomBounds.xMin + offset) && pos.x <= (roomBounds.xMax - offset) && pos.y >= (roomBounds.yMin - offset) && pos.y <= (roomBounds.yMax - offset))
                {
                    floor.Add(pos);
                }
            }
        }
        return floor;
    }

    private HashSet<Vector2Int> ConnectRooms(List<Vector2Int> roomCenters)
    {
        //THIS METHOD CONNECTS ROOMS BY FINDING THE CLOSEST ROOMCENTERS AND CONNECTING THEM WITH CORRIDORS
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var currentRoomCenter = roomCenters[UnityEngine.Random.Range(0, roomCenters.Count)];
        roomCenters.Remove(currentRoomCenter);

        while (roomCenters.Count > 0)
        {
            Vector2Int closestCenter = FindClosestPointTo(currentRoomCenter, roomCenters);
            roomCenters.Remove(closestCenter);
            HashSet<Vector2Int> corridor = CreateCorridor(currentRoomCenter, closestCenter);
            currentRoomCenter = closestCenter;
            corridors.UnionWith(corridor);
        }
        return corridors;
    }

    private HashSet<Vector2Int> CreateCorridor(Vector2Int currentRoomCenter, Vector2Int closestCenter)
    {
        HashSet<Vector2Int> corridors = new HashSet<Vector2Int>();
        var position = currentRoomCenter;
        corridors.Add(position);
        while (position.y != closestCenter.y)
        {
            if (closestCenter.y > position.y)
            {
                position += Vector2Int.up;
            }else if(closestCenter.y < position.y)
            {
                position += Vector2Int.down;
            }
            corridors.Add(position);
        }
        while (position.x != closestCenter.x)
        {
            if (closestCenter.x > position.x)
            {
                position += Vector2Int.right;
            }else if (closestCenter.x < position.x)
            {
                position += Vector2Int.left;

            }
            corridors.Add(position);
        }
        return corridors;
    }

    private Vector2Int FindClosestPointTo(Vector2Int currentRoomCenter, List<Vector2Int> roomCenters)
    {
        Vector2Int closestCenter = Vector2Int.zero;
        float length = float.MaxValue; // JUST SO THE FIRST IF RUNS 100% OF THE TIME 
        foreach(var pos in roomCenters)
        {
            float distance = Vector2.Distance(pos, currentRoomCenter);
            if (distance < length)
            {
                length = distance;
                closestCenter = pos;
            }
        }
        return closestCenter;
    }

    private HashSet<Vector2Int> CreateSimpleRooms(List<BoundsInt> roomList)
    {
        HashSet<Vector2Int> floor = new HashSet<Vector2Int>();
        foreach(var room in roomList)
        {
            for (int column = offset; column < room.size.x-offset; column++)
            {
                for (int row = offset; row < room.size.y-offset; row++)
                {
                    Vector2Int pos = (Vector2Int)room.min + new Vector2Int(column, row);
                    floor.Add(pos);
                }
            }
        }
        return floor;
    }

}
