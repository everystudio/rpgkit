using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;

namespace rpgkit
{
    [CreateAssetMenu(fileName = "Reference", menuName = "ScriptableObject/ScriptableReference")]
    public class ScriptableReference : ScriptableObject
    {
        [SerializeField]
        private GameObject instantiateReference;

        [System.NonSerialized]
        private System.Action<GameObject> dispatchEvent = delegate { };

        [System.NonSerialized]
        private GameObject reference;

        public GameObject Reference
        {
            get
            {
                if (reference == null && instantiateReference != null)
                {
                    reference = GameObject.Instantiate(instantiateReference);
                }

                return reference;
            }

            set
            {
                reference = value;

                if (reference != null)
                {
                    if (dispatchEvent != null)
                    {
                        dispatchEvent.Invoke(reference);
                    }
                }
                else
                {
                    if (dispatchEvent != null)
                    {
                        foreach (Delegate d in dispatchEvent.GetInvocationList())
                        {
                            dispatchEvent -= (System.Action<GameObject>)d;
                        }
                    }
                }
            }
        }

        public void AddListener(System.Action<GameObject> listener)
        {
            dispatchEvent += listener;

            if (reference != null)
            {
                listener.Invoke(reference);
            }
        }

        public void RemoveListener(System.Action<GameObject> listener)
        {
            dispatchEvent -= listener;
        }
    }
}

