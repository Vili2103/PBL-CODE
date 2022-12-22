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
    }

    protected abstract void RunPPG();
}
