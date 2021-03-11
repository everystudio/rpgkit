using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace sequence
{
    public enum TimescaleModes {
        Scaled,
        Unscaled
    }

    [System.Serializable]
    public class SequenceTiming
    {
        [Header("Timescale")]
        public TimescaleModes TimescaleMode = TimescaleModes.Scaled;

        [Header("Delays")]
        public float InitialDelay = 0f;
        public float CooldownDuration = 0f;

        [Header("Repeat")]
        public int NumberOfRepeats = 0;
        public bool RepeatForever = false;
        public float DelayBetweenRepeats = 1f;

    }
}



