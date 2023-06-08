using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
public class shoot : MonoBehaviour
{

    
    public Transform[] shootingPointArr;
    public GameObject bulletPrefab;
    public Sprite flashSprite;
    public float firerate = 0.3f;
    public float playerSpeed = 5;
    public float shotgunPallets = 0f;
    public float PalletWidth = 90;
    public float damage = 50;
    public float splashRadius = 0;
    public float bulletSpeed = 12;
    public int framesToFlash = 5; 
    public float shakeIntensity = 5f;
    public float shakeTime = .1f;

    private float tick;
    private BobMove PlayerScript;
    private bullet BulletScript;

    // Start is called before the first frame update
    void Start()
    {
        tick = firerate + 1;
        PlayerScript = GameObject.Find("Bob").GetComponent<BobMove>();
        PlayerScript.Speed = playerSpeed;

        BulletScript = bulletPrefab.GetComponent<bullet>();
        BulletScript.damage = this.damage;
        BulletScript.splash = this.splashRadius;
        BulletScript.speed = this.bulletSpeed;
        BulletScript.framesToFlash = this.framesToFlash;
        BulletScript.flashSprite = this.flashSprite;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) && tick>firerate) {
            camGoBrrr.Instance.shakeCamera(shakeIntensity, shakeTime);
            foreach(var muzzle in shootingPointArr) {
                Bang(muzzle);
            }
            tick = 0;
        }
        tick = tick + Time.deltaTime;
    }

    void Bang(Transform PowPoint) {
        if(shotgunPallets == 0) {
                Instantiate(bulletPrefab, PowPoint.position, PowPoint.transform.rotation);
                
            } else {
                float PalletAngle = PalletWidth/(shotgunPallets-1);
                PowPoint.transform.Rotate(0, 0, PalletWidth/2);
                for(int i = 0; i < shotgunPallets; i++) {
                    Instantiate(bulletPrefab, PowPoint.position, PowPoint.transform.rotation);
                    PowPoint.transform.Rotate (0,0,-PalletAngle);

                }
                PowPoint.transform.Rotate(0, 0, PalletWidth/2 + PalletAngle);
            }
    }
}
