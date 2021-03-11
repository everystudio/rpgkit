using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sequence
{
    [AddComponentMenu("")]
    [SequencePath("GameObject/SetActive")]
    public class SequenceGameObjectSetActive : SequenceBase
    {
        [Header("Target")]
        public GameObject Target;

        public bool SetFlag = false;

        protected override IEnumerator CustomPlaySequence(Vector3 position, float attenuation = 1)
        {
            if(Target != null)
            {
                Target.SetActive(SetFlag);
            }
            yield break;
        }
    }
}



