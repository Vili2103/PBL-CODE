using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage = 60;
    private void OnCollisionEnter2D(Collision2D collision)
    {

        var enemy = collision.gameObject;
        if(enemy.tag == "Enemy")
        enemy.GetComponentInChildren<MelleeAttacker>().TakeDMG(damage);
        DestroyImmediate(gameObject);
    }
}
