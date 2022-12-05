using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
[CustomEditor(typeof(AbstractDungeonGen),true)]
public class DungeonEditor : Editor 
{
    // THIS SCRIPT EXISTS SO I CAN GENERATE DUNGEONS IN THE EDITOR WITHOUT RUNNING THE GAME
    
    AbstractDungeonGen generator;

    private void Awake()
    {
        generator = (AbstractDungeonGen)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUILayout.Button("Generate Dungeon"))
        {
            generator.GenerateDungeon();
        }
    }
}
