// object's sprite must have pivot point set to bottom in order to work

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    public float RotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        
        //Debug.Log("Hit!");
    }
    
    public Vector2 MousePos;
    public Vector2 WorldPos;

    // Update is called once per frame
    void Update()
    {
        //gets world position of object
        WorldPos = Camera.main.WorldToViewportPoint (transform.position);

        //gets world position of mouse coordinates
        MousePos = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //we studied this two weeeks ago
        float angle = AngleBetweenTwoPoints(WorldPos, MousePos);
        
        // rotates z degrees around the z axis      
        transform.rotation =  Quaternion.Euler(new Vector3(0f,0f,angle));

    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b) {
         return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
     }



}
