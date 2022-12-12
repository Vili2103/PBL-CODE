using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
public class shoot : MonoBehaviour
{
     [SerializeField]
    protected WeaponParameters weaponParams;
    
    public Transform shootingPoint;
    weaponParams.coolDown*=50;
    private int recharge = 0;
   
   
    void FixedUpdate() // FixedUpdate updates 50 times a second.
    {
           weaponParams.coolDown++; // this may be dumb and very inefficient but it's the only thing i could think of. 
        if(Input.GetInput("Fire1") && recharge => weaponParams.coolDown){
            Instantiate(weaponParams.bulletPrefab,shootingPoint.position, transform.rotation);
        }
    }
}
