using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace rpgkit
{
    [CustomEditor(typeof(ChestManager))]
    public class ChestManagerEditor : MetaEditor
    {
        public override Object FindTarget()
        {
            if (ChestManager.Instance == null)
            {
                ChestManager.Instance = FindObjectOfType<ChestManager>();
            }
            return ChestManager.Instance;
        }
        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();

            string scene_name = SceneManager.GetActiveScene().name;

            GUILayout.Label(
                string.Format("Current Scene：{0}", scene_name),
                GUILayout.ExpandWidth(true));

            GUILayout.Space(20);

            GUILayout.BeginHorizontal();
            GUILayout.Label("ChestId", EditorStyles.label, GUILayout.Width(50));
            GUILayout.Label("ItemName", EditorStyles.label, GUILayout.Width(200));
            GUILayout.EndHorizontal();

            ChestBase[] chestArr = FindObjectsOfType<ChestBase>();
            int chest_number = 0;
            foreach( ChestBase chest in chestArr)
            {
                GUILayout.BeginHorizontal();

                string strChestId = string.Format("{0}_{1}", scene_name, chest_number);

                GUILayout.Label(strChestId, EditorStyles.label, GUILayout.Width(50));

                GUILayout.Label(
                    chest.GetInstanceID().ToString(),
                    EditorStyles.label, GUILayout.Width(200));

                GUILayout.EndHorizontal();

                chest_number += 1;
            }


        }

    }
}





