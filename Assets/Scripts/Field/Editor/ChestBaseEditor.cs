using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace rpgkit
{
    [CustomEditor(typeof(ChestBase))]
    public class ChestBaseEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            ChestBase script = target as ChestBase;

            base.OnInspectorGUI();

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Open"))
            {
                script.Open();
            }
            if (GUILayout.Button("Close"))
            {
                script.Close();
            }

            GUILayout.EndHorizontal();


        }

    }
}



