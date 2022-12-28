using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleDetector : Detector
{   
    [SerializeField]
    private float awarenessRadius = 2;
    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    private bool showGizmos = false;
    Collider2D[] obstacles;
    public override void Detect(AIData aiData)
    {
        obstacles = Physics2D.OverlapCircleAll(transform.position, awarenessRadius, layerMask);
        aiData.obstacles = obstacles;
        //Transform.position -> The center of awareness circle and layermask tells the enemy for things in what layer to look for
    }
    private void OnDrawGizmos()
    {
        if (showGizmos == false)
        {

            return;
        }
        else // There is no need for the else but this way is more readable and I like it.
        {
            if (Application.isPlaying && obstacles != null)
            {
                Gizmos.color = Color.red;
                foreach (Collider2D obstacleCollider in obstacles)
                {
                    Gizmos.DrawSphere(obstacleCollider.transform.position, 0.2f);
                }
            }
        }
    }
}
