using System;
using UnityEngine;
using UnityEngine.Events;

namespace NoBrainer.Utility
{
    public abstract class AdaptingGameEventListener<EventArgType, GameEventType, GlobalEventType> : MonoBehaviour
        where EventArgType : EventArgs
        where GameEventType : GameEvent<EventArgType>
        where GlobalEventType : GlobalGameEvent<GameEventType, EventArgType>
    {
        public GlobalEventType globalEvent;

        public void OnEnable()
        {
            Subscribe();
        }

        public void OnDisable()
        {
            Unsubscribe();
        }

        public void Subscribe()
        {
            globalEvent.Subscribe(RaiseResponse);
        }

        public void Unsubscribe()
        {
            globalEvent.Unsubscribe(RaiseResponse);
        }

        public abstract void RaiseResponse(object sender, EventArgType parameter);
    }

    public class ForwardingGameEventListener<EventArgType, GameEventType, GlobalEventType, UnityEventType>
         : AdaptingGameEventListener<EventArgType, GameEventType, GlobalEventType>
        where EventArgType : EventArgs
        where GameEventType : GameEvent<EventArgType>
        where GlobalEventType : GlobalGameEvent<GameEventType, EventArgType>
        where UnityEventType : UnityGameEvent<EventArgType>
    {
        public UnityEventType response;

        public override void RaiseResponse(object sender, EventArgType parameter)
        {
            response.Invoke(sender, parameter);
        }
    }
}
