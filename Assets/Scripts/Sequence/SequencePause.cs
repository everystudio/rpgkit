using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sequence
{
    public class SequencePause : SequenceBase
    {
        public override YieldInstruction Pause { get { return _waitForSeconds; } }
        protected WaitForSeconds _waitForSeconds;

        [Header("Pause")]
        public float PauseDuration = 1f;

        protected override IEnumerator CustomPlaySequence(Vector3 position, float attenuation = 1.0f)
        {
            if (Config.Active)
            {
                yield return StartCoroutine(PlayPause());
            }
        }
        protected virtual IEnumerator PlayPause()
        {
            yield return Pause;
        }
        protected virtual void CacheWaitForSeconds()
        {
            _waitForSeconds = new WaitForSeconds(PauseDuration);
        }
        protected virtual void OnValidate()
        {
            CacheWaitForSeconds();
        }
    }
}



