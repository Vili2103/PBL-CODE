using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstGen : RandomWalkMapGen
{
    [SerializeField]
    private int length, corridorCount = 5;
    [SerializeField]
    [Range(0.1f, 1)] // Sets a range for the roomPercent variable. It cannot be bigger than one since it is a %
    private float roomPercent=0.8f; //Chance to spawn a room where there is enough space to.    
   
    protected override void RunPPG()
    {
        corridorFirstGeneration();
    }

    private void corridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPos = new HashSet<Vector2Int>(); 
        makeCorridors(floorPos,potentialRoomPos);

        HashSet<Vector2Int> roomPos = CreateRooms(potentialRoomPos);
        

        List <Vector2Int> deadEnds = FindDeadEnds(floorPos);

        CreateRoomsAtDeadEnds(deadEnds, roomPos);
        floorPos.UnionWith(roomPos);

        tileMaker.PlaceFloorTiles(floorPos);
        WallMaker.MakeWalls(floorPos, tileMaker);
    }

    private void CreateRoomsAtDeadEnds(List<Vector2Int> deadEnds, HashSet<Vector2Int> roomPositions)
    {
       foreach(var pos in deadEnds)
        {
            if (roomPositions.Contains(pos) == false)
            {
                var room = RunRandomWalk(randomWalkParams, pos);
                roomPositions.UnionWith(room);
            }
        }
    }

    private List<Vector2Int> FindDeadEnds(HashSet<Vector2Int> floorPos)
    {
        List<Vector2Int> deadEnds = new List<Vector2Int>();
        foreach(var pos in floorPos)
        {
            int neighbourCount = 0;
            foreach(var dir in RandomWalk.Direction2D.dirList)
            {
                if (floorPos.Contains(pos + dir))
                {
                    neighbourCount++;
                
                }
            }
            if (neighbourCount==1)
            {
                deadEnds.Add(pos);
            }
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPos)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomCount = Mathf.RoundToInt(potentialRoomPos.Count * roomPercent);
        List<Vector2Int> roomsToMake = potentialRoomPos.OrderBy(x => Guid.NewGuid()).Take(roomCount).ToList();  //Gui = Globaly Unique Identifier x=> is a LAMBDA expression

        foreach(var roomPos in roomsToMake)
        {
            var room = RunRandomWalk(randomWalkParams,roomPos);
            roomPositions.UnionWith(room);
        }
        return roomPositions;
                

    }

    private void makeCorridors(HashSet<Vector2Int> floorPos, HashSet<Vector2Int> potentialRoomPos)
    {
        var pos = startPos;
        potentialRoomPos.Add(pos);

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = RandomWalk.RandomWalkCorridor(pos, length);
            pos = corridor[corridor.Count - 1];
            potentialRoomPos.Add(pos);
            floorPos.UnionWith(corridor); //HASHSET BUILT IN METHOD WOW!!!


        }
    }
}
