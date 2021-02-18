using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace rpgkit
{
    [CustomEditor(typeof(EventManager))]
    public class EventManagerEditor : MetaEditor
    {
        public override Object FindTarget()
        {
            if (EventManager.Instance == null)
            {
                EventManager.Instance = FindObjectOfType<EventManager>();
            }
            return EventManager.Instance;
        }
        public override void OnInspectorGUI()
        {
            if (!metaTarget)
            {
                Debug.Log("notfound metatarget");
                return;
            }
            EventManager main = (EventManager)metaTarget;

            Undo.RecordObject(main, "");
            Color defColor = GUI.color;

            #region Load
            if (GUILayout.Button("AddDummy", GUILayout.Width(80)))
            {
            }

            #endregion

            #region Event
            GUILayout.Label("Events", GUILayout.ExpandWidth(true));
            /*
            foreach( EventDataParam param in main.m_eventData.list)
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
            */


            #endregion

        }


    }
}




