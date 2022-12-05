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
    // THIS WHOLE SCRIPT SERVES TO CALCULATE WHAT TILE WE ARE SUPPOSED TO HAVE AT OUR SPECIFIC LOCATION. 
    private static void CreateCornerWalls(TileMaker tileMaker, HashSet<Vector2Int> cornerPos, HashSet<Vector2Int> floorPositions)
    {
        // 
        foreach (var pos in cornerPos)
        {
            string neighboursType = string.Empty;
            foreach (var dir in RandomWalk.Direction2D.eigthDirList)
            {
                var neighbourPos = pos + dir;
                if (floorPositions.Contains(neighbourPos))
                {
                    neighboursType += "1"; // THIS MEANS THAT WE ADD 1 TO THE BYTE VALUE. 1 MEANS THAT THERE IS A FLOOR IN THIS GIVEN DIRECTION. 
                }
                else
                {
                    neighboursType += "0"; // 0 -> FLOOR IN THE GIVEN DIRECTION
                }
            }
            tileMaker.PlaceSingleCornerWall(pos, neighboursType); 
            // THE tileMaker  SCRIPT CHECKS THE BYTE VALUE OF neighboursType AND CALCULATES WHICH TILE TO PLACE. 
            // tileMaker CHECKS IF ONE OF THE LISTS IN Wall Bytes CONTAINS neighboursType, WHICH WE PASS ON HERE.
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
            // THE tileMaker  SCRIPT CHECKS THE BYTE VALUE OF neighboursType AND CALCULATES WHICH TILE TO PLACE. 
            // tileMaker CHECKS IF ONE OF THE LISTS IN Wall Bytes CONTAINS neighboursType, WHICH WE PASS ON HERE.
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
        // THIS METHOD CHECKS EVERY FLOOR POSITION AND CHECKS WHERE THERE ARE EMPTY SPACES. THEY ARE LATER FILLED IN BY WALLS.
    }
}
