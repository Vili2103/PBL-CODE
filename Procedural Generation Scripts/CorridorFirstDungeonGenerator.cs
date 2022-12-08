using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CorridorFirstDungeonGenerator : RandomWalkMapGen
{
    [SerializeField]
    private int length = 14, corridorCount = 5;
    [SerializeField]
    [Range(0.1f,1)] // Sets a range for the roomPercent variable. It cannot be bigger than one since it is a %
    private float roomPercent = 0.8f; //Chance to spawn a room where there is enough space to.
     
    protected override void RunPPG() // OVERRIDING AN ABSTRACT METHOD IS BASICALLY OVERLOADING BUT COOLER
    // SINCE WE DONT HAVE TO CHANGE THE PARAMETERS OR ANYTHING, BUT WE JUST CHANGE THE METHOD ALLTOGETHER.
    {
        CorridorFirstGeneration();
    }

    private void CorridorFirstGeneration()
    {
        HashSet<Vector2Int> floorPos = new HashSet<Vector2Int>();
        HashSet<Vector2Int> potentialRoomPos = new HashSet<Vector2Int>();
        makeCorridors(floorPos, potentialRoomPos);

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
            if(roomPositions.Contains(pos) == false)
            {
                var room = RunRandomWalk(randomWalkParams, pos);
                roomPositions.UnionWith(room);
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
            if (neighboursCount==1)
            {
                deadEnds.Add(pos);
            }
               
        }
        return deadEnds;
    }

    private HashSet<Vector2Int> CreateRooms(HashSet<Vector2Int> potentialRoomPos)
    {
        HashSet<Vector2Int> roomPositions = new HashSet<Vector2Int>();
        int roomCount = Mathf.RoundToInt(potentialRoomPos.Count * roomPercent); // WE MULTIPLY THE NUMBER OF POTENTIAL ROOMS BY THE ROOM SPAWN PERCENTAGE TO GET OUR ROOMCOUNT.
        List<Vector2Int> roomsToCreate = potentialRoomPos.OrderBy(x => Guid.NewGuid()).Take(roomCount).ToList(); //Gui = Globaly Unique Identifier x=> is a LAMBDA expression


        foreach (var roomPos in roomsToCreate)
        {
            var room = RunRandomWalk(randomWalkParams, roomPos);
            roomPositions.UnionWith(room); // BUILT IN HASHSET METHOD!!!! WOW!!!
        }
        return roomPositions;
    }

    private void makeCorridors(HashSet<Vector2Int> floorPositions, HashSet<Vector2Int> potentialRoomPos)
    {
        //RANDOM WALK CORRIDORS WITH THE STARTING POINT BEING startPos
        var pos = startPos;
        potentialRoomPos.Add(pos);

        for (int i = 0; i < corridorCount; i++)
        {
            var corridor = ProceduralGenerationAlgorithms.RandomWalkCorridor(pos, length);
            pos = corridor[corridor.Count - 1];
            potentialRoomPos.Add(pos);
            floorPositions.UnionWith(corridor); //HASHSET BUILT IN METHOD WOW!!!
        }
    }
}
