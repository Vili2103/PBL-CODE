using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{

    public float speed;
    private Rigidbody2D rb;

    void Start()
    {
        //get rigid body
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;

        /*
        RaycastHit2D raycastHit2D = Physics2D.Raycast(transform.position, Vector3.up);
        if(raycastHit2D) {
            Debug.Log("Hit!");
        }
        */
    }

    
}
