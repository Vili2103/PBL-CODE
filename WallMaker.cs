using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallMaker 
{
   public static void MakeWalls(HashSet<Vector2Int>floorPositions,TileMaker tileMaker)
    {
        var wallPos = FindWallsInDirections(floorPositions, RandomWalk.Direction2D.dirList);
        foreach(var position in wallPos)
        {
            tileMaker.PlaceSingleBasicWall(position);
        }

    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> dirList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach(var position in floorPositions)
        {
            foreach(var direction in dirList)
            {
               var neighborPos = position + direction;
                if (floorPositions.Contains(neighborPos) == false)
                {
                  wallPositions.Add(neighborPos);
                }
            }
        }
        return wallPositions;
    }
}
