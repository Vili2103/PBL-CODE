// object's sprite must have pivot point set to bottom in order to work

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAim : MonoBehaviour
{
   
    // Start is called before the first frame update
    void Start()
    {
        
        //Debug.Log("Hit!");
    }
    

    // Update is called once per frame
    void Update()
    {
       var dir = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);
       var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
       transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);


    }



}
