using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace sequence
{
    [ExecuteAlways]
    [AddComponentMenu("Sequence/SequencePlayer")]
    [DisallowMultipleComponent]
    public class SequencePlayer : MonoBehaviour
    {
        public List<SequenceBase> Sequences = new List<SequenceBase>();
        public enum InitializationModes {
            Script,
            Awake,
            Start
        };
        public enum SafeModes {
            Nope,
            EditorOnly,
            RuntimeOnly,
            Full
        };

        public InitializationModes InitializationMode = InitializationModes.Start;
        public SafeModes SafeMode = SafeModes.Full;

        public bool AutoPlayOnStart = false;
        public bool IsPlaying { get; protected set; }

        protected float _startTime = 0f;
        protected float _holdingMax = 0f;
        protected float _lastStartAt = 0f;

        protected virtual void Awake()
        {
            if ((InitializationMode == InitializationModes.Awake) && (Application.isPlaying))
            {
                Initialization(this.gameObject);
            }
        }
        protected virtual void Start()
        {
            if ((InitializationMode == InitializationModes.Start) && (Application.isPlaying))
            {
                Initialization(this.gameObject);
            }
            if (AutoPlayOnStart && Application.isPlaying)
            {
                PlaySequences();
            }
        }

        public virtual void PlaySequences()
        {
            PlayFeedbacksInternal(this.transform.position, 1.0f);
        }

        protected virtual void PlayFeedbacksInternal(Vector3 position, float attenuation)
        {
            if (!this.isActiveAndEnabled)
            {
                return;
            }

            /*
            if (!GlobalMMFeedbacksActive)
            {
                return;
            }
            */

            _startTime = Time.time;
            _holdingMax = 0f;
            _lastStartAt = _startTime;

            ResetSequences();

            bool pauseFound = false;
            for (int i = 0; i < Sequences.Count; i++)
            {
                if ((Sequences[i].Pause != null) && (Sequences[i].Active))
                {
                    pauseFound = true;
                }
                if ((Sequences[i].HoldingPause == true) && (Sequences[i].Active))
                {
                    pauseFound = true;
                }
            }

            if (!pauseFound)
            {
                IsPlaying = true;
                for (int i = 0; i < Sequences.Count; i++)
                {
                    Sequences[i].Play(position, attenuation);
                }
            }
            else
            {
                StartCoroutine(PausedFeedbacksCo(position, attenuation));
            }
        }

        protected virtual IEnumerator PausedFeedbacksCo(Vector3 position, float attenuation)
        {
            IsPlaying = true;
            for (int i = 0; i < Sequences.Count; i++)
            {
                if ((Sequences[i].Active)
                    && ((Sequences[i].HoldingPause == true) || (Sequences[i].LooperPause == true)))
                {
                    while (Time.time - _lastStartAt < _holdingMax)
                    {
                        yield return null;
                    }

                    _holdingMax = 0f;
                    _lastStartAt = Time.time;
                }

                Sequences[i].Play(position, attenuation);

                if ((Sequences[i].Pause != null)
                    && (Sequences[i].Active))
                {
                    bool shouldPause = true;
                    if (Sequences[i].Chance < 100)
                    {
                        float random = Random.Range(0f, 100f);
                        if (random > Sequences[i].Chance)
                        {
                            shouldPause = false;
                        }
                    }

                    if (shouldPause)
                    {
                        yield return Sequences[i].Pause;
                        _lastStartAt = Time.time;
                        _holdingMax = 0f;
                    }
                }

                if (Sequences[i].Active)
                {
                    if (Sequences[i].Pause == null)
                    {
                        float feedbackDuration = Sequences[i].SequenceDuration + Sequences[i].Timing.InitialDelay + Sequences[i].Timing.NumberOfRepeats * (Sequences[i].SequenceDuration + Sequences[i].Timing.DelayBetweenRepeats);
                        _holdingMax = Mathf.Max(feedbackDuration, _holdingMax);
                    }
                }

                if ((Sequences[i].LooperPause == true)
                    && (Sequences[i].Active)
                    && (((Sequences[i] as SequenceLooper).NumberOfLoopsLeft > 0) || (Sequences[i] as SequenceLooper).InInfiniteLoop))
                {
                    bool loopAtLastPause = (Sequences[i] as SequenceLooper).LoopAtLastPause;
                    bool loopAtLastLoopStart = (Sequences[i] as SequenceLooper).LoopAtLastLoopStart;
                    int newi = 0;
                    for (int j = i - 1; j >= 0; j--)
                    {
                        if (j == 0)
                        {
                            newi = j - 1;
                            break;
                        }
                        if ((Sequences[j].Pause != null)
                            && (Sequences[j].SequenceDuration > 0f)
                            && loopAtLastPause && (Sequences[j].Active))
                        {
                            newi = j;
                            break;
                        }
                        if ((Sequences[j].LooperStart == true)
                            && loopAtLastLoopStart
                            && (Sequences[j].Active))
                        {
                            newi = j;
                            break;
                        }
                    }
                    i = newi;
                }
            }
        }
        public virtual void Initialization()
        {
            for (int i = 0; i < Sequences.Count; i++)
            {
                Sequences[i].Initialization(this.gameObject);
            }
        }

        public virtual void Initialization(GameObject owner)
        {
            if ((SafeMode == SequencePlayer.SafeModes.RuntimeOnly) || (SafeMode == SequencePlayer.SafeModes.Full))
            {
                AutoRepair();
            }

            IsPlaying = false;
            for (int i = 0; i < Sequences.Count; i++)
            {
                if (Sequences[i] != null)
                {
                    Sequences[i].Initialization(owner);
                }
            }
        }

        public virtual void ResetSequences()
        {
            for (int i = 0; i < Sequences.Count; i++)
            {
                Sequences[i].ResetSequence();
            }
            IsPlaying = false;
        }
        public virtual void StopSequences()
        {
            for (int i = 0; i < Sequences.Count; i++)
            {
                Sequences[i].Stop(this.transform.position, 1.0f);
            }
            IsPlaying = false;
        }


        public virtual void AutoRepair()
        {
            List<Component> components = components = new List<Component>();
            components = this.gameObject.GetComponents<Component>().ToList();
            foreach (var component in components)
            {
                if (component is SequenceBase)
                {
                    bool found = false;
                    for (int i = 0; i < Sequences.Count; i++)
                    {
                        if (Sequences[i] == (SequenceBase)component)
                        {
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        Sequences.Add((SequenceBase)component);
                    }
                }
            }
        }

        protected virtual void OnDestroy()
        {
            IsPlaying = false;
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                // we remove all binders
                foreach (SequenceBase sequence in Sequences)
                {
                    EditorApplication.delayCall += () =>
                    {
                        DestroyImmediate(sequence);
                    };
                }
            }
#endif
        }

    }
}
