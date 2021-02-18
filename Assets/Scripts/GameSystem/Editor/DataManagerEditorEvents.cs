using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace rpgkit
{
    [CustomEditor(typeof(DataManager))]
    public class DataManagerEditorEvents : MetaEditor
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

            MasterEvent master_event = new MasterEvent();
            if (main.m_taMasterEvent != null)
            {
                master_event.Load(main.m_taMasterEvent);
            }

            #region Event
            GUILayout.Label("Events", GUILayout.ExpandWidth(true));
            foreach( MasterEventParam param in master_event.list)
            {
                EditorGUILayout.BeginHorizontal();

                GUILayout.Label(
                    param.event_serial.ToString(),
                    EditorStyles.label, GUILayout.Width(10));

                GUILayout.Label(
                    param.event_name,
                    EditorStyles.label, GUILayout.Width(100));

                EditorGUILayout.EndHorizontal();
            }


            #endregion

        }
    }
}




