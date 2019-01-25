using System;

namespace NoBrainer.Utility
{
    [Serializable]
    public class VoidGameEventReference : GameEventReference<EventArgs, VoidGameEvent, GlobalVoidGameEvent>
    { }
}
