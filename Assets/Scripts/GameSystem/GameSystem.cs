using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace gamesystem
{
    public abstract class GameSystem : MonoBehaviour
    {
        [System.Serializable]
        public class Settings
        {
            public bool hasUpdate, hasFixedUpdate;

            public float updateDelay = 0;
            public float fixedUpdateDelay = 0;
        }

        public Settings systemSettings;

        private float currentUpdateTime, currentFixedUpdateTime;

        private bool isInitialized;

        private bool pausedUpdate;

        protected void Pause(bool state)
        {
            pausedUpdate = state;

            if (state)
            {
                currentUpdateTime = systemSettings.updateDelay;
                currentFixedUpdateTime = systemSettings.fixedUpdateDelay;
            }
        }

        public void Initialize(SystemTicker _ticker)
        {
            if (!isInitialized)
            {
                OnLoadSystem();
                isInitialized = true;
            }
        }

        public void Tick()
        {
            if (pausedUpdate)
            {
                return;
            }

            if (systemSettings.updateDelay == 0 || currentUpdateTime >= systemSettings.updateDelay)
            {
                OnTick();
                currentUpdateTime = 0;
            }
            else
            {
                currentUpdateTime += Time.deltaTime;
            }
        }

        public void FixedTick()
        {
            if (pausedUpdate)
            {
                return;
            }

            if (systemSettings.fixedUpdateDelay == 0 || currentFixedUpdateTime >= systemSettings.fixedUpdateDelay)
            {
                OnFixedTick();
                currentFixedUpdateTime = 0;
            }
            else
            {
                currentFixedUpdateTime += Time.fixedDeltaTime;
            }
        }

        public abstract void OnLoadSystem();
        public virtual void OnTick() { }
        public virtual void OnFixedTick() { }
    }
}