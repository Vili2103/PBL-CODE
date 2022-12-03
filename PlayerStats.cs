using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int strength=10;
    public int agility = 10;
    public int intelligence = 10;
    public int dexterity = 10;
    public int vigour = 10;
    public int level;
    public float currentXP=0;
    public float neededXP;

 
    void Update()
    {
        
    }

    void LevelUp()
    {
        
    }

    void Death()
    {
      strength = 10;
      agility = 10;
      intelligence = 10;
      dexterity = 10;
      vigour = 10;
      level=1;
      currentXP = 0;
     
}

}
