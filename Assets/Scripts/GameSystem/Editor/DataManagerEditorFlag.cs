using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace rpgkit
{
    [CustomEditor(typeof(DataManager))]
    public class DataManagerEditorFlag : MetaEditor
    {
        public override Object FindTarget()
        {
            if (DataManager.Instance == null)
            {
                DataManager.Instance = FindObjectOfType<DataManager>();
            }
            return DataManager.Instance;
        }
        public override void Show()
        {
            if (!metaTarget)
            {
                Debug.Log("notfound metatarget");
                return;
            }
            DataManager main = (DataManager)metaTarget;

            Undo.RecordObject(main, "");
            Color defColor = GUI.color;

            MasterFlag master_event = new MasterFlag();
            if (main.m_taMasterFlag != null)
            {
                master_event.Load(main.m_taMasterFlag);
            }

            #region Event
            GUILayout.Label("Events", GUILayout.ExpandWidth(true));
            foreach( MasterFlagParam param in master_event.list)
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




