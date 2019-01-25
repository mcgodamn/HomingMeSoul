using System;
using UnityEngine;

namespace NoBrainer.Utility
{
    public class GlobalGameEvent<GameEventType, EventArgType> : GlobalVariable<GameEventType>, IGameEvent<EventArgType>
        where GameEventType : GameEvent<EventArgType>
        where EventArgType : EventArgs
    {
        public void Subscribe(EventHandler<EventArgType> subscriber)
        {
            Value.Subscribe(subscriber);
        }

        public void Unsubscribe(EventHandler<EventArgType> subscriber)
        {
            Value.Unsubscribe(subscriber);
        }

        public void RaiseEvent(EventArgType arg1)
        {
            Value.RaiseEvent(arg1);
        }

        public void RaiseEvent()
        {
            Value.RaiseEvent(default(EventArgType));
        }
    }
}