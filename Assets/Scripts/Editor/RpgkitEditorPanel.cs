using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace rpgkit
{
	public class RpgkitPanel : EditorWindow
	{
		Color selectionColor;
		Color bgColor;

		[MenuItem("Window/Rpgkit Panel")]
		public static RpgkitPanel CraeteRpgkitPanel()
		{
			RpgkitPanel window = GetWindow<RpgkitPanel>();
			window.titleContent = new GUIContent("Rpgkit Panel");
			window.Show();
			window.OnEnable();
			return window;
		}

		void OnEnable()
		{
			//BPicon = EditorGUIUtility.Load("BerryPanelIcon.png") as Texture;
			selectionColor = Color.Lerp(Color.red, Color.white, 0.7f);
			bgColor = Color.Lerp(GUI.backgroundColor, Color.black, 0.3f);
			//showList = new AnimBool(false);
			//showList.valueChanged.AddListener(Repaint);
			//EditorCoroutine.start(DownloadHelpLibraryRoutine());
		}

		Color defalutColor;
		public Vector2 editorScroll, tabsScroll, levelScroll = new Vector2();
		public System.Action editorRender;
		public MetaEditor editor = null;
		public string editorTitle = "";
		Dictionary<string, Dictionary<string, string>> helpLibrary = new Dictionary<string, Dictionary<string, string>>();


		void OnGUI()
		{
			if (editorRender == null || editor == null)
			{
				editorRender = null;
				editor = null;
			}
			defalutColor = GUI.backgroundColor;
			EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
			GUI.backgroundColor = bgColor;
			EditorGUILayout.BeginVertical(EditorStyles.textArea, GUILayout.Width(150), GUILayout.ExpandHeight(true));
			GUI.backgroundColor = defalutColor;
			tabsScroll = EditorGUILayout.BeginScrollView(tabsScroll);

			DrawTabs();

			EditorGUILayout.EndVertical();
			EditorGUILayout.EndScrollView();

			GUI.backgroundColor = bgColor;
			EditorGUILayout.BeginVertical(EditorStyles.textArea, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
			GUI.backgroundColor = defalutColor;
			editorScroll = EditorGUILayout.BeginScrollView(editorScroll);

			if (!string.IsNullOrEmpty(editorTitle))
			{
				DrawTitle(editorTitle);
			}

			if (editor != null)
			{
				editorRender.Invoke();
			}
			else
			{
				GUILayout.Label("Nothing selected");
			}

			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();

			EditorGUILayout.EndHorizontal();
		}

		void DrawTabs()
		{
			DrawTabTitle("General");

			if (DrawTabButton("UI"))
			{
				editor = CreateInstance<UIAssistantEditor>();
				editorRender = editor.OnInspectorGUI;
			}
			if (DrawTabButton("DataManager-Event"))
			{
				editor = CreateInstance<DataManagerEditorEvents>();
				editorRender = editor.Show;
			}
		}


		bool DrawTabButton(string _strText)
		{
			Color color = GUI.backgroundColor;
			if (editorTitle == _strText)
			{
				GUI.backgroundColor = selectionColor;
			}
			bool result = GUILayout.Button(_strText, EditorStyles.miniButton, GUILayout.ExpandWidth(true));
			GUI.backgroundColor = color;

			if (string.IsNullOrEmpty(editorTitle) || (editorTitle == _strText && editorRender == null))
			{
				result = true;
			}

			if (result)
			{
				EditorGUI.FocusTextInControl("");
				editorTitle = _strText;
			}

			return result;
		}
		void DrawTitle(string _strText)
		{
			EditorGUILayout.BeginHorizontal();
			GUILayout.Label(_strText, EditorStyles.largeLabel, GUILayout.ExpandWidth(true));

			if (helpLibrary.ContainsKey(_strText))
			{
				foreach (string key in helpLibrary[_strText].Keys)
				{
					GUIContent content = new GUIContent(key);
					if (GUILayout.Button(key, EditorStyles.miniButton, GUILayout.Width(EditorStyles.miniButton.CalcSize(content).x)))
						Application.OpenURL(helpLibrary[_strText][key]);
				}
			}

			EditorGUILayout.EndHorizontal();
			GUILayout.Space(10);
		}



		void DrawTabTitle(string _strText)
		{
			GUILayout.Label(_strText, EditorStyles.centeredGreyMiniLabel, GUILayout.ExpandWidth(true));
		}

	}
}





