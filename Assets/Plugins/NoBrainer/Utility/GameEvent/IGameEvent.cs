using System;

namespace NoBrainer.Utility
{
    public interface IGameEvent
    {
        void RaiseEvent();
    }

    public interface IGameEvent<EventArgType> : IGameEvent
        where EventArgType : EventArgs
    {
        void Subscribe(EventHandler<EventArgType> subscriber);
        void Unsubscribe(EventHandler<EventArgType> subscriber);
        void RaiseEvent(EventArgType arg1);
    }
}