using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BA_Studio.StatePattern;

namespace AngerStudio.HomingMeSoul.Core
{

    public class AppCore : MonoBehaviour
    {
        
        StateMachine<AppCore> stateMachine;

        void Awake ()
        {
            stateMachine = new StateMachine<AppCore>(this);
        }
        
    }
}