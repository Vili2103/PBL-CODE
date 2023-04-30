using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyMaker : ScriptableObject
{

    public int spawnRate;

    public GameObject enemyPrefab;

    public string enemyName;

    public int maxHP;

    public EnemyMaker(GameObject enemyPrefab, int spawnRate, int maxHP)//Object wooow! 
    {
        this.enemyPrefab = enemyPrefab;
        this.spawnRate = spawnRate;
        this.maxHP = maxHP;
    }
   

}
