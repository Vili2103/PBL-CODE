using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SeekBehaviour : SteeringBehaviour
{
    [SerializeField]
    private float targetReachedThreshold = 0.5f;

    [SerializeField]
    private bool showGizmo = true;

    bool reachedLastTarget = true;

    private Vector2 targetPosCache;
    private float[] interestsTemp;

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        if(aiData.targets == null || aiData.targets.Count <= 0)
        {
            aiData.currentTarget = null;
            return (danger, interest);
        }
        else
        {
            reachedLastTarget = false;
            aiData.currentTarget = aiData.targets.OrderBy(target => Vector2.Distance(target.position, transform.position)).FirstOrDefault();
            //Find closest target
        }
        if(aiData.currentTarget!= null && aiData.targets != null && aiData.targets.Contains(aiData.currentTarget))
        {
            targetPosCache = aiData.currentTarget.position;
        }
        if (Vector2.Distance(transform.position, targetPosCache) < targetReachedThreshold)
        {
            reachedLastTarget = true;
            aiData.currentTarget = null;
            return (danger, interest);
        }
        Vector2 dirToTarget = (targetPosCache - (Vector2)transform.position);
        for(int i = 0; i < interest.Length; i++)
        {
            float result = Vector2.Dot(dirToTarget.normalized, Dir.eightDir[i]);
            if (result > 0)
            {
                float valueToPutIn = result;
                if (valueToPutIn > interest[i])
                {
                    interest[i] = valueToPutIn;
                }
            }
        }
        interestsTemp = interest;
        return (danger, interest);
    }
    private void OnDrawGizmos()
    {

        if (showGizmo == false)
            return;

        Gizmos.DrawSphere(targetPosCache, 0.2f);

        if (Application.isPlaying && interestsTemp != null)
        {
            if (interestsTemp != null)
            {
                Gizmos.color = Color.green;
                for (int i = 0; i < interestsTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Dir.eightDir[i] * interestsTemp[i] * 2);
                }
                if (reachedLastTarget == false)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawSphere(targetPosCache, 0.1f);
                }
            }
        }
    }

}

