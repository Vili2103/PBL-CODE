using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.InputSystem;
public class shoot : MonoBehaviour
{
    public Transform shootingPoint;
    public GameObject bulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0) || Keyboard.current.spaceKey.wasPressedThisFrame) {
            Instantiate(bulletPrefab,shootingPoint.position, transform.rotation);
        }
    }
}
