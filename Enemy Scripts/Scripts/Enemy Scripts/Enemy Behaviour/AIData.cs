using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIData : MonoBehaviour
{
    public List<Transform> targets = null;
    public Collider2D[] obstacles = null;
    public Transform currentTarget;

    public int getTargetCount()
    {
        if (targets == null)
        {
            return 0;
        }
        else
        {
            return targets.Count;
        }
    }
}
