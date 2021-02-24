using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace rpgkit
{
    public class SaveSystem : gamesystem.GameSystem
    {
        [SerializeField]
        private SaveGame cachedSaveData;

        [SerializeField]
        private EventSave onGameSave;

        [SerializeField]
        private EventSave onGameLoad;

        //[SerializeField]
        //private GameEvent onNewGameStarted;

        [SerializeField]
        private EventString onChangeScene;

        [SerializeField]
        private EventFloat onTeleportStart;

        [SerializeField]
        private EventFloat onTeleportEnd;

        [SerializeField]
        private IntReference saveSlot;

        [SerializeField]
        private StringReference playerName;

        [System.NonSerialized]
        private bool isNewGame;

        public override void OnLoadSystem()
        {
            Debug.Log("SaveSystem.OnLoadSystem");
            cachedSaveData = SaveUtility.LoadSave(saveSlot.Value);
            Debug.Log(cachedSaveData);
            if (cachedSaveData == null)
            {
                CreateNewSave();
                isNewGame = true;
            }

            onChangeScene?.AddListener(OnChangeScene);
            onTeleportStart?.AddListener(OnTeleportStart);
            onTeleportEnd?.AddListener(OnTeleportEnd);
        }

        private void CreateNewSave()
        {
            cachedSaveData = new SaveGame();

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                Scene getScene = SceneManager.GetSceneAt(i);

                if (getScene.name != "Core")
                {
                    cachedSaveData.lastScene = getScene.name;
                    cachedSaveData.playerName = playerName.Value;
                }
            }
            WriteSaveToFile();
        }

        private void Start()
        {
            onGameLoad?.Invoke(cachedSaveData);
            /*
            if (isNewGame)
            {
                onNewGameStarted.Invoke();
                onGameSave?.Invoke(cachedSaveData);
            }
            */
        }
        private void OnChangeScene(string scene)
        {
            cachedSaveData.lastScene = scene;
            onGameSave?.Invoke(cachedSaveData);
            WriteSaveToFile();
        }

        private void OnTeleportStart(float obj)
        {
            Pause(true);
        }

        private void OnTeleportEnd(float obj)
        {
            //Debug.Log("teleportend");
            onGameLoad?.Invoke(cachedSaveData);
            Pause(false);
        }


        public override void OnTick()
        {
            // Nofify all listeners for onGameSave that game is saved
            onGameSave?.Invoke(cachedSaveData);
#if UNITY_WEBGL && !UNITY_EDITOR
        WriteSaveToFile();
#endif
        }

        private void OnDestroy()
        {
            onGameSave?.Invoke(cachedSaveData);
            WriteSaveToFile();
        }
        private void WriteSaveToFile()
        {
            TimeSpan currentTimePlayed = DateTime.Now - cachedSaveData.saveDate;
            TimeSpan allTimePlayed = cachedSaveData.timePlayed;
            cachedSaveData.timePlayed = allTimePlayed + currentTimePlayed;

            SaveUtility.WriteSave(cachedSaveData, saveSlot.Value);
        }

        public void ForceSave()
        {
            if (cachedSaveData != null)
            {
                onGameSave?.Invoke(cachedSaveData);
            }
        }
    }
}



