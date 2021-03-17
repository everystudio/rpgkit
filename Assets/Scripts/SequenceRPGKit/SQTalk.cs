using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sequence;

namespace rpgkit
{
    public class SQTalk : SequenceBase
    {
        public List<string> message;
        protected override IEnumerator CustomPlaySequence(Vector3 position, float attenuation = 1)
        {
            /*
            Debug.Log(TalkManager.Instance);
            Debug.Log(PanelSelect.Instance);
            Debug.Log(Gomitesu.Instance);
            Debug.Log(ChoiceSelect.Instance);
            */
            yield return StartCoroutine(TalkManager.Instance.Talk(message, ()=> { }));
        }
    }
}


