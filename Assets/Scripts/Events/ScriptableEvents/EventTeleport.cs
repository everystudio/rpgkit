using UnityEngine;
using System.Collections;
using rpgkit;
using System;

[CreateAssetMenu(menuName = "Events/Teleport Event")]
public class EventTeleport : ScriptableEvent<TeleportLocation>
{
    internal void AddListener(EventTeleport onWarpRequest)
    {
        throw new NotImplementedException();
    }

}
