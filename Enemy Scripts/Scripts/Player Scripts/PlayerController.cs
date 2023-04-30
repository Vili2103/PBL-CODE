using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb; // Rigidbody in order to move
    public Animator anim; //Animation Controller

    float horizontal;
    float vertical;
    public float diagonalMoveLimiter = 1f;
    public float moveSpeed = 8.0f;
    public int maxHP = 100;
    public int currentHP;
    bool walkRight=false;
    bool walkLeft=false;
    bool walkDown=false;
    bool walkUp=false;

    private void Start()
    {
        currentHP = maxHP;
        Debug.Log("starting with " + currentHP);
    }


    void Update()
    {
       
        horizontal = Input.GetAxisRaw("Horizontal");  // Check input if player is trying to move horizontally
        vertical = Input.GetAxisRaw("Vertical");     // Check input if player is trying to move vertically

        if (walkDown ==true && walkLeft==true)
        {
            //DIAGONALKA
        }
        if (walkDown == true && walkRight == true)
        {
            //DIAGONALKA
        }
        if (walkUp == true && walkLeft == true)
        {
            //DIAGONALKA
        }
        if (walkUp == true && walkRight == true)
        {
            //DIAGONALKA
        }
        if (currentHP <= 0)
            Debug.Log("umreh");

    }
   public  void TakeDMG(int dmg)
    {
        currentHP -= dmg;
        if (currentHP <= 0)
        {
            Debug.Log("BOOOM"); 
        }
    }

    void FixedUpdate() // We put it in FixedUpdate so it doesnt run more times based on player's FPS. I think it will run 60/s
    {
        if (horizontal != 0 && vertical != 0) // Diagonal movement
        {
            // Diagonal Speed Slows So You Dont Go Supersonic With it

            horizontal *= diagonalMoveLimiter;
            vertical *= diagonalMoveLimiter; // Make Diagonal Movement Slower So Players Don't Exploit
        }
        //Movement Line
        rb.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);

        //ANIMATION CHECKS

        if (horizontal >= 0.1) {
            walkRight = true;
        }
        else
        {
            walkRight = false;
        }

        if (horizontal <= 0.1)
        {
            walkLeft = true;
        }
        else
        {
            walkLeft = false;
        }

        if (vertical >= 0.1)
        {
            walkUp = true;
        }
        else
        {
            walkUp = false;
        }

        if (vertical <= 0.1)
        {
            walkDown = true;
        }
        else
        {
            walkDown = false;
        }


    }
}
