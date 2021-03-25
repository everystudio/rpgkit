using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using sequence;

namespace rpgkit
{
    [AddComponentMenu("")]
    [SequencePath("rpgkit/FlagSetter")]
    public class SQFlagSetter : SequenceBase
    {
        public int flag_id;
        public bool is_completed;

        protected override IEnumerator CustomPlaySequence(Vector3 position, float attenuation = 1)
        {
            DataManager.Instance.m_dataFlag.Write(flag_id, is_completed);
            yield break;
        }
    }
}
