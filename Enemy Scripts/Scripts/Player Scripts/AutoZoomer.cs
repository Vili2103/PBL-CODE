using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class AutoZoomer : MonoBehaviour
{
    public CinemachineVirtualCamera cam;

    private Vector2Int pos;
    float fov = 8f;

    private void Start()
    {
        var player = gameObject;
        pos = new Vector2Int((int)player.transform.position.x, (int)player.transform.position.y);
        
    }

    private void FixedUpdate()
    {
        int walls = 0;
        if (CorridorFirstDungeonGenerator.corridorPositions.Contains(pos))
        {
            foreach (var dir in Direction2D.dirList)
            {
                if (TileMaker.wallPositions.Contains(dir + pos))
                    walls++;
            }
            if (walls == 2)
            {
                zoomIn();
              
            }
               
            else zoomOut();
            
        }
       
    }

    private void zoomIn()
    {
        fov = cam.m_Lens.OrthographicSize-= 5f;       
    }

    private void zoomOut()
    {
        fov = cam.m_Lens.FieldOfView = 8;
    }
}
