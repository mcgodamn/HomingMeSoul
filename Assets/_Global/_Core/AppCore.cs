using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BA_Studio.StatePattern;
using BA_Studio.UnityLib.SingletonLocator;

namespace AngerStudio.HomingMeSoul.Core
{

    public class AppCore : MonoBehaviour
    {
        HashSet<KeyCode> blocked = new HashSet<KeyCode> 
        {
            KeyCode.LeftAlt, KeyCode.RightAlt,
            KeyCode.LeftWindows, KeyCode.RightWindows,
            KeyCode.Escape,
            KeyCode.F1, KeyCode.F2, KeyCode.F3, KeyCode.F4, KeyCode.F5, KeyCode.F6,
            KeyCode.F7, KeyCode.F8, KeyCode.F9, KeyCode.F10, KeyCode.F11, KeyCode.F12,
            KeyCode.ScrollLock, KeyCode.Print, KeyCode.Break
        };

        HashSet<KeyCode> allowed;

        [SerializeField]
        AppView m_View;

        public static AppCore Instance { get => SingletonBehaviourLocator<AppCore>.Instance; }

        StateMachine<AppCore> stateMachine;

        public AppConfig config;

        void Awake ()
        {
            stateMachine = new StateMachine<AppCore>(this);
            SingletonBehaviourLocator<AppCore>.Set(this);

            avaliableColors = new List<Color>(config.playerColorPool);

            allowed = new HashSet<KeyCode>((System.Enum.GetValues(typeof(KeyCode)) as KeyCode[]).Except(blocked));

        }

        public Dictionary<KeyCode, Color> activePlayers;
        List<Color> avaliableColors;

        public void ReceiveInput()
        {
            foreach (KeyCode vKey in allowed)
            {
                if (SimpleInput.GetKeyDown(vKey))
                {
                    if (!activePlayers.Any(kvp => kvp.Key == vKey)) AddPlayer(vKey);
                    else
                    {
                        avaliableColors.Add(activePlayers[vKey]);
                        activePlayers.Remove(vKey);
                        m_View.RemovePlayerCard(vKey);                        
                    }             
                }
            }
        }

        void AddPlayer (KeyCode slot)
        {
            if (avaliableColors.Count == 0 || activePlayers.Count >= config.maxPlayers) return;
            int r = Random.Range(0, avaliableColors.Count);
            activePlayers.Add(slot,avaliableColors[r]);
            m_View.AddPlayerCard(slot, avaliableColors[r]);
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