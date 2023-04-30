using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextSolver : MonoBehaviour
{
    [SerializeField]
    private bool showGizmos = true;
    float[] interestGizmo = new float[0];
    Vector2 resultDir = Vector2.zero;
    private float rayLength = 1;

    private void Start()
    {
        interestGizmo = new float[8];
    }

    public Vector2 GetDir(List<SteeringBehaviour>behaviours,AIData aiData)
    {
        float[] danger = new float[8];
        float[] interest = new float[8];

        foreach(SteeringBehaviour behaviour in behaviours)
        {
            (danger, interest) = behaviour.GetSteering(danger, interest, aiData);
        }

        for(int i =0; i<8; i++)
        {
            interest[i] = Mathf.Clamp01(interest[i] - danger[i]); //Mathf.Clamp01 returns 0 if the value is negative and 1 if it's greater than 1
        }
        interestGizmo = interest;

        Vector2 outPutDir = Vector2.zero;
        for(int i = 0; i < 8; i++)
        {
            outPutDir += Dir.eightDir[i] * interest[i];
        }
        outPutDir.Normalize();
        resultDir = outPutDir;
        return resultDir;
    }
    private void OnDrawGizmos()
    {
        if (Application.isPlaying && showGizmos == true)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(transform.position, resultDir * rayLength);
        }
    }
}
