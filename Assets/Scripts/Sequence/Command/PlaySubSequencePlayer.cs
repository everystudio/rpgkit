using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sequence
{
    public class PlaySubSequencePlayer : SequenceBase
    {
        public SequencePlayer m_subSequencePlayer;

        protected override IEnumerator CustomPlaySequence(Vector3 position, float attenuation = 1)
        {
            yield return StartCoroutine(m_subSequencePlayer.PlaySequences(()=> { }));
        }
    }
}



