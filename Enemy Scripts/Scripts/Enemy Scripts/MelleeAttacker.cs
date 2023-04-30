using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MelleeAttacker : MonoBehaviour
{
	[SerializeField]
	private int damage = 5;
    private float firerate = 0.8f;

    private float tick;

    private Animator anim;
    private GameObject player;
    [SerializeField]
    private int maxHP = 60;
    int currentHP;
    
    private void Start()
    {
		player = GameObject.FindGameObjectWithTag("Player");
        currentHP = maxHP;
        tick = firerate + 1;
        anim = gameObject.GetComponentInParent<Animator>();
    }

    private void FixedUpdate()
    {
        tick += Time.deltaTime;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (tick>firerate && collision.gameObject.tag == "Player")
        {
            melleeAttack();
        }
            
    }

    private void melleeAttack()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        var pc = player.GetComponent<PlayerController>();
        pc.currentHP -= damage;
      //  player.GetComponent<PlayerController>().TakeDMG(damage);
        tick = -3;
    }
    public void TakeDMG(int damage)
    {
       
        Debug.Log("UDAREN");
        currentHP -= damage;
        anim.SetTrigger("TakeDMG");
        if (currentHP <= 0)
        {
            anim.SetTrigger("Die");
            DestroyImmediate(gameObject);
            
        }
    }
}
