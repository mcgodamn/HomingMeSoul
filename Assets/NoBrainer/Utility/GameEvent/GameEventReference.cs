using System;

namespace NoBrainer.Utility
{
    public class GameEventReference<EventArgType, GameEventType, GlobalGameEventType> : VariableReference<GameEventType, GlobalGameEventType>, IGameEvent<EventArgType>
        where EventArgType : EventArgs
        where GameEventType : GameEvent<EventArgType>
        where GlobalGameEventType : GlobalGameEvent<GameEventType, EventArgType>
    {
        public void Subscribe(EventHandler<EventArgType> subscriber)
        {
            Unsubscribe(subscriber);

            Value.Subscribe(subscriber);
        }

        public void Unsubscribe(EventHandler<EventArgType> subscriber)
        {
            if (GlobalVariable != null)
                GlobalVariable.Unsubscribe(subscriber);

            LocalValue.Unsubscribe(subscriber);
        }
        public void RaiseEvent(EventArgType arg1)
        {
            Value.RaiseEvent(arg1);
        }

        public void RaiseEvent()
        {
            Value.RaiseEvent();
        }
    }

}