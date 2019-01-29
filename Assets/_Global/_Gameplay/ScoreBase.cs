using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AngerStudio.HomingMeSoul.Core;
using BA_Studio.UnityLib.General.CommonEvents;
using BA_Studio.UnityLib.GameObjectPool;

namespace AngerStudio.HomingMeSoul.Game
{

    public class ScoreBase : MonoBehaviour
    {
        int lastConsumeFrame;

        public int[] storage;

        public IntReference scoreR;

        float hiddenStack = 0;

        public IntCollectionEvent comboProfit;
        public IntEvent scored;

        public GameObject inner, innerScoreVisualPrefab;

        GameObjectPool<SupplyDrop> innerPickupPool;

        public List<SupplyDrop> innerScores;

        int currentMainInnerType;

        void Awake ()
        {
            innerPickupPool = new GameObjectPool<SupplyDrop>(innerScoreVisualPrefab);
            innerScores = new List<SupplyDrop>();

        }

        void Update ()
        {
            currentMainInnerType = storage.MaxIndex();
            if (storage.All(s => s == 0)) currentMainInnerType = -1;
            if (currentMainInnerType != -1)
            {
                if (innerScores.Count < storage[currentMainInnerType]) 
                {
                    innerScores.Add(innerPickupPool.GetObjectFromPool(this.inner.transform));
                    innerScores.Last().gameObject.SetActive(true);
                }
                if (innerScores.Count > storage[currentMainInnerType])
                {
                    SupplyDrop t = innerScores.Last();
                    innerScores.Remove(t);
                    t.gameObject.SetActive(false);
                    innerPickupPool.ReturnToPool(t);
                }
                foreach (SupplyDrop s in innerScores)
                {
                    s.SetType(currentMainInnerType);
                    s.GetComponent<Rigidbody2D>().AddForce(Vector2.one * Random.Range(0f, 0.025f));
                }                
            }

            if (Time.frameCount > lastConsumeFrame + GameCore.Instance.config.Value.updatesDelayBetweenProfits)
            {
                Profit (CheckCombos());
                lastConsumeFrame = Time.frameCount;
            }
        }

        List<int> t = new List<int>();
        
        ICollection<int> CheckCombos ()
        {
            if (storage == null || storage.Length != AppCore.Instance.activePlayers.Count) 
                storage = new int[AppCore.Instance.activePlayers.Count];
            
            t.Clear();

            for (int i = 0; i < AppCore.Instance.activePlayers.Count; i++)
            {
                if (storage[i] > 0) t.Add(i);
            }
            return t;
        }

        void Profit (ICollection<int> counted)
        {
            if (counted.Count < 2) return;

            foreach (int i in counted)
            {
                storage[i] -= 1;
            }
            hiddenStack += GameCore.Instance.config.Value.rewardLevel[counted.Count - 1];
            scoreR.Value += Mathf.FloorToInt(hiddenStack);
            scored?.Invoke(Mathf.FloorToInt(hiddenStack));
            hiddenStack = hiddenStack % 1;
            comboProfit?.Invoke(counted);
            
        }

        public void DeliverPickups (KeyCode key, int count)
        {
            storage[AppCore.Instance.activePlayers[key].UsingPlayerSlot] += count;
        }


    }
}