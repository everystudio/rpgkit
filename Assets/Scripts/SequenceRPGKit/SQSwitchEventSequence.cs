using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using anogamelib;

namespace rpgkit
{
    [AddComponentMenu("")]
    [SequencePath("rpgkit/SwichEventSequence")]
    public class SQSwitchEventSequence : SequenceBase
    {
        public int flag_id;
        public SequencePlayer m_subSequenceTrue;
        public SequencePlayer m_subSequenceFalse;

        protected override IEnumerator CustomPlaySequence(Vector3 position, float attenuation = 1)
        {
            //Debug.Log(DataManager.Instance.m_dataFlag.Check(flag_id));
            yield return StartCoroutine(
                (DataManager.Instance.m_dataFlag.Check(flag_id)?
                m_subSequenceTrue: m_subSequenceFalse).PlaySequences(() => { }));
        }
    }
}


