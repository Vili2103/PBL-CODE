using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponMaker : ScriptableObject
{
  
    public  int dropRate;
   
    public  GameObject weaponPrefab;
 
    public  string weaponName;

    public  WeaponMaker(GameObject weaponPrefab,int dropRate)//Object wooow! 
    {
        this.weaponPrefab = weaponPrefab;
        this.dropRate = dropRate; 
    }

}