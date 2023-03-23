using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelleeAttacker : MonoBehaviour
{
	[SerializeField]
	private int damage = 10;
    public float attackRate = 5f;
    float nextAttackTime = 0f;
	private GameObject player;

    private void Start()
    {
		player = GameObject.FindGameObjectWithTag("Player");
    }

   
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Time.time >= nextAttackTime)
        {
            melleeAttack();
            nextAttackTime = Time.time + 1f / attackRate;
        }
            
    }

    private void melleeAttack()
    {
		player.GetComponent<PlayerController>().TakeDMG(damage);
    }
}
