using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WallMaker 
{
    public static void CreateWalls(HashSet<Vector2Int> floorPositions, TileMaker tileMaker)
    {
        var wallPos = FindWallsInDirections(floorPositions, Direction2D.dirList);
        var cornerPos = FindWallsInDirections(floorPositions, Direction2D.diagonalDirList);
        
        CreateBasicWall(tileMaker, wallPos, floorPositions);
        CreateCornerWalls(tileMaker, cornerPos, floorPositions);    

        // THIS WHOLE SCRIPT SERVES TO CALCULATE WHAT TILE WE ARE SUPPOSED TO HAVE AT OUR SPECIFIC LOCATION. 
    }

    private static void CreateCornerWalls(TileMaker tilemapVisualizer, HashSet<Vector2Int> cornerPos, HashSet<Vector2Int> floorPositions)
    {
        foreach (var pos in cornerPos)
        {
            string neighboursBinaryType =string.Empty;

            foreach (var dir in Direction2D.eightDirList)
            {
                var neighbourPos = pos + dir;
                if (floorPositions.Contains(neighbourPos))
                {
                    neighboursBinaryType += "1"; // THIS MEANS THAT WE ADD 1 TO THE BYTE VALUE. 1 MEANS THAT THERE IS A FLOOR IN THIS GIVEN DIRECTION. 
                }
                else
                {
                    neighboursBinaryType += "0"; // 0 -> FLOOR IN THE GIVEN DIRECTION
                }
            }
            tilemapVisualizer.PlaceSingleCornerWall(pos, neighboursBinaryType);
            // THE tileMaker  SCRIPT CHECKS THE BYTE VALUE OF neighboursType AND CALCULATES WHICH TILE TO PLACE. 
            // tileMaker CHECKS IF ONE OF THE LISTS IN Wall Bytes CONTAINS neighboursType, WHICH WE PASS ON HERE.
        }
    }

    private static void CreateBasicWall(TileMaker tilemapVisualizer, HashSet<Vector2Int> wallPos, HashSet<Vector2Int> floorPositions)
    {
        foreach (var position in wallPos)
        {
            string neighboursBinary = string.Empty;
            foreach (var direction in Direction2D.dirList)
            {
                var neighbourPosition = position + direction;
                if (floorPositions.Contains(neighbourPosition))
                {
                    neighboursBinary += "1";
                }
                else
                {
                    neighboursBinary += "0";
                }
            }
            tilemapVisualizer.PaintSingleBasicWall(position, neighboursBinary);
            // THE tileMaker  SCRIPT CHECKS THE BYTE VALUE OF neighboursType AND CALCULATES WHICH TILE TO PLACE. 
            // tileMaker CHECKS IF ONE OF THE LISTS IN Wall Bytes CONTAINS neighboursType, WHICH WE PASS ON HERE.
        }
    }

    private static HashSet<Vector2Int> FindWallsInDirections(HashSet<Vector2Int> floorPositions, List<Vector2Int> dirList)
    {
        HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
        foreach (var position in floorPositions)
        {
            foreach (var direction in dirList)
            {
                var neighboursPos = position + direction;
                if (floorPositions.Contains(neighboursPos) == false)
                    wallPositions.Add(neighboursPos);
            }
        }
        return wallPositions;
        // THIS METHOD CHECKS EVERY FLOOR POSITION AND CHECKS WHERE THERE ARE EMPTY SPACES. THEY ARE LATER FILLED IN BY WALLS.
    }
}
