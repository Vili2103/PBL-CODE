using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : SteeringBehaviour
{
    [SerializeField]
    private float radius = 2f, agentColliderSize = 0.6f;

    [SerializeField]
    private bool showGizmos = true;

    float[] dangersResultTemp = null;

    public override (float [] danger, float[] interest) GetSteering(float[]danger,float[]interest,AIData aiData) //Anonymus type
    {
        foreach(Collider2D obstacleCollider in aiData.obstacles)
        {
            Vector2 dirToObstacle = obstacleCollider.ClosestPoint(transform.position) - (Vector2)transform.position;
            float distToObstacle = dirToObstacle.magnitude;

            float weight;
            if (distToObstacle <= agentColliderSize)
            {
             weight = 1;
            }
            else
            {
             weight = (radius - distToObstacle) / radius;
            }

            Vector2 dirToObstacleNormalized = dirToObstacle.normalized;

            for(int i = 0; i < Dir.eightDir.Count; i++)
            {
                float result = Vector2.Dot(dirToObstacleNormalized, Dir.eightDir[i]);
                //.Dot is the dotprotuct between the given direction to the obstancle and the current direction 
                float valueToPutIn = result * weight;

                if (valueToPutIn > danger[i])
                {
                    danger[i] = valueToPutIn;
                }
            }
        }
        dangersResultTemp = danger;

        return (danger, interest);
    }
    private void OnDrawGizmos()
    {
        if(showGizmos == false)
        {
            return;
        }
        if(Application.isPlaying && dangersResultTemp != null)
        {
            if(dangersResultTemp != null)
            {
                Gizmos.color = Color.red;
                for(int i = 0; i < dangersResultTemp.Length; i++)
                {
                    Gizmos.DrawRay(transform.position, Dir.eightDir[i] * dangersResultTemp[i] * 2);
                }
            }
        }
    }
}

public static class Dir
{
    public static List<Vector2> eightDir = new List<Vector2>
    {
       new Vector2(0,1).normalized,
       new Vector2(1,1).normalized,
       new Vector2(1,0).normalized,
       new Vector2(1,-1).normalized,
       new Vector2(0,-1).normalized,
       new Vector2(-1,-1).normalized,
       new Vector2(-1,0).normalized,
       new Vector2(-1,1).normalized
        };
}
