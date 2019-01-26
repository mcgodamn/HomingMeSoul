using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BA_Studio.StatePattern;
using BA_Studio.UnityLib.SingletonLocator;
using AngerStudio.HomingMeSoul.Game;

namespace AngerStudio.HomingMeSoul.Core
{

    public class AppCore : MonoBehaviour
    {
        public static AppCore Instance { get => SingletonBehaviourLocator<AppCore>.Instance; }
        HashSet<KeyCode> blocked = new HashSet<KeyCode> 
        {
            KeyCode.LeftAlt, KeyCode.RightAlt,
            KeyCode.LeftWindows, KeyCode.RightWindows,
            KeyCode.Escape,
            KeyCode.F1, KeyCode.F2, KeyCode.F3, KeyCode.F4, KeyCode.F5, KeyCode.F6,
            KeyCode.F7, KeyCode.F8, KeyCode.F9, KeyCode.F10, KeyCode.F11, KeyCode.F12,
            KeyCode.ScrollLock, KeyCode.Print, KeyCode.Break
        };

        internal HashSet<KeyCode> allowedKeys;

        [SerializeField]
        internal AppView m_View;
        

        public SceneField toLoad;


        StateMachine<AppCore> stateMachine;

        public Dictionary<KeyCode, SupplyType> playerTypeMap;

        public AppConfig config;

        void Awake ()
        {
            stateMachine = new StateMachine<AppCore>(this);
            SingletonBehaviourLocator<AppCore>.Set(this);

            avaliableColors = new List<Color>(config.playerColorPool);

            allowedKeys = new HashSet<KeyCode>((System.Enum.GetValues(typeof(KeyCode)) as KeyCode[]).Except(blocked));

        }

        public Dictionary<KeyCode, Color> activePlayers;
        internal List<Color> avaliableColors;

        internal void AddPlayer (KeyCode slot)
        {
            if (avaliableColors.Count == 0 || activePlayers.Count >= config.maxPlayers) return;
            int r = Random.Range(0, avaliableColors.Count);
            activePlayers.Add(slot,avaliableColors[r]);
            m_View.AddPlayerCard(slot, avaliableColors[r]);
            avaliableColors.RemoveAt(r);

        }
    }
}