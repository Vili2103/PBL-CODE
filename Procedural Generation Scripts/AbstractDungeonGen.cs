using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractDungeonGen : MonoBehaviour
{
    [SerializeField]
    protected TileMaker tileMaker = null; 
    [SerializeField]
    protected Vector2Int startPos = Vector2Int.zero;   

    public void GenerateDungeon()
    {
        ClearAll();
        RunPPG();
    
    }
    public void ClearAll()
    {
        tileMaker.ClearTiles();
        tileMaker.DeleteHitboxes();
        tileMaker.DeletePrefabs();
        CorridorFirstDungeonGenerator.roomPositions.Clear();
        CorridorFirstDungeonGenerator.floorPos.Clear();
        CorridorFirstDungeonGenerator.startRoomPos.Clear();
        CorridorFirstDungeonGenerator.corridorPositions.Clear();
        CorridorFirstDungeonGenerator.bossRoomPos.Clear();
        CorridorFirstDungeonGenerator.doorPositions.Clear();
        CorridorFirstDungeonGenerator.rotatedDoorPositions.Clear();
        CorridorFirstDungeonGenerator.rooms.Clear();
        PlaceEnemies.clearSet();
        //Earlier Iterations used to stack on top of each other

    }

    protected abstract void RunPPG();
}
