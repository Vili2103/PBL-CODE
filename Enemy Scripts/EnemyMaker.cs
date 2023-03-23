using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyMaker : ScriptableObject
{

    public int spawnRate;

    public GameObject enemyPrefab;

    public string enemyName;

    public EnemyMaker(GameObject enemyPrefab, int spawnRate)//Object wooow! 
    {
        this.enemyPrefab = enemyPrefab;
        this.spawnRate = spawnRate;
    }

}
