using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed;
    public float splash;
    public float damage;
    public int framesToFlash = 1;
    public Sprite flashSprite;

    private Respawn enemyHealth;
    private Rigidbody2D rb;



    void Start()
    {
        StartCoroutine(doFlash());
        //get rigid body
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.up * speed;
        Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GameObject.Find("Bob").GetComponent<Collider2D>());
        Physics2D.IgnoreLayerCollision(4, 4, true);
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.CompareTag("Obstacle")) {
            if(splash > 0) {

                var hitColliders = Physics2D.OverlapCircleAll(transform.position, splash);
                foreach( var hitCollider in hitColliders) {
                    var enemy = hitCollider.GetComponent<Respawn>();

                    if (enemy) {
                        var closestPoint = hitCollider.ClosestPoint(transform.position);
                        var distance = Vector3.Distance(closestPoint, transform.position);

                        var damagePercent = Mathf.InverseLerp(splash, 0, distance);
                        enemy.TakeDamage(damagePercent * damage);
                        
                        
                    }
                }
            }else {
                enemyHealth = collision.gameObject.GetComponent<Respawn>();
                enemyHealth.TakeDamage(damage);
            }
        }
        Destroy(gameObject);
        
    }

    IEnumerator doFlash() {
        var renderer =  GetComponent<SpriteRenderer>();
        var originalSprite = renderer.sprite;

        renderer.sprite = flashSprite;
        
        for(int i = 0; i< framesToFlash; i++) {
            yield return null;
        }

        renderer.sprite = originalSprite;

    }

    
}
