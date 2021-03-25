using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class EventFloatListener : ScriptableEventListener<float>
{
    [SerializeField]
    protected EventFloat eventObject;

    [SerializeField]
    protected UnityEventFloat eventAction;

    protected override ScriptableEvent<float> ScriptableEvent
    {
        get
        {
            return eventObject;
        }
    }

    protected override UnityEvent<float> Action
    {
        get
        {
            return eventAction;
        }
    }
}
