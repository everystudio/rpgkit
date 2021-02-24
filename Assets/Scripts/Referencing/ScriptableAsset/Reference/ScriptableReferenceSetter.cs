using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace rpgkit
{
    public class ScriptableReferenceSetter : MonoBehaviour
    {
        [SerializeField]
        private ScriptableReference target;

        private void Awake()
        {
            target.Reference = this.gameObject;
        }
    }
}

