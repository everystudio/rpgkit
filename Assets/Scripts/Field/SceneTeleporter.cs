using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

# if UNITY_EDITOR
using UnityEditor;
#endif

namespace rpgkit
{
    [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
    public class SceneTeleporter : MonoBehaviour
    {
        [SerializeField]
        private TeleportLocation location;

        [SerializeField]
        private TeleportLocation target;

        [SerializeField]
        private EventTeleport teleportRequest;

        [SerializeField]
        private GameObject spawnLocation;

        public Vector2 SpawnLocation { get { return spawnLocation.transform.position; } }

        public TeleportLocation Location { get { return location; } set { location = value; } }
        public TeleportLocation Target { get { return target; } set { target = value; } }

        [SerializeField, HideInInspector]
        private BoxCollider2D boxCollider;

        [SerializeField, HideInInspector]
        private Rigidbody2D rigidBody;


        private void OnDrawGizmos()
        {
            RPGDebug.DrawCube(transform.position, Color.cyan, Vector3.one * 0.5f);
            // Show ExitPoint
            RPGDebug.DebugDrawCross(
                new Vector3(
                    this.SpawnLocation.x ,
                    this.SpawnLocation.y ,
                    0.0f), 0.25f, Color.yellow);
            RPGDebug.DrawPoint(
                new Vector3(
                    this.SpawnLocation.x,
                    this.SpawnLocation.y,
                    0.0f), Color.yellow, 0.25f);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if( collision.tag == "Player")
            {
                //SceneManager.LoadScene(Target.Scene);
                if (Target != null)
                {
                    Debug.Log(Target);
                    teleportRequest.Invoke(Target);
                }
            }

        }

        public void OnValidate()
        {
            if (this.transform.position.z != 0)
            {
                Vector3 newPosition = this.transform.position;
                newPosition.z = 0;
                this.transform.position = newPosition;
            }

            if (spawnLocation == null)
            {
                spawnLocation = new GameObject();
                spawnLocation.name = "Spawn Location";
                spawnLocation.transform.SetParent(this.transform);
                spawnLocation.transform.position = this.transform.position;
            }
            else
            {
                if (spawnLocation.transform.position.z != 0)
                {
                    Vector3 newPosition = spawnLocation.transform.position;
                    newPosition.z = 0;
                    spawnLocation.transform.position = newPosition;
                }
            }

            // Ensure box collider is set to trigger once it is first referenced.
            if (boxCollider == null)
            {
                boxCollider = GetComponent<BoxCollider2D>();
                boxCollider.isTrigger = true;
                boxCollider.size = new Vector2(0.2f, 0.1f);
            }

            // Ensure rigidbody body type is set to startic once it is first referenced.
            if (rigidBody == null)
            {
                rigidBody = GetComponent<Rigidbody2D>();
                rigidBody.bodyType = RigidbodyType2D.Static;
            }
        }
    }

#if UNITY_EDITOR

    [CustomEditor(typeof(SceneTeleporter))]
    public class SceneTeleporterInspector : Editor
    {
        private SceneTeleporter warperReference;

        private string warperLocationName;

        public override void OnInspectorGUI()
        {
            if (warperReference == null)
            {
                warperReference = (target as SceneTeleporter);
            }

            base.OnInspectorGUI();

            if (warperReference.Target == null)
                GUI.enabled = false;

            if (GUILayout.Button("Go to target location"))
            {
                warperReference.Target.GoToLocation();
            }

            GUI.enabled = true;

            if (warperReference.Location == null)
            {
                GUILayout.BeginVertical(GUI.skin.box);

                if (string.IsNullOrEmpty(warperLocationName))
                {
                    warperLocationName = $"{warperReference.gameObject.scene.name}_";
                }

                warperLocationName = EditorGUILayout.TextField("Location Name", warperLocationName);

                if (GUILayout.Button("Create Teleporter Location Asset"))
                {
                    if (string.IsNullOrEmpty(warperLocationName))
                    {
                        warperLocationName = "location";
                    }

                    TeleportLocation warpLocation = new TeleportLocation();
                    string uniquePath = AssetDatabase.GenerateUniqueAssetPath($"Assets/ScriptableObjects/Locations/{warperLocationName}.asset");

                    AssetDatabase.CreateAsset(warpLocation, uniquePath);
                    warperReference.Location = warpLocation;
                    warperReference.OnValidate();

                    warperReference.Location.Set(warperReference);

                    EditorUtility.SetDirty(warperReference.gameObject);
                    UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene());
                    AssetDatabase.SaveAssets();
                }

                GUILayout.EndVertical();
            }

            if (!warperReference.Location.IsTarget(warperReference) ||
                (warperReference.SpawnLocation != warperReference.Location.Position ))
            {
                if (GUILayout.Button("UpdateLocation"))
                {
                    warperReference.Location.Set(warperReference);
                }
            }


        }
    }

#endif

}



