using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BA_Studio.StatePattern;
using BA_Studio.UnityLib.GameObjectPool;
using AngerStudio.HomingMeSoul.Core;
using BA_Studio.UnityLib.SingletonLocator;

namespace AngerStudio.HomingMeSoul.Game
{

    public class GameCore : MonoBehaviour
    {
        public static GameCore Instance { get => SingletonBehaviourLocator<GameCore>.Instance; }
        StateMachine<GameCore> stateMachine;
        public GameObject gravityZonePrefab;
        public GameObjectArrayReference gravityZones;
        public GameConfigReference config;
        public IntReference score, sp;
        public List<HashSet<SupplyDrop>> dropsInZones;

        GameObjectPool<SupplyDrop> dropsPool;

        Dictionary<SupplyDrop, int> zoneIndexMap = new Dictionary<SupplyDrop, int>();
        
        List<(Sprite, float depth, int, SupplyType)> pool = new List<(Sprite, float, int, SupplyType)>();

        float poolDepth = 0;

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
            Vector2[] positions = GetSpawnPosition(playersChoose.Count);
            foreach(var player in playersChoose)
            {
                //Initialize character
                GameObject temp = Instantiate(characterPrefab, positions[i], Quaternion.identity);
                Characters[i].Value = temp;
                CharacterProperty character = temp.GetComponent<CharacterProperty>();
                character.type = SupplyType.Food;
                character.collideLocation = homeTransform.gameObject;
                character.Ready = true;
                character.m_key = player.Key;
                character.Stamina = CharacterStamina[i];
                character.Stamina.Value = config.Value.staminaChargeNumber;
                character.faceLocation();

                Players.Add(player.Key, character);
                listPlayerInHome.Add(player.Key);

                i++;
            }
        }


        Vector2[] GetSpawnPosition(int playerNumber)
        {
            int radius = 1;
            Vector2[] returnVectors;

            switch(playerNumber)
            {
                case 1:
                    returnVectors = new Vector2[]{new Vector2(0,radius)};
                    break;
                case 2:
                    returnVectors = new Vector2[] { new Vector2(0, radius),new Vector2(0, -radius) };
                    break;
                case 3:
                    returnVectors = new Vector2[] {new Vector2(0, radius), new Vector2(radius*Mathf.Cos(210 * Mathf.Deg2Rad), radius*Mathf.Sin(210*Mathf.Deg2Rad)),new Vector2(radius * Mathf.Cos(330 * Mathf.Deg2Rad), radius * Mathf.Sin(330 * Mathf.Deg2Rad)) };
                    break;
                case 4:
                    returnVectors = new Vector2[] { new Vector2(0, radius),new Vector2(-radius,0),new Vector2(0, -radius),new Vector2(radius,0) };
                
                    break;
                case 5:
                    returnVectors = new Vector2[] { new Vector2(0, radius), new Vector2(radius * Mathf.Cos(162 * Mathf.Deg2Rad), radius * Mathf.Sin(162 * Mathf.Deg2Rad)), new Vector2(radius * Mathf.Cos(234 * Mathf.Deg2Rad), radius * Mathf.Sin(234 * Mathf.Deg2Rad)),new Vector2(radius * Mathf.Cos(306 * Mathf.Deg2Rad), radius * Mathf.Sin(306 * Mathf.Deg2Rad)),new Vector2(radius * Mathf.Cos(18 * Mathf.Deg2Rad), radius * Mathf.Sin(18 * Mathf.Deg2Rad)) };
                    break;
                case 6:
                    returnVectors = new Vector2[] { new Vector2(0, radius), new Vector2(radius * Mathf.Cos(150 * Mathf.Deg2Rad), radius * Mathf.Sin(150 * Mathf.Deg2Rad)), new Vector2(radius * Mathf.Cos(210 * Mathf.Deg2Rad), radius * Mathf.Sin(210 * Mathf.Deg2Rad)), new Vector2(radius * Mathf.Cos(270 * Mathf.Deg2Rad), radius * Mathf.Sin(270 * Mathf.Deg2Rad)), new Vector2(radius * Mathf.Cos(330 * Mathf.Deg2Rad), radius * Mathf.Sin(330 * Mathf.Deg2Rad)),new Vector2(radius * Mathf.Cos(30 * Mathf.Deg2Rad), radius * Mathf.Sin(30 * Mathf.Deg2Rad)) };
                   break;
                case 7:
                    returnVectors = new Vector2[] { new Vector2(0, radius), new Vector2(radius * Mathf.Cos(141 * Mathf.Deg2Rad), radius * Mathf.Sin(141 * Mathf.Deg2Rad)), new Vector2(radius * Mathf.Cos(192 * Mathf.Deg2Rad), radius * Mathf.Sin(192 * Mathf.Deg2Rad)), new Vector2(radius * Mathf.Cos(243 * Mathf.Deg2Rad), radius * Mathf.Sin(243 * Mathf.Deg2Rad)), new Vector2(radius * Mathf.Cos(294 * Mathf.Deg2Rad), radius * Mathf.Sin(294 * Mathf.Deg2Rad)), new Vector2(radius * Mathf.Cos(345 * Mathf.Deg2Rad), radius * Mathf.Sin(345 * Mathf.Deg2Rad)),new Vector2(radius * Mathf.Cos(39 * Mathf.Deg2Rad), radius * Mathf.Sin(39 * Mathf.Deg2Rad)) };
                    break;
                case 8:
                    returnVectors = new Vector2[] { new Vector2(0, radius), new Vector2(-radius, 0), new Vector2(0, -radius), new Vector2(radius, 0),new Vector2(radius * Mathf.Cos(135 * Mathf.Deg2Rad), radius * Mathf.Sin(135 * Mathf.Deg2Rad)), new Vector2(radius * Mathf.Cos(225 * Mathf.Deg2Rad), radius * Mathf.Sin(225 * Mathf.Deg2Rad)), new Vector2(radius * Mathf.Cos(315 * Mathf.Deg2Rad), radius * Mathf.Sin(315 * Mathf.Deg2Rad)), new Vector2(radius * Mathf.Cos(45 * Mathf.Deg2Rad), radius * Mathf.Sin(45 * Mathf.Deg2Rad)) };
                    break;
                default:
                    returnVectors = new Vector2[1];
                    break;
            }
            

            return returnVectors;
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
                {
                    Players[player].Stamina.Value = 0;
                }
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
                foreach (HashSet<SupplyDrop> h in dropsInZones)
                {
                    t += h.Count;
                }
                return t;

            }
        }

        void Awake ()
        {
            SingletonBehaviourLocator<GameCore>.Set(this);
            stateMachine = new StateMachine<GameCore>(this);

            Players = new Dictionary<KeyCode, CharacterProperty>();
            playersChoose = new Dictionary<KeyCode, Color>() {
                { KeyCode.Q, Color.black },
                { KeyCode.W, Color.black },
                { KeyCode.E, Color.black },
                { KeyCode.R, Color.black },
                { KeyCode.T, Color.black },
                { KeyCode.Y, Color.black },
                { KeyCode.U, Color.black },
                { KeyCode.I, Color.black }
            };

            stateMachine.ChangeState(new GamePreparing(stateMachine));

            SingletonBehaviourLocator<GameCore>.Set(this);

            dropsPool = new GameObjectPool<SupplyDrop>(AppCore.Instance.config.supplyDropPrefab, 20);

            GenerateSupplyPool();
        }

        void Update ()
        {
            stateMachine?.Update();
        }

        public void Picked (SupplyDrop s)
        {
            dropsInZones[zoneIndexMap[s]].Remove(s);
            zoneIndexMap.Remove(s);
            dropsPool.ReturnToPool(s);          
        }

        public void Prepare ()
        {
            dropsInZones = new List<HashSet<SupplyDrop>>();            

            gravityZones.Value = new GameObject[config.Value.gravityZoneSteps.Length - 1];
            GameObject g = GameObject.Instantiate(gravityZonePrefab);
            g.transform.localScale = Vector3.one * config.Value.gravityZoneSteps[0];

            for (int i = 1; i < config.Value.gravityZoneSteps.Length; i++)
            {
                gravityZones.Value[i - 1] = GameObject.Instantiate(gravityZonePrefab);
                gravityZones.Value[i - 1].transform.localScale = Vector3.one * config.Value.gravityZoneSteps[i];                    
            }
        }

        void GenerateSupplyPool ()
        {
            if (!config.Value.forbidBook1) pool.Add((AppCore.Instance.config.knowledgeSprites[0], config.Value.bookWeight * config.Value.supplyWeightLV1, 0, SupplyType.Book));
            if (!config.Value.forbidBook2) pool.Add((AppCore.Instance.config.knowledgeSprites[1], config.Value.bookWeight * config.Value.supplyWeightLV2, 1, SupplyType.Book));
            if (!config.Value.forbidBook3) pool.Add((AppCore.Instance.config.knowledgeSprites[2], config.Value.bookWeight * config.Value.supplyWeightLV3, 2, SupplyType.Book));
            if (!config.Value.forbidFood1) pool.Add((AppCore.Instance.config.foodSprites[0], config.Value.foodWeight * config.Value.supplyWeightLV1, 0, SupplyType.Food));
            if (!config.Value.forbidFood2) pool.Add((AppCore.Instance.config.foodSprites[1], config.Value.foodWeight * config.Value.supplyWeightLV2, 1, SupplyType.Food));
            if (!config.Value.forbidFood3) pool.Add((AppCore.Instance.config.foodSprites[2], config.Value.foodWeight * config.Value.supplyWeightLV3, 2, SupplyType.Food));
            if (!config.Value.forbidMoney1) pool.Add((AppCore.Instance.config.moneySprites[0], config.Value.moneyWeight * config.Value.supplyWeightLV1, 0, SupplyType.Money));
            if (!config.Value.forbidMoney2) pool.Add((AppCore.Instance.config.moneySprites[1], config.Value.moneyWeight * config.Value.supplyWeightLV2, 1, SupplyType.Money));
            if (!config.Value.forbidMoney3) pool.Add((AppCore.Instance.config.moneySprites[2], config.Value.moneyWeight * config.Value.supplyWeightLV3, 2, SupplyType.Money));
            poolDepth = pool.Sum(p => p.Item2);
        }

        public (SupplyType, int) GetRandomSupplySet ()
        {
            float r = Random.Range(0, poolDepth);
            for (int i = 0; i < pool.Count; i++)
            {
                r -= pool[i].depth;
                if (r < pool[i+1].depth && r > 0) return (pool[i].Item4, pool[i].Item3);
            }
            throw new System.Exception("Should not happen!");
        }

        public int SpawnSupplyInMostEmptyZone ((SupplyType type, int supplyLevel) s)
        {
            int emptyZoneIndex = gravityZones.Value.Length - 1, lastLeast = dropsInZones[emptyZoneIndex].Count;
            for (int i = emptyZoneIndex - 1; i > 0; i--)
            {
                if (dropsInZones[emptyZoneIndex].Count < lastLeast)
                {
                    lastLeast = dropsInZones[emptyZoneIndex].Count;
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
            
            GameObject t = null;
            SupplyDrop d = dropsPool.GetObjectFromPool(null);
            t = d.gameObject;
            d.type = s.type;
            d.level = s.supplyLevel;
            
            PlaceToOrbit(Random.Range(config.Value.gravityZoneSteps[zoneIndex - 1], config.Value.gravityZoneSteps[zoneIndex]),
            gravityZones.Value[zoneIndex].transform,
            t);

            if (dropsInZones[zoneIndex] == null) dropsInZones[zoneIndex] = new HashSet<SupplyDrop>();
            dropsInZones[zoneIndex].Add(d);

            zoneIndexMap.Add(d, zoneIndex);
        }

        ContactFilter2D filter = new ContactFilter2D () { useTriggers = true };

        Collider2D[] colCache;

        void PlaceToOrbit (float distance, Transform parentZone, GameObject t)
        {
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