using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobMove : MonoBehaviour
{
   public float Speed = 3f;
    // Start is called before the first frame update
   void Start()
   { 
    
   }
   // Update is called once per frame
   void Update()
   {
      float horizontal = Input.GetAxis("Horizontal");
      float vertical = Input.GetAxis("Vertical");
      
      Vector2 position = transform.position;
      position.x = position.x + Speed * horizontal * Time.deltaTime;
      position.y = position.y + Speed * vertical * Time.deltaTime;
      transform.position = position;
   }
}
