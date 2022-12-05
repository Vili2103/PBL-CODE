using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="RandomWalkParams_",menuName ="PCG/RandomWalkData")]
public class DungeonScriptableObjects : ScriptableObject
{
    public int loops = 10, stepsToTake = 100;
    public bool startRandomlyEachLoop = true;

}
