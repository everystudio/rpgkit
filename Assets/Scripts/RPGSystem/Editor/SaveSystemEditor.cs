using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace rpgkit
{
    [CustomEditor(typeof(SaveSystem))]
    public class SaveSystemEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SaveSystem script = target as SaveSystem;

            if(GUILayout.Button("ForceSave"))
            {
                script.ForceSave();
            }
        }
    }
}

