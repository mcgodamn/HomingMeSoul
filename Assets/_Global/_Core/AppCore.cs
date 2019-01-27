﻿using System.Linq;
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
        public SceneField toLoad;
        StateMachine<AppCore> stateMachine;
        public AppConfig config;
        public ResourceConfig resourceConfig;

        public Dictionary<KeyCode, PlayerProfile> activePlayers;
        public Transform titleT;
        public CanvasGroup titleGroup;
        public CSPanelController cSPanel;

        public TMPro.TextMeshProUGUI countDownText;

        void Awake ()
        {

            avaliableColors = new List<Color>(config.playerColorPool);

            avaliablePickup = new List<int>();

            avaliableSlots = new Stack<int>();
            for (int i = config.maxPlayers - 1; i >= 0; i--) avaliableSlots.Push(i);

            for (int i= 0; i < config.usablePickupSprites.Length; i++) avaliablePickup.Add(i);

            allowedKeys = new HashSet<KeyCode>((System.Enum.GetValues(typeof(KeyCode)) as KeyCode[]).Except(blocked));

            activePlayers = new Dictionary<KeyCode, PlayerProfile>(8);

            
            stateMachine = new StateMachine<AppCore>(this);
            stateMachine.ChangeState(new Awaking(stateMachine));

            SingletonBehaviourLocator<AppCore>.Set(this);

        }

        void Update ()
        {
            stateMachine?.Update();
        }
        internal List<Color> avaliableColors;

        internal List<int> avaliablePickup;
        
        internal Stack<int> avaliableSlots;

        public void TogglePlayer (KeyCode slot)
        {
            if (activePlayers.ContainsKey(slot)) RemovePlayer(slot);
            else AddPlayer(slot);
        }

        internal void AddPlayer (KeyCode slot)
        {
            if (avaliableColors.Count == 0 || activePlayers.Count >= config.maxPlayers || activePlayers.ContainsKey(slot)) return;
            int r1 = Random.Range(0, avaliableColors.Count), r2 = Random.Range(0, avaliablePickup.Count);
            activePlayers.Add(slot, new PlayerProfile(avaliableSlots.Pop(), avaliablePickup[r2], avaliableColors[r1]));
            //m_View.AddPlayerCard(slot, avaliableColors[r]);
            cSPanel.AddCard(slot);
            avaliableColors.RemoveAt(r1);         
            avaliablePickup.RemoveAt(r2);

        }

        internal void RemovePlayer (KeyCode slot)
        {
            if (!activePlayers.ContainsKey(slot)) return;

            cSPanel.RemoveCard(slot);
            avaliableColors.Add(activePlayers[slot].assignedColor);
            avaliablePickup.Add(activePlayers[slot].assginedPickupType);
            avaliableSlots.Push(activePlayers[slot].UsingPlayerSlot);
            activePlayers.Remove(slot);
        }

        [System.Serializable]
        public class PlayerProfile
        {

            public Color assignedColor;

            public int assginedPickupType;

            public int UsingPlayerSlot;

            public PlayerProfile( int usingPlayerSlot, int assginedPickupType, Color assignedColor)
            {
                this.assignedColor = assignedColor;
                this.assginedPickupType = assginedPickupType;
                UsingPlayerSlot = usingPlayerSlot;
            }
        }
    }
}