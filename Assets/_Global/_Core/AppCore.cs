using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BA_Studio.StatePattern;
using BA_Studio.UnityLib.SingletonLocator;

namespace AngerStudio.HomingMeSoul.Core
{

    public class AppCore : MonoBehaviour
    {
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
        }

        public Dictionary<KeyCode, Color> activePlayers;
        List<Color> avaliableColors;

        public void ReceiveInput()
        {
            foreach (KeyCode vKey in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (SimpleInput.GetKeyDown(vKey))
                {
                    if (activePlayers.ContainsKey(vKey))
                    {
                        avaliableColors.Add(activePlayers[vKey]);
                        activePlayers.Remove(vKey);
                        m_View.RemovePlayerCard(vKey);
                    }
                    else
                    {
                        AddPlayer(vKey);
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