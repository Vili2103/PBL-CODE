using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Shooting : MonoBehaviour
{
    [SerializeField]
    private Transform gunPoint;
    [SerializeField]
    private WeaponMaker weapon;
  

    
    private float shootTimer = 0f;
   
    void Update()
    {
       
        if (Input.GetButtonDown("Fire1") && Time.time > shootTimer)
        {
            shootTimer = Time.time + weapon.shotRate;
            Shoot();
            CameraShake.Instance.Shake(10f, 0.2f);
        }

    }
    public void Shoot()
    {
        var offset = new Vector3(0, 0, -5);
        var bullet = Instantiate(weapon.bulletPrefab, gunPoint.position+offset, gunPoint.rotation);
        var rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(gunPoint.up * weapon.bulletSpeed, ForceMode2D.Impulse);
    }
}
