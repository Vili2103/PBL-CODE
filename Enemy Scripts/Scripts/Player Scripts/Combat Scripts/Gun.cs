using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private Vector2 MousePos;
    private Vector2 WorldPos;

    void Update()
    {
        MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void FixedUpdate()
    {
        //gets world position of object
        WorldPos = Camera.main.WorldToViewportPoint(transform.position);

        //gets world position of mouse coordinates
        MousePos = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //we studied this two weeeks ago
        float angle = AngleBetweenTwoPoints(WorldPos, MousePos);

        // rotates z degrees around the z axis      
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }
    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg+135f;
    }
}
