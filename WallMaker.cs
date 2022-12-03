using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallMaker 
{
   public static void MakeWalls(HashSet<Vector2Int>floorPositions,TileMaker tileMaker)
    {
        var wallPos = FindWallsInDirections(floorPositions, RandomWalk.Direction2D.dirList);
        var cornerPos = FindWallsInDirections(floorPositions, RandomWalk.Direction2D.diagonalDirList);
        CreateBasicWalls(tileMaker, wallPos,floorPositions);
        CreateCornerWalls(tileMaker, cornerPos, floorPositions);

    }

    private static void CreateCornerWalls(TileMaker tileMaker, HashSet<Vector2Int> cornerPos, HashSet<Vector2Int> floorPositions)
    {
        foreach (var pos in cornerPos)
        {
            string neighboursType = string.Empty;
            foreach (var dir in RandomWalk.Direction2D.eigthDirList)
            {
                var neighbourPos = pos + dir;
                if (floorPositions.Contains(neighbourPos))
                {
                    neighboursType += "1";
                }
                else
                {
                    neighboursType += "0";
                }
            }
            tileMaker.PlaceSingleCornerWall(pos, neighboursType);
        }
    }

    private static void CreateBasicWalls(TileMaker tileMaker, HashSet<Vector2Int> wallPos, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in wallPos)
        {
            string neighboursBinary = string.Empty;
            foreach (var dir in RandomWalk.Direction2D.dirList)
            {
                var neighbourPos = position + dir;
                if (floorPositions.Contains(neighbourPos))
                {
                    neighboursBinary += "1";
                }
                else
                {
                    neighboursBinary += "0";
                }
            }
            tileMaker.PlaceSingleBasicWall(position,neighboursBinary);
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
