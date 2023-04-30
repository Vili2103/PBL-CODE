using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShooter : MonoBehaviour
{
    [SerializeField]
    private Transform shootingPoint;
    [SerializeField]
    public GameObject bulletPrefab;
    [SerializeField]
    private  float firerate = 0.3f;

    private float tick;

    private void Start()
    {
        tick = firerate + 1;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && tick > firerate)
        {
            Shoot();
        }
    }
    private void FixedUpdate()
    {
        tick += Time.deltaTime;
    }
    private void Shoot()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 offset= new Vector3 (0, 0, -5f);
        var moveDir = (player.transform.position - transform.position);
       var bullet =  Instantiate(bulletPrefab, shootingPoint.position+offset, transform.rotation);
        var rb = bullet.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(moveDir.x, moveDir.y);
        tick = -2;
      
        }
    }

