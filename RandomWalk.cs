using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RandomWalk{
    public static HashSet<Vector2Int> RandomWalkAlg(Vector2Int startPos, int stepsToTake)
    {

        /* We use a HashSet mainly to guarantee that all values are unique and that our agent does not “step” on the same tile twice. It also has the operations “UnionWith”, 
        * “ExceptWith”, and “IntersectWith”. The argument startPos is the coordinates of our starting position and stepsToTake helps us define the scope of the algorithm and will be used in a for loop*/
        /* Vector2Int is a Representation of 2D vectors and points using integers that is provided by Unity. We also use it for our character movement. */

        HashSet<Vector2Int> path = new HashSet<Vector2Int>();
        path.Add(startPos);
        // We just create a new HashSet called path, where we store the tiles where the agent has stepped

        var previousPos = startPos;

        for(int i=0; i<stepsToTake; i++)
        {
            var newPos = previousPos + Direction2D.RandomDir();
            path.Add(newPos); // Add the tile that we stepped on to the path
            previousPos = newPos;
        }
        return path;
    }
    public static class Direction2D
    {
        public static List<Vector2Int> dirList = new List<Vector2Int>{ // List of Vector2IntValues -> Right, Left, Up, Down | x comes first, y second 
        new Vector2Int(0,1), new Vector2Int(0,-1), new Vector2Int(1,0), new Vector2Int(-1,0)};
        public static Vector2Int RandomDir()
        {

            return dirList[Random.Range(0, dirList.Count)];
            // returns a random Vector2 value (UP, DOWN, LEFT, RIGHT) from with index from 0 to 4 (the number of directions)
        }

    }





    

}
