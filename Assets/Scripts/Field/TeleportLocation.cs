using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace rpgkit
{
    [CreateAssetMenu(
        fileName = "TeleporertLocation",
        menuName = "ScriptableObject/TeleporertLocation")]
    public class TeleportLocation : ScriptableAsset
    {
        [SerializeField]
        private string scene;

        public string Scene { get { return scene; } }

        [SerializeField]
        private Vector2 position;
        public Vector2 Position { get { return position; } }

#if UNITY_EDITOR

        [SerializeField]
        private string setterName;

        [SerializeField]
        private string scenePath;

        public bool IsTarget(SceneTeleporter warper)
        {
            return warper != null && (warper.name == setterName && warper.gameObject.scene.name == scene);
        }

        public void Set(SceneTeleporter teleporter)
        {
            position = teleporter.SpawnLocation;
            scene = teleporter.gameObject.scene.name;
            scenePath = teleporter.gameObject.scene.path;
            teleporter.name = $"TL_{scene}_{position}";
            setterName = teleporter.name;

            UnityEditor.EditorUtility.SetDirty(this);
        }

        public void GoToLocation()
        {
            if (EditorSceneManager.GetActiveScene().isDirty)
            {
                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            }

            if (EditorSceneManager.GetActiveScene().name != scene)
            {
                Scene openScene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);

                if (!openScene.IsValid())
                {
                    return;
                }
                // 編集の都合
                // ここでCoreSceneを追加
                Scene corescene = EditorSceneManager.OpenScene("Assets/Scenes/Core.unity", OpenSceneMode.Additive);
            }

            GameObject target = GameObject.Find(setterName);

            if (target != null)
            {
                Vector3 position = SceneView.lastActiveSceneView.pivot;
                position.z -= 10.0f;
                SceneView.lastActiveSceneView.pivot = target.transform.position;
                SceneView.lastActiveSceneView.Repaint();
                Selection.activeGameObject = target;
            }

        }
#endif
    }


#if UNITY_EDITOR

    [CustomEditor(typeof(TeleportLocation))]
    public class TeleportLocationInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            GUI.enabled = false;
            base.OnInspectorGUI();
            GUI.enabled = true;

            if (GUILayout.Button("Go to location"))
            {
                ((TeleportLocation)target).GoToLocation();
            }
        }
    }
#endif

}



