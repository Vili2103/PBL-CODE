using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
public class shoot : MonoBehaviour
{
    public Transform shootingPoint;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField] // Makes this visible in the inspector, even though it's private
    private float coolDown = 0.0f;
    coolDown*=50;
    private int recharge = 0;
   
   
    void FixedUpdate() // FixedUpdate updates 50 times a second.
    {
           coolDown++; // this may be dumb and very inefficient but it's the only thing i could think of. 
        if(Input.GetInput("Fire1") && recharge => coolDown){
            Instantiate(bulletPrefab,shootingPoint.position, transform.rotation);
        }
    }
}
