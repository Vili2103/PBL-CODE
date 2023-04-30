using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestScript : MonoBehaviour
{
    [SerializeField]
    private List<WeaponMaker> weaponsList = new List<WeaponMaker>();
    public void openChest()
    {
        var offset = new Vector3(0.5f, 0.5f, 0);
        Instantiate(getRandomItem(), transform.position + offset, transform.rotation);
    }
    public GameObject getRandomItem()
    {
        GameObject item = null;
        var totalDropRate = 0;
        foreach(var weapon in weaponsList)
        {
            totalDropRate += weapon.dropRate;
        }
        var rng = Random.Range(1, totalDropRate + 1);
        var processedWeight = 0;
        foreach(var weapon in weaponsList)
        {
            processedWeight += weapon.dropRate;
            if (rng <= processedWeight)
            {
                item = weapon.weaponPrefab;
                break;
            }
        }
        return item;
    }
}
