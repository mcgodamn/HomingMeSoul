using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BA_Studio.StatePattern;
using BA_Studio.UnityLib.SingletonLocator;
using BA_Studio.UnityLib.GameObjectPool;
using BA_Studio.DataStructure;
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
        

        BiMap<int, HashSet<SupplyDrop>> pickUpInstances;

        public GameObject burstVFXPrefab;

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
            pickUpInstances = new BiMap<int, HashSet<SupplyDrop>>();

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


        public int GetLeastPickupTypeIndex ()
        {
            return pickUpInstances.First(i => i.Value.Count == pickUpInstances.Min(s => s.Value.Count)).Key;
        }

        public int SpawnSupplyInMostEmptyZone (int typeIndex)
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

            PlaceSupply(emptyZoneIndex, typeIndex);
            return emptyZoneIndex;
        }

        public int SpawnSupplyInRandomZone (int pickupType)
        {
            int t = Random.Range(1, gravityZones.Value.Length);
            PlaceSupply(t, pickupType);
            return t;
        }

        public void PlaceSupply (int zoneIndex, int pickupType)
        {
            
            GameObject t = null;
            SupplyDrop d = dropsPool.GetObjectFromPool(null);
            t = d.gameObject;
            d.typeIndex = pickupType;
            
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
            GameObject p = GameObject.Instantiate(burstVFXPrefab, t.transform.position, Quaternion.identity);
            MonoBehaviour.Destroy(p, 5f);
        }
    }
}