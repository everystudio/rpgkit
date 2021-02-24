using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace rpgkit
{
    [CustomEditor(typeof(SceneFlagManager))]
    public class SceneFlagManagerEditor : MetaEditor
    {
        public override Object FindTarget()
        {
            if (SceneFlagManager.Instance == null)
            {
                SceneFlagManager.Instance = FindObjectOfType<SceneFlagManager>();
            }
            return SceneFlagManager.Instance;
        }
        public override void OnInspectorGUI()
        {
            if (!metaTarget)
            {
                Debug.Log("notfound metatarget");
                return;
            }
            SceneFlagManager main = (SceneFlagManager)metaTarget;

            Undo.RecordObject(main, "");
            Color defColor = GUI.color;

            main.m_textAssetSource = (TextAsset)EditorGUILayout.ObjectField(
                main.m_textAssetSource,
                typeof(TextAsset), true, GUILayout.Width(200));

            #region Flag
            GUILayout.Label("Flags", GUILayout.ExpandWidth(true));
            MasterFlag master_flag = new MasterFlag();
            if(main.m_textAssetSource != null)
            {
                master_flag.Load(main.m_textAssetSource);
            }
            foreach( MasterFlagParam param in master_flag.list)
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label(
                    param.id.ToString(),
                    EditorStyles.label, GUILayout.Width(10));

                GUILayout.Label(
                    param.name,
                    EditorStyles.label, GUILayout.Width(100));

                EditorGUILayout.EndHorizontal();
            }


            #endregion

        }


    }
}




