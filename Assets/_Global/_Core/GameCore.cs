using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BA_Studio.StatePattern;
using BA_Studio.UnityLib.SingletonLocator;
using BA_Studio.UnityLib.GameObjectPool;
using AngerStudio.HomingMeSoul.Core;

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
            stateMachine = new StateMachine<GameCore>(this);
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