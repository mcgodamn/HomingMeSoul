using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BA_Studio.StatePattern;
using BA_Studio.UnityLib.SingletonLocator;

namespace AngerStudio.HomingMeSoul.Core
{

    public class AppCore : MonoBehaviour
    {
        public static AppCore Instance { get => SingletonBehaviourLocator<AppCore>.Instance; }

        StateMachine<AppCore> stateMachine;

        public AppConfig config;

        void Awake ()
        {
            stateMachine = new StateMachine<AppCore>(this);
            SingletonBehaviourLocator<AppCore>.Set(this);

            avaliableColors = new List<Color>(config.playerColorPool);
        }

        public List<(KeyCode, Color)> activePlayers;
        List<Color> avaliableColors;

        public void AddPlayer (KeyCode slot)
        {
            if (avaliableColors.Count == 0) return;
            int r = Random.Range(0, avaliableColors.Count);
            activePlayers.Add((slot, avaliableColors[r]));
            avaliableColors.RemoveAt(r);
        }
        
    }

    [System.Serializable]
    public class AppConfig
    {
        public Color[] playerColorPool;

        public int maxPlayers;

        public KeyCode[] keySets = { KeyCode.Q, KeyCode.Z, KeyCode.E, KeyCode.C, KeyCode.T, KeyCode.B, KeyCode.U, KeyCode.M };


    }
}