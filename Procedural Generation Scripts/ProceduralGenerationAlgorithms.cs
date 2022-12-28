using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class ProceduralGenerationAlgorithms
{
    public static HashSet<Vector2Int> SimpleRandomWalk(Vector2Int startPos, int stepsToTake)
    {
        /* We use a HashSet mainly to guarantee that all values are unique and that our agent does not “step” on the same tile twice. It also has the operations “UnionWith”, 
        * “ExceptWith”, and “IntersectWith”. The argument startPos is the coordinates of our starting position and stepsToTake helps us define the scope of the algorithm and will be used in a for loop*/
        /* Vector2Int is a Representation of 2D vectors and points using integers that is provided by Unity. We also use it for our character movement. */


        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startPos);
        var previousPos = startPos;

        // We just create a new HashSet called path, where we store the tiles where the agent has stepped

        for (int i = 0; i < stepsToTake; i++)
        {
            var newPos = previousPos + Direction2D.RandomDir();
            path.Add(newPos);
            previousPos = newPos;
        }
        return path;
    }

    public static List<Vector2Int> RandomWalkCorridor(Vector2Int startPos, int stepsToTake)
    {
        List<Vector2Int> corridors = new List<Vector2Int>();
        var dir = Direction2D.RandomDir();
        var pos = startPos;
        corridors.Add(pos);
        for (int i = 0; i < stepsToTake; i++)
        {
            pos += dir;
            corridors.Add(pos);
        }
        return corridors;
    }

    public static List<BoundsInt> BSP(BoundsInt spaceToSplit, int minWidth, int minHeight)
    {
        // A boundsInt represents an axis aligned bounding box with all values as ints

        Queue<BoundsInt> roomQueue = new Queue<BoundsInt>(); // A queue is a collection of objects, in which the first one to enter it is the first one to exit
        List<BoundsInt> roomList = new List<BoundsInt>();

        roomQueue.Enqueue(spaceToSplit); //Enqueue adds an object to the queue
        while (roomQueue.Count > 0)
        {
            var room = roomQueue.Dequeue(); // Removes an object from the queue
            int width = room.size.x;
            int height = room.size.y;


            if (height >= minHeight && width >= minWidth)
            {
                if(UnityEngine.Random.value < 0.5f)
                {
                    if(height >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomQueue, room);

                    }
                    else if(width >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomQueue, room);

                    }else if(width >= minWidth && height >= minHeight)
                    {
                        roomList.Add(room);
                    }
                }
                else
                {
                    if (width >= minWidth * 2)
                    {
                        SplitVertically(minWidth, roomQueue, room);
                    }
                    else if (height >= minHeight * 2)
                    {
                        SplitHorizontally(minHeight, roomQueue, room);
                    }
                    else 
                    {
                        roomList.Add(room);
                    }
                }
            }
        }
        return roomList;
    }

    private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        int width = room.size.x;
        int height = room.size.y;

        var vertSplit = UnityEngine.Random.Range(1, width);

        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(vertSplit, height, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(width + vertSplit, height, room.min.z), new Vector3Int(width - vertSplit, height, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private static void SplitHorizontally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room)
    {
        int width = room.size.x;
        int height = room.size.y;

        var horSplit = UnityEngine.Random.Range(1, height);

        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(width, horSplit, room.size.z));
        BoundsInt room2 = new BoundsInt(new Vector3Int(width, height + horSplit, room.min.z),new Vector3Int(width, height - horSplit, room.size.z));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
}

public static class Direction2D
{
    public static List<Vector2Int> dirList = new List<Vector2Int> // List of Vector2IntValues -> Right, Left, Up, Down | x comes first, y second 
    {
        new Vector2Int(0,1), //UP X =0 Y = 1
        new Vector2Int(1,0), //RIGHT X =1 Y =0
        new Vector2Int(0, -1), // DOWN X =0 Y=-1
        new Vector2Int(-1, 0) //LEFT X=-1 Y=0
    };

    public static List<Vector2Int> diagonalDirList = new List<Vector2Int>
    {
        new Vector2Int(1,1), //UP-RIGHT
        new Vector2Int(1,-1), //RIGHT-DOWN
        new Vector2Int(-1, -1), // DOWN-LEFT
        new Vector2Int(-1, 1) //LEFT-UP
    };

    public static List<Vector2Int> eightDirList = new List<Vector2Int>
    {
        new Vector2Int(0,1), //UP
        new Vector2Int(1,1), //UP-RIGHT
        new Vector2Int(1,0), //RIGHT
        new Vector2Int(1,-1), //RIGHT-DOWN
        new Vector2Int(0, -1), // DOWN
        new Vector2Int(-1, -1), // DOWN-LEFT
        new Vector2Int(-1, 0), //LEFT
        new Vector2Int(-1, 1) //LEFT-UP

    };

    public static Vector2Int RandomDir()
    {
        return dirList[UnityEngine.Random.Range(0, dirList.Count)];
        // returns a random Vector2 value (UP, DOWN, LEFT, RIGHT) with a random index from 0 to 4
    }
}