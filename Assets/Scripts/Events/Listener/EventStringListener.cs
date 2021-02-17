using UnityEngine;
using System.Collections;
using UnityEngine.Events;

[AddComponentMenu("Events/EventStringListener")]
public class EventStringListener : ScriptableEventListener<string>
{
    [SerializeField]
    protected EventString eventObject;

    [SerializeField]
    protected UnityEventString eventAction;

    protected override ScriptableEvent<string> ScriptableEvent
    {
        get
        {
            return eventObject;
        }
    }

    protected override UnityEvent<string> Action
    {
        get
        {
            return eventAction;
        }
    }
}
