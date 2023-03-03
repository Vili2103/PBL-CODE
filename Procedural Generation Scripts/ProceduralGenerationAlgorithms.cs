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
