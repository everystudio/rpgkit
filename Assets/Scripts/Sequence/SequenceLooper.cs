using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sequence
{
    public class SequenceLooper : SequencePause
    {
        [Header("Loop conditions")]
        public bool LoopAtLastPause = true;
        public bool LoopAtLastLoopStart = true;

        [Header("Loop")]
        public bool InfiniteLoop = false;
        public int NumberOfLoops = 2;
        public int NumberOfLoopsLeft = 1;
        public bool InInfiniteLoop = false;

        public override bool LooperPause { get { return true; } }
        public override YieldInstruction Pause { get { return _waitForSeconds; } }
        public override float SequenceDuration { get { return PauseDuration; } }

        protected override void CustomInitialization(GameObject owner)
        {
            base.CustomInitialization(owner);
            InInfiniteLoop = InfiniteLoop;
            NumberOfLoopsLeft = NumberOfLoops;
        }
        protected override IEnumerator CustomPlaySequence(Vector3 position, float attenuation = 1.0f)
        {
            if (Config.Active)
            {
                NumberOfLoopsLeft--;
                yield return StartCoroutine(PlayPause());
            }
        }
        protected override void CustomReset()
        {
            base.CustomReset();
            InInfiniteLoop = InfiniteLoop;
            NumberOfLoopsLeft = NumberOfLoops;
        }
    }
}



