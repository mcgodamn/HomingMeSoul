using System;
using UnityEngine;

namespace NoBrainer.Utility
{
    public abstract class GameEvent : IGameEvent
    {
        public abstract void RaiseEvent();
    }

    public class GameEvent<EventArgType> : GameEvent, IGameEvent<EventArgType>
        where EventArgType : EventArgs
    {
        public event EventHandler<EventArgType> GameEventOccurred;

#if UNITY_EDITOR
        public EventArgType inspectorArgs;
#endif

        public override void RaiseEvent()
        {
            if (GameEventOccurred != null)
#if UNITY_EDITOR
                GameEventOccurred.Invoke(this, inspectorArgs);
#else
                GameEventOccurred.Invoke(this, default(EventArgType));
#endif
        }

        public void RaiseEvent(EventArgType args)
        {
            if (GameEventOccurred != null)
                GameEventOccurred.Invoke(this, args);
        }

        public void Subscribe(EventHandler<EventArgType> subscriber)
        {
            Unsubscribe(subscriber);
            GameEventOccurred += subscriber;
        }

        public void Unsubscribe(EventHandler<EventArgType> subscriber)
        {
            GameEventOccurred -= subscriber;
        }
    }
}
