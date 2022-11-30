using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] // So we can acess it from the inspector even though it's private.
    private int strength=10;
     [SerializeField]
    private int agility = 10;
     [SerializeField]
    private int intelligence = 10;
     [SerializeField]
    private int dexterity = 10;
     [SerializeField]
    private int vigour = 10;
     [SerializeField]
    private int level
     [SerializeField]
    private float currentXP=0;
     [SerializeField]
    private float neededXP;

 
    

    


    // Update is called once per frame
    void Update()
    {
        
    }

    void LevelUp()
    {
        
    }

    void Death()
    {
  
     
}

}
