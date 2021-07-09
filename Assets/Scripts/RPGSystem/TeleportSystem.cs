using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using gamesystem;
using anogamelib;

namespace rpgkit
{
    public class TeleportSystem : GameSystem
    {
        [SerializeField]
        private EventTeleport RequestTeleport;

        [SerializeField]
        private EventString OnSceneTeleport;

        [SerializeField]
        private ScriptableReference player;

        [SerializeField]
        private EventFloat onTeleportStart;

        [SerializeField]
        private EventFloat onTeleportEnd;

        [SerializeField]
        private float sceneWarpTime;

        private string currentScene;

        public override void OnLoadSystem()
        {
            RequestTeleport?.AddListener(Warp);
            currentScene = SceneManager.GetActiveScene().name;
        }

        private void OnDestroy()
        {
            RequestTeleport?.RemoveListener(Warp);
        }

        private void Start()
        {
            OnSceneTeleport?.Invoke(currentScene);
        }

        private void Warp(TeleportLocation location)
        {
            onTeleportStart?.Invoke(sceneWarpTime * 0.5f);
            FadeScreen.Instance.Fadeout(1.0f, () =>
            {
                StartCoroutine(SwitchScene(location.Scene, currentScene, location.Position));
            });
        }

        private IEnumerator SwitchScene(string target, string previous, Vector3 playerLocation)
        {
            // If within the same scene, just warp the player
            if (target == previous)
            {
                player.Reference.transform.position = playerLocation;
                yield break;
            }

            if (!Application.CanStreamedLevelBeLoaded(target))
            {
                Debug.Log($"Could not load scene: {target}. Ensure it is added to the build settings.");
                yield break;
            }

            yield return new WaitForSeconds(sceneWarpTime * 0.5f);

            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(target, LoadSceneMode.Additive);
            asyncOperation.allowSceneActivation = false;

            AsyncOperation unloadPreviousScene = SceneManager.UnloadSceneAsync(previous);

            while (asyncOperation.progress != 0.9f)
            {
                yield return null;
            }

            currentScene = target;

            yield return new WaitForSeconds(sceneWarpTime * 0.5f);

            asyncOperation.allowSceneActivation = true;

            yield return new WaitUntil(() => asyncOperation.isDone);

            SceneManager.SetActiveScene(SceneManager.GetSceneByName(target));

            player.Reference.transform.position = playerLocation;

            yield return new WaitUntil(() => unloadPreviousScene.isDone);

            FadeScreen.Instance.Fadein(1.0f, () =>
            {
                onTeleportEnd?.Invoke(sceneWarpTime * 0.5f);
                OnSceneTeleport?.Invoke(target);
            });
        }

        [System.Serializable]
        public struct RuntimeData
        {
            public string scene;
        }
    }
}

