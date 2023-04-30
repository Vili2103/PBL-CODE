using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class FlagMaker : ScriptableObject
{

    public int spawnRate;

    public Tile wallTile;

    public string tileName;

    public FlagMaker(Tile wallTile, int spawnRate)//Object wooow! 
    {
        this.wallTile = wallTile;
        this.spawnRate = spawnRate;
    }

}


