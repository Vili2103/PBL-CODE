using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class AbstractDungeonGen : MonoBehaviour
{   
    
  [SerializeField]
  protected TileMaker tileMaker = null;
    [SerializeField]
    protected Vector2Int startPos = Vector2Int.zero;

    public void GenerateDungeon()
    {
        tileMaker.ClearTiles();
        RunPPG();

    }
    protected abstract void RunPPG();

}
