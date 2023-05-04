using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEquip : MonoBehaviour
{
    // Start is called before the first frame update

    private GunAim gunScript;
    private shoot shootScript;
    private BoxCollider2D box;
    private SpriteRenderer sprite;
    private BobMove PlayerScript;

    private float speedNoWeapon;

    private string weaponName;
    private string secondaryName;
    private string tempName;
    private GameObject weapon;
    private GameObject secondary;
    private GameObject temp;
    
    

    
    void Start()
    {
        PlayerScript = GameObject.Find("Bob").GetComponent<BobMove>();
        speedNoWeapon = PlayerScript.Speed;

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && !string.IsNullOrEmpty(weaponName)) {

            dropWeapon();

            makePrimary(secondaryName, secondary, true);
                
            
        }

        if(Input.GetKeyDown(KeyCode.Q)) {
            if(string.IsNullOrEmpty(weaponName) && string.IsNullOrEmpty(secondaryName)) {
                //Do nothing, chill
                //Debug.Log("No weapons");

            } else if (string.IsNullOrEmpty(weaponName)) {
                makePrimary(secondaryName, secondary, true);
                
                //Debug.Log("Only secondary");

            } else if (string.IsNullOrEmpty(secondaryName)) {
                makeSecondary();
                //Debug.Log("Only primary");
                
            } else {
                //Debug.Log("Two weapons");                

                temp = secondary;
                tempName = secondaryName;
                makeSecondary();
                makePrimary(tempName, temp, false);
            }
            
        }
    }

    private void OnTriggerStay2D(Collider2D collision) {

        if (collision.gameObject.CompareTag("Gun") && (string.IsNullOrEmpty(secondaryName) || string.IsNullOrEmpty(weaponName)))
        {
                makeSecondary();  

           activateWeapon(collision.gameObject.name, collision.gameObject);

        }

    }

    void activateWeapon(string weaponName, GameObject weaponObject) {

        sprite = GameObject.Find(weaponName).GetComponent<SpriteRenderer>();
        gunScript = GameObject.Find(weaponName).GetComponent<GunAim>();
        shootScript = GameObject.Find(weaponName).GetComponent<shoot>();
        box = GameObject.Find(weaponName).GetComponent<BoxCollider2D>();
        this.weaponName = weaponName;
        weapon = weaponObject;
        weaponObject.transform.parent = this.gameObject.transform;
        PlayerScript.Speed = shootScript.playerSpeed;
        
        GameObject.Find(weaponName).transform.position = GameObject.Find("GunHold").transform.position;
        sprite.sortingOrder = 7;

        box.enabled = false;
        gunScript.enabled = true;
        shootScript.enabled = true;

    }

    void deactivateWeapon() {
        sprite.sortingOrder = 1;
        gunScript.enabled = false;
        shootScript.enabled = false;
        
        PlayerScript.Speed = speedNoWeapon;
        
        
    }

    IEnumerator MakeReadyForPickUp(BoxCollider2D currentBox ,float duration){
        yield return new WaitForSecondsRealtime(duration);
        currentBox.enabled = true;

    }

    void dropWeapon() {

        deactivateWeapon();
        weaponName = string.Empty;
        weapon.transform.parent = null;
        sprite.sortingOrder = 0;
        StartCoroutine(MakeReadyForPickUp(box, 2f));
    }

    void makePrimary(string weaponName, GameObject weaponObject, bool clearSecName) {
        if(!string.IsNullOrEmpty(secondaryName)) {
            activateWeapon(weaponName, weaponObject);
            if (clearSecName) {
                secondaryName = string.Empty;   
            }

            //Debug.Log("Make " + weaponName + " primary. Secondary is " + secondaryName);
        }
            
            
        
    }
    void makeSecondary() {
        if(!string.IsNullOrEmpty(weaponName)) {
            deactivateWeapon();
            weapon.transform.position = GameObject.Find("SecondaryHold").transform.position;
            weapon.transform.rotation = GameObject.Find("SecondaryHold").transform.rotation;
            secondaryName = weaponName;
            secondary = weapon;

            //Debug.Log("Make " + secondaryName + " secondary. Primary is " + weaponName);
            weaponName = string.Empty;
        }
        
    }

    
}
