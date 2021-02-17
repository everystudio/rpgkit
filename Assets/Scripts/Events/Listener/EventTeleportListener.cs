using UnityEngine;
using System.Collections;
using UnityEngine.Events;

using rpgkit;

[AddComponentMenu("Events/EventTeleportListener")]
public class EventTeleportListener : MonoBehaviour//ScriptableEventListener<TeleporterLocation>
{
    [SerializeField]
    private EventTeleport scriptableEvent;

    [SerializeField]
    private UnityEventVector2 action;

    public void Dispatch(Vector2 parameter)
    {
        action?.Invoke(parameter);
    }

    public void OnEnable()
    {
        scriptableEvent?.AddListener(OnTeleportEvent);
    }

    public void OnDisable()
    {
        scriptableEvent?.RemoveListener(OnTeleportEvent);
    }

    private void OnTeleportEvent(TeleportLocation warpEvent)
    {
        Dispatch(warpEvent.Position);
    }
}
