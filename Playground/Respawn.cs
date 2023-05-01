using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    // Start is called before the first frame update

    public float Health = 100f;

   

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Die() {
        StartCoroutine(respawn(2f));
    }

    IEnumerator respawn(float duration){
        transform.localScale = new Vector3 (0, 0, 0);
        yield return new WaitForSecondsRealtime(duration);
        Health = 100;
        transform.localScale = new Vector3 (1, 1, 1);

    }

    public void TakeDamage(float damage) {
        Health-= damage;
        //Debug.Log(this.gameObject.name + " took " + damage);

        if(Health <= 0f) {
            
            Die();
        }
    }
}