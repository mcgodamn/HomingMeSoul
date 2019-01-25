using System;
using UnityEngine.Events;

namespace NoBrainer.Utility
{
    public class UnityGameEvent<EventArgType> : UnityEvent<object, EventArgType>
        where EventArgType : EventArgs
    {
    }
}
