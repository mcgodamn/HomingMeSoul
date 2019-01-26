﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BA_Studio.StatePattern;
using BA_Studio.UnityLib.SingletonLocator;

namespace AngerStudio.HomingMeSoul.Game
{

    public class GameCore : MonoBehaviour
    {
        public GameObject gravityZonePrefab;
        public GameObjectArrayReference gravityZones;
        public GameConfigReference config;
        public int[] suppliesCountOfZones;

        public GameObject characterPrefab;
        public GameObjectReference[] Characters;
        public FloatReference[] CharacterStamina;

        public static GameCore Instance { get => SingletonBehaviourLocator<GameCore>.Instance; }

        public Dictionary<KeyCode, Character> Players;
        public Dictionary<KeyCode, Color> playersChoose; 

        StateMachine<GameCore> stateMachine;

        void Awake()
        {
            SingletonBehaviourLocator<GameCore>.Set(this);
            stateMachine = new StateMachine<GameCore>(this);
        }

        void Update()
        {
            stateMachine.Update();
        }

        public void CreaterPlayers()
        {
            Players = new Dictionary<KeyCode, Character>();

            int i = 0;
            foreach(var player in playersChoose)
            {
                //Initialize character
                GameObject temp = Instantiate(characterPrefab, Vector3.zero, Quaternion.identity);
                Characters[i++].Value = temp;
                Players.Add(player.Key, temp.GetComponent<Character>());
            }

        }

        public void ReceieveInput()
        {
            foreach(var key in Players.Keys)
            {
                if (SimpleInput.GetKeyDown(key))
                {
                    if (Players[key].Ready)
                        Players[key].Shoot();
                }
            }
        }

        public void PlayerMove()
        {

        }

        public void RotatePlayerInHome()
        {

        }

        public void Prepare ()
        {
            suppliesCountOfZones = new int[config.Value.gravityZoneSteps.Length - 1];

            gravityZones.Value = new GameObject[config.Value.gravityZoneSteps.Length];
            gravityZones.Value[0] = GameObject.Instantiate(gravityZonePrefab);
            gravityZones.Value[0].transform.localScale = Vector3.one * config.Value.gravityZoneSteps[0];

            for (int i = 1; i < config.Value.gravityZoneSteps.Length; i++)
            {
                gravityZones.Value[i] = GameObject.Instantiate(gravityZonePrefab);
                gravityZones.Value[i].transform.localScale = Vector3.one * config.Value.gravityZoneSteps[i];                    
            }
        }

        public int SpawnSupplyInMostEmptyZone (SupplyType type, int supplyLevel)
        {
            int emptyZoneIndex = 0, lastLeast = -1;
            for (int i = 0; i < gravityZones.Value.Length; i++)
            {
                if (suppliesCountOfZones[i] > lastLeast)
                {
                    lastLeast = suppliesCountOfZones[i];
                    emptyZoneIndex = i;
                }                
            }

            PlaceSupply(emptyZoneIndex, type, supplyLevel);
            return emptyZoneIndex;
        }

        public int SpawnSupplyInRandomZone (SupplyType type, int supplyLevel)
        {
            int t = Random.Range(0, gravityZones.Value.Length);
            PlaceSupply(t, gravityZones.Value[t].transform, type, supplyLevel);
            return t;
        }

        public void PlaceSupply (int zoneIndex, SupplyType type, int supplyLevel)
        {
            PlaceSupply(Random.Range(config.Value.gravityZoneSteps[zoneIndex - 1], config.Value.gravityZoneSteps[zoneIndex]), gravityZones.Value[zoneIndex].transform, type, supplyLevel);
        }

        public void PlaceSupply (float distance, Transform parentZone, SupplyType type, int supplyLevel)
        {
            GameObject t = null;
            switch (type)
            {
                case SupplyType.Money:
                    t = GameObject.Instantiate(config.Value.moneySupplyPrefabs[supplyLevel]);
                    break;
                case SupplyType.Book:
                    t = GameObject.Instantiate(config.Value.bookSupplyPrefabs[supplyLevel]);
                    break;
                case SupplyType.Food:
                    t = GameObject.Instantiate(config.Value.foodSupplyPrefabs[supplyLevel]);
                    break;
            }
            
            t.transform.SetParent(parentZone);
            t.transform.localPosition = Vector3.zero;
            t.transform.position = parentZone.position + Quaternion.Euler(0, 0, Random.Range(0f, 359.9f)) * Vector2.left * distance;
        }
    }
}