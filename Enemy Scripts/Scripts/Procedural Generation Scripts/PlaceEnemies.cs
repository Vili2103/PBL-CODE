using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlaceEnemies : TileMaker
{
    public static HashSet<Vector2Int> enemyPositions = new HashSet<Vector2Int>();
    public static void placeEnemies(Tilemap floorTilemap,Tilemap wallTilemap,List<EnemyMaker>enemiesList,GameObject organizer)
    {
        foreach (var room in CorridorFirstDungeonGenerator.rooms)
        {
            HashSet<Vector2Int> roomTiles = room.Value;
            int enemiesInRoom = 0;
            int enemiesToHaveInRoom = UnityEngine.Random.Range(roomTiles.Count/55, roomTiles.Count/45); //Placeholder numbers, but we want it to scale with room size.
            int minX = int.MaxValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;
            int maxX = int.MinValue;
            foreach (var tile in roomTiles)
            {
                if (tile.x < minX)
                    minX = tile.x;

                if (tile.y < minY)
                    minY = tile.y;

                if (tile.x > maxX)
                    maxX = tile.x;

                if (tile.y > maxY)
                    maxY = tile.y;
            }

            while (enemiesInRoom < enemiesToHaveInRoom)
            {
                Vector2Int enemyPos = TileMaker.getRandomPos(minX, maxX, minY, maxY);
                if (CorridorFirstDungeonGenerator.bossRoomPos.Contains(enemyPos) || CorridorFirstDungeonGenerator.startRoomPos.Contains(enemyPos))
                    break;
                if (floorTilemap.HasTile((Vector3Int)enemyPos) && !CorridorFirstDungeonGenerator.corridorPositions.Contains(enemyPos) && !TileMaker.prefabPositions.Contains(enemyPos) && !enemyPositions.Contains(enemyPos)) // check if the position where the chest is being spawned is a floor tile
                //Its also very important that we check that the chest isn't a part of the corridor, so we don't block off rooms unintetionaly.
                {
                    PlaceEnemy(enemyPos,wallTilemap,getRandomEnemy(enemiesList),organizer);
                    enemiesInRoom++;
                    enemyPositions.Add(enemyPos);
                  
                }
            }
            enemiesInRoom = 0;
            //This is a bit clunky but recursion causes stackoverflow and this works sooo yeah.
        }
    }

    public static GameObject getRandomEnemy(List<EnemyMaker> enemiesList)
    {
        GameObject selectedEnemy = null;
        var totalSpawnRate = 0;
        foreach (var enemy in enemiesList)
        {
            totalSpawnRate += enemy.spawnRate;
        }
        var rng = UnityEngine.Random.Range(1, totalSpawnRate + 1);
        var processedWeight = 0;
        foreach (var enemy in enemiesList)
        {
            processedWeight += enemy.spawnRate;
            if (rng <= processedWeight)
            {
                selectedEnemy = enemy.enemyPrefab;
                break;
            }
        }
        return selectedEnemy;
    }

    private static void PlaceEnemy(Vector2Int enemyPos,Tilemap wallTilemap,GameObject enemyPrefab, GameObject organizer)
    {
        var offset = new Vector3(0.5f, 0.5f, -1); //For some reason I need this because it places it in the middle of the tiles, not where it should.
        var enemyPosV3 = wallTilemap.WorldToCell((Vector3Int)enemyPos);
        Instantiate(enemyPrefab, enemyPosV3 + offset, enemyPrefab.transform.rotation,organizer.transform);
    }
    public static void clearSet()
    {
        enemyPositions.Clear();
    }
}
