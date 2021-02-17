using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace rpgkit
{
    public class AddCoreScene : Editor
    {
        [MenuItem("Tools/AddCoreScene")]
        public static void AddScene()
        {
            if (EditorSceneManager.GetActiveScene().name != "Core")
            {
                //Debug.Log()
                Scene openScene = EditorSceneManager.OpenScene("Assets/Scenes/Core.unity", OpenSceneMode.Additive);

                /*
                Scene openScene = EditorSceneManager.OpenScene("Scenes/Core", OpenSceneMode.Additive);
                
                if (!openScene.IsValid())
                {
                    return;
                }
                */
            }

        }
    }
}


