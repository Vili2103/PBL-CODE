using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class FloorMaker : ScriptableObject
{

    public int spawnRate;

    public Tile floorTile;

    public string tileName;

    public FloorMaker(Tile floorTile, int spawnRate)//Object wooow! 
    {
        this.floorTile = floorTile;
        this.spawnRate = spawnRate;
    }

}


