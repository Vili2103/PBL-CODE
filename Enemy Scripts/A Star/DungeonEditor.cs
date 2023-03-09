using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AbstractDungeonGen), true)]
public class DungeonEditor : Editor
{
    AbstractDungeonGen generator;
    // THIS SCRIPT EXISTS SO I CAN GENERATE DUNGEONS IN THE EDITOR WITHOUT RUNNING THE GAME

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
        if(GUILayout.Button("Clear Tiles")){
            generator.ClearAll();
        }
    }
}
