using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimator : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var rb = gameObject.GetComponent<Rigidbody2D>();
        if (rb.velocity != Vector2.zero)  
            anim.SetBool("Moving", true);
        else anim.SetBool("Moving", false);
    }
}
