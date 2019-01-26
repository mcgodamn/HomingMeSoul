using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BA_Studio.StatePattern;
using BA_Studio.UnityLib.SingletonLocator;

namespace AngerStudio.HomingMeSoul.Game
{

    public class GameCore : MonoBehaviour
    {
        StateMachine<GameCore> stateMachine;
        public GameObject gravityZonePrefab;
        public GameObjectArrayReference gravityZones;
        public GameConfigReference config;
        public IntReference Score, SP;
        public int[] suppliesCountOfZones;

        public GameObject characterPrefab;
        public GameObjectReference[] Characters;
        public FloatReference[] CharacterStamina;

        public static GameCore Instance { get => SingletonBehaviourLocator<GameCore>.Instance; }

        Dictionary<KeyCode, CharacterProperty> Players;
        public Dictionary<KeyCode, Color> playersChoose;

        public Transform homeTransform;

        public void CreaterPlayers()
        {
            int i = 0;
            foreach(var player in playersChoose)
            {
                //Initialize character
                GameObject temp = Instantiate(characterPrefab, new Vector3(0,1,0), Quaternion.identity);
                Characters[i].Value = temp;
                CharacterProperty character = temp.GetComponent<CharacterProperty>();
                character.type = SupplyType.Food;
                character.Ready = true;
                character.m_key = player.Key;
                character.Stamina = CharacterStamina[i];
                character.Stamina.Value = config.Value.staminaChargeNumber;
                Players.Add(player.Key, character);

                listPlayerInHome.Add(player.Key);

                i++;
            }
        }


        void GetSpawnPosition(int playerNumber)
        {

        }

        public void ReceieveInput()
        {
            foreach(var key in Players.Keys)
            {
                if (SimpleInput.GetKeyDown(key))
                {
                    if (Players[key].Ready)
                    {
                        Shoot(key);
                    }
                }
            }
        }

        public void EnterHome(KeyCode key)
        {
            if (!listPlayerMoving.Contains(key)) return;

            listPlayerMoving.Remove(key);
            listPlayerInHome.Add(key);
        }

        public void EnterLocation(KeyCode key)
        {
            if (!listPlayerMoving.Contains(key)) return;

            listPlayerMoving.Remove(key);
            listPlayerOnLocation.Add(key);
        }

        public void DecentStamina()
        {
            float decentValue = config.Value.characterStatminaDecayRate * Time.deltaTime;
            foreach(var player in Players.Keys)
            {
                if (Players[player].Stamina.Value > 0)
                    Players[player].Stamina.Value -= decentValue;
                else
                    Players[player].Stamina.Value = 0;
            }
        }

        void Shoot(KeyCode key)
        {
            Vector3 midPosition = Vector3.zero;
            if (listPlayerOnLocation.Contains(key))
            {
                midPosition = Players[key].collideLocation.transform.position;
                listPlayerOnLocation.Remove(key);
            }
            if (listPlayerInHome.Contains(key))
            {
                midPosition = homeTransform.position;
                listPlayerInHome.Remove(key);
            }

            Players[key].collideLocation = null;

            Players[key].ForwardVector = (Players[key].transform.position - midPosition).normalized * Players[key].GetSpeed();

            Players[key].Ready = false;
            Players[key].canCollide = true;
            listPlayerMoving.Add(key);
        }

        List<KeyCode> listPlayerMoving = new List<KeyCode>();
        public void PlayerMove()
        {
            foreach(var player in listPlayerMoving)
            {
                // float gravityMagnitude = config.Value.gravityMultiplier / (Players[player].Stamina + 1);
                Vector3 vGravity = Gravity.getGravity(Players[player].transform.position,config.Value.gravityMultiplier);
                
                Players[player].gravityAccelator = vGravity;
                Players[player].PlayerMove();

                BorderDectection(player);
            }
        }

        void BorderDectection(KeyCode player)
        {

            if (Vector2.Distance(Players[player].transform.position, homeTransform.position) >= config.Value.worldRidius)
            {
                Vector3 inVector = Players[player].ForwardVector.normalized;
                Vector3 normal = (Players[player].transform.position - homeTransform.position).normalized;
                Vector2 outVector = -1 * (Vector2.Dot(inVector, normal) * normal * 2 - inVector);
                Players[player].ForwardVector = outVector.normalized * Players[player].ForwardVector.magnitude;
            }
        }

        List<KeyCode> listPlayerOnLocation = new List<KeyCode>();
        public void RotatePlayerOnLocation()
        {
            foreach (var player in listPlayerOnLocation)
            {
                Players[player].transform.position += Players[player].collideLocation.transform.position - Players[player].lastLocationPosition;

                Players[player].lastLocationPosition = Players[player].collideLocation.transform.position;

                Players[player].transform.RotateAround(Players[player].collideLocation.transform.position, Vector3.forward, 1f);
            }
        }

        List<KeyCode> listPlayerInHome = new List<KeyCode>();
        public void RotatePlayerInHome()
        {
            foreach(var player in listPlayerInHome)
            {
                Players[player].Stamina.Value = config.Value.staminaChargeNumber;
                Players[player].transform.RotateAround(homeTransform.position,Vector3.forward,1f);
            }
        }

        float GetHomeRadius(int peopleNumber)
        {
            if (peopleNumber <= 3) return 0.1f;

            float r = 0.05f;
            for (int n = 3; n < peopleNumber; n++)
                r = r + r * 0.1f * n;

            return r;
        }
        
        public int SuppliesSum
        {
            get
            {
                int t = 0;
                foreach (int i in suppliesCountOfZones)
                {
                    t += i;
                }
                return t;

            }
        }

        void Awake ()
        {
            SingletonBehaviourLocator<GameCore>.Set(this);
            stateMachine = new StateMachine<GameCore>(this);

            Players = new Dictionary<KeyCode, CharacterProperty>();
            playersChoose = new Dictionary<KeyCode, Color>() { { KeyCode.Q, Color.black } };

            stateMachine.ChangeState(new GamePreparing(stateMachine));
        }

        void Update ()
        {
            stateMachine?.Update();
        }

        public void Prepare ()
        {
            suppliesCountOfZones = new int[config.Value.gravityZoneSteps.Length - 1];

            gravityZones.Value = new GameObject[config.Value.gravityZoneSteps.Length - 1];
            GameObject g = GameObject.Instantiate(gravityZonePrefab);
            g.transform.localScale = Vector3.one * config.Value.gravityZoneSteps[0];

            for (int i = 1; i < config.Value.gravityZoneSteps.Length; i++)
            {
                gravityZones.Value[i - 1] = GameObject.Instantiate(gravityZonePrefab);
                gravityZones.Value[i - 1].transform.localScale = Vector3.one * config.Value.gravityZoneSteps[i];                    
            }
        }

        public (SupplyType, int) GetRandomSupplySet ()
        {
            float r = Random.Range(0f, 1f);
            if (r < config.Value.supplyWeightLV3) return ((SupplyType) Random.Range(0, 3), 2);
            else if (r < config.Value.supplyWeightLV2) return ((SupplyType) Random.Range(0, 3), 1);
            else return ((SupplyType) Random.Range(0, 3), 1);
        }

        public int SpawnSupplyInMostEmptyZone ((SupplyType type, int supplyLevel) s)
        {
            int emptyZoneIndex = gravityZones.Value.Length - 1, lastLeast = suppliesCountOfZones[emptyZoneIndex];
            for (int i = emptyZoneIndex - 1; i > 0; i--)
            {
                if (suppliesCountOfZones[i] < lastLeast)
                {
                    lastLeast = suppliesCountOfZones[i];
                    emptyZoneIndex = i;
                }                
            }

            PlaceSupply(emptyZoneIndex, s);
            return emptyZoneIndex;
        }

        public int SpawnSupplyInRandomZone ((SupplyType type, int supplyLevel) s)
        {
            int t = Random.Range(1, gravityZones.Value.Length);
            PlaceSupply(t, s);
            return t;
        }

        public void PlaceSupply (int zoneIndex, (SupplyType type, int supplyLevel) s)
        {
            if (Random.Range(0f, Mathf.Pow(1 + config.Value.rimRewardFactor, zoneIndex)) > 1 && s.supplyLevel < 2) s.supplyLevel += 1; 
            PlaceSupply(Random.Range(config.Value.gravityZoneSteps[zoneIndex - 1], config.Value.gravityZoneSteps[zoneIndex]),
            gravityZones.Value[zoneIndex].transform,
            s);
            suppliesCountOfZones[zoneIndex] += 1;
        }

        ContactFilter2D filter = new ContactFilter2D () { useTriggers = true };

        Collider2D[] colCache;

        public void PlaceSupply (float distance, Transform parentZone, (SupplyType type, int supplyLevel) s)
        {
            GameObject t = null;
            switch (s.type)
            {
                case SupplyType.Money:
                    t = GameObject.Instantiate(config.Value.moneySupplyPrefabs[s.supplyLevel]);
                    break;
                case SupplyType.Book:
                    t = GameObject.Instantiate(config.Value.bookSupplyPrefabs[s.supplyLevel]);
                    break;
                case SupplyType.Food:
                    t = GameObject.Instantiate(config.Value.foodSupplyPrefabs[s.supplyLevel]);
                    break;
            }
            
            t.transform.SetParent(parentZone);
            t.transform.localPosition = Vector3.zero;
            distance = distance / 2f; // Steps is scale value for zones.
            t.transform.position = parentZone.position + Quaternion.Euler(0, 0, Random.Range(0f, 359.9f)) * Vector2.left * distance;
            for (int i = 0; i < 3; i++)
            {
                if (Physics2D.OverlapCircle(t.transform.position, config.Value.densityBalancingDistance, filter, colCache) > 0) 
                    t.transform.position = parentZone.position + Quaternion.Euler(0, 0, Random.Range(0f, 359.9f)) * Vector2.left * distance;
                else break;
            }
        }
    }
}