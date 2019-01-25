using System;
using UnityEngine;

namespace NoBrainer.Utility
{
    [Serializable, CreateAssetMenu]
    public class GlobalVoidGameEvent : GlobalGameEvent<VoidGameEvent, EventArgs>, IGameEvent
    { }
}