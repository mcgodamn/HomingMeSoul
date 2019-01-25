using System;
using UnityEngine;

namespace NoBrainer.Utility
{
    public class VoidGameEventListener : ForwardingGameEventListener<EventArgs, VoidGameEvent, GlobalVoidGameEvent, UnityVoidGameEvent>
    { }
}
