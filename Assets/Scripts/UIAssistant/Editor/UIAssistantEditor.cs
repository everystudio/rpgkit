using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.Linq;
//using EditorUtils;

[CustomEditor(typeof(UIAssistant))]
public class UIAssistantEditor : Editor {

    UIAssistant.Page edit = null;

    public override void OnInspectorGUI() {

        UIAssistant main = target as UIAssistant;

        Undo.RecordObject(main, "");
        Color defColor = GUI.color;

        if (main.UImodules == null)
        {
            main.UImodules = new List<Transform>();
        }

        if (main.pages == null)
        {
            main.pages = new List<UIAssistant.Page>();
        }

        #region UI Modules

        GUILayout.Label("UI Modules", GUILayout.ExpandWidth(true));
        EditorGUILayout.BeginVertical();
        for (int i = 0; i < main.UImodules.Count; i++) {
            EditorGUILayout.BeginHorizontal();
            main.UImodules[i] = (Transform) EditorGUILayout.ObjectField(main.UImodules[i], typeof(Transform), true, GUILayout.Width(200));
            if (main.UImodules[i] == null) {
                main.UImodules.RemoveAt(i);
                break;
            } else {
                EditorGUILayout.LabelField(main.UImodules[i].GetComponentsInChildren<PanelPage>(true).Length.ToString() + " panel(s)", EditorStyles.miniBoldLabel, GUILayout.Width(100));
            }
            EditorGUILayout.EndHorizontal();
        }
        Transform new_module = (Transform) EditorGUILayout.ObjectField(null, typeof(Transform), true, GUILayout.Width(150));
        if (new_module)
        {
            main.UImodules.Add(new_module);
        }
        EditorGUILayout.EndVertical();
        #endregion

        #region Pages

        GUILayout.Space(20);
        GUILayout.Label("Pages", GUILayout.ExpandWidth(true));
        main.ArraysConvertation();
        EditorGUILayout.BeginVertical();

        EditorGUILayout.BeginHorizontal();

        GUILayout.Space(10);
        GUILayout.Label("Edit", EditorStyles.centeredGreyMiniLabel, GUILayout.Width(35));
        GUILayout.Label("Name", EditorStyles.centeredGreyMiniLabel, GUILayout.Width(200));

        EditorGUILayout.EndHorizontal();

        foreach (UIAssistant.Page page in main.pages) {

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("X", GUILayout.Width(20))) {
                main.pages.Remove(page);
                break;
            }
            if (GUILayout.Button("Edit", GUILayout.Width(35))) {
                if (edit == page)
                {
                    edit = null;
                }
                else
                {
                    edit = page;
                }
            }
            page.name = EditorGUILayout.TextField(page.name, GUILayout.Width(200));

            UIAssistant.Page default_page = main.pages.Find(x => x.default_page);

            if (default_page == null) {
                default_page = page;
                page.default_page = true;
            }

            if (page.default_page && default_page != page){
                page.default_page = false;
            }
            if (page.default_page)
            {
                GUILayout.Label("DEFAULT", GUILayout.Width(80));

            }
            else
            {
                if (GUILayout.Button("Make default", EditorStyles.miniButton, GUILayout.Width(80)))
                {
                    default_page.default_page = false;
                    default_page = page;
                    page.default_page = true;
                }
            }
                
            EditorGUILayout.EndHorizontal();

            if (edit == page) {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(10);
                EditorGUILayout.BeginVertical(EditorStyles.textArea, GUILayout.Width(300));

                bool active = GUI.enabled;
                GUI.enabled = false;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.Toggle(false, GUILayout.Width(20));
                GUILayout.Label("Show Ads", GUILayout.Width(100));
                EditorGUILayout.EndHorizontal();
                GUI.enabled = active;

                EditorGUILayout.BeginHorizontal();
                page.setTimeScale = EditorGUILayout.Toggle(page.setTimeScale, GUILayout.Width(20));
                GUILayout.Label("Time Scale", GUILayout.Width(100));
                if (page.setTimeScale)
                {
                    page.timeScale = EditorGUILayout.Slider(page.timeScale, 0, 1, GUILayout.Width(150));
                }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Parent", GUILayout.Width(50));
                page.parent_page = EditorGUILayout.TextField(page.parent_page, GUILayout.Width(150));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Name", EditorStyles.boldLabel, GUILayout.Width(150));
                GUILayout.Label("Show", EditorStyles.boldLabel, GUILayout.Width(30));
                GUILayout.Label("Ignor", EditorStyles.boldLabel, GUILayout.Width(30));
                GUILayout.Label("Hide", EditorStyles.boldLabel, GUILayout.Width(30));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginVertical();
                Dictionary<PanelPage, int> mask = new Dictionary<PanelPage, int>();
                foreach (PanelPage panel in main.panels) {
                    mask.Add(panel, -1);
                    if (page.panels.Contains(panel))
                    {
                        mask[panel] = 1;
                    }
                    else if (page.ignoring_panels.Contains(panel))
                    {
                        mask[panel] = 0;
                    }
                }

                foreach (PanelPage panel in main.panels) {
                    EditorGUILayout.BeginHorizontal();
                    switch (mask[panel]) {
                        case -1: GUI.color = Color.red; break;
                        case 0: GUI.color = Color.yellow; break;
                        case 1: GUI.color = Color.green; break;
                    }
                    EditorGUILayout.LabelField(panel.name, GUILayout.Width(150));
                    GUI.color = defColor;

                    if (EditorGUILayout.Toggle(mask[panel] == 1, GUILayout.Width(30)))
                    {
                        mask[panel] = 1;
                    }
                    if (EditorGUILayout.Toggle(mask[panel] == 0, GUILayout.Width(30)))
                    {
                        mask[panel] = 0;
                    }
                    if (EditorGUILayout.Toggle(mask[panel] == -1, GUILayout.Width(30)))
                    {
                        mask[panel] = -1;
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();

                page.panels.Clear();
                page.ignoring_panels.Clear();
                foreach (KeyValuePair<PanelPage, int> pair in mask) {
                    if (pair.Value == 1)
                    {
                        page.panels.Add(pair.Key);
                    }
                    else if (pair.Value == 0)
                    {
                        page.ignoring_panels.Add(pair.Key);
                    }
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }

        }

        if (GUILayout.Button("Add", GUILayout.Width(60)))
        {
            main.pages.Add(new UIAssistant.Page());
        }

        EditorGUILayout.EndVertical();
        #endregion

        GUI.color = defColor;
    }
}

