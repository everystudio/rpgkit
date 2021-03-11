using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sequence
{
    [AddComponentMenu("")]
    [SequencePath("Debug/Log")]
    public class SequenceDebugLog : SequenceBase
    {
        public enum DebugLogModes { DebugLogTime, Log, Assertion, Error, Warning }

        [Header("Debug")]
        public DebugLogModes DebugLogMode = DebugLogModes.Log;

        [TextArea]
        public string DebugMessage;

        protected override IEnumerator CustomPlaySequence(Vector3 position, float attenuation = 1)
        {
            if (Config.Active)
            {
                switch (DebugLogMode)
                {
                    case DebugLogModes.Assertion:
                        Debug.LogAssertion(DebugMessage);
                        break;
                    case DebugLogModes.Log:
                        Debug.Log(DebugMessage);
                        break;
                    case DebugLogModes.Error:
                        Debug.LogError(DebugMessage);
                        break;
                    case DebugLogModes.Warning:
                        Debug.LogWarning(DebugMessage);
                        break;
                }
            }
            yield break;
        }
    }
}




