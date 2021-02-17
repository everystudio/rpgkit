using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace rpgkit
{
    public class TalkBase : MonoBehaviour
    {
        public string message;

        public void Talk(Action _onFinished)
        {
            StartCoroutine(TalkManager.Instance.Talk(message, _onFinished));
        }

    }
}