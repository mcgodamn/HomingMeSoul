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

        public List<SupplyDrop>[] storageVisuals;

        public IntCollectionEvent storageUpdate;

        public ProfitFX fX;

        void Awake ()
        {
            innerPickupPool = new GameObjectPool<SupplyDrop>(innerScoreVisualPrefab);
            innerScores = new List<SupplyDrop>();

            storageVisuals = new List<SupplyDrop>[AppCore.Instance.orderedPlayers.Count];
            for (int i = 0; i < storageVisuals.Length; i++) storageVisuals[i] = new List<SupplyDrop>();
        }

        public void DoUpdate ()
        {       
            foreach (SupplyDrop s in innerScores)
            {
                s.GetComponent<Rigidbody2D>().AddForce(Vector2.left * Random.Range(-0.14f, 0.14f) + Vector2.up * Random.Range(-0.14f, 0.14f));
            }   

            if (Time.frameCount > lastConsumeFrame + GameCore.Instance.config.Value.updatesDelayBetweenProfits)
            {
                ProfitStart (CheckCombos());
                lastConsumeFrame = Time.frameCount;
            }
        }

        List<int> t = new List<int>();
        
        int[] CheckCombos ()
        {
            if (storage == null || storage.Length != AppCore.Instance.activePlayers.Count) 
                storage = new int[AppCore.Instance.activePlayers.Count];
            
            t.Clear();

            for (int i = 0; i < AppCore.Instance.activePlayers.Count; i++)
            {
                if (storage[i] > 0) t.Add(i);
            }

            if (t.Count < 2) return null;
            else return t.ToArray();
        }

        void ProfitStart (int[] comboTypes)
        {
            if (comboTypes == null) return;

            ProfitFX(comboTypes);

        }

        void ProfitFX (int[] comboTypes)
        {
            Vector3[] points = new Vector3[comboTypes.Length];
            for (int i = 0; i < points.Length; i++)
            {
                points[i] = storageVisuals[(comboTypes.ToArray())[i]].Last().transform.position;
            }
            foreach (int i in comboTypes)
            {
                storage[i] -= 1;
            }
            fX.StartLine(points, true, null, () => ProfitFinish(comboTypes));
        }

        void ProfitFinish (int[] comboTypes)
        {
            foreach (int i in comboTypes)
            {
                SupplyDrop t = storageVisuals[i].Last();
                innerScores.Remove(t);
                storageVisuals[i].Remove(t);
                t.gameObject.SetActive(false);
                innerPickupPool.ReturnToPool(t);
            }

            hiddenStack += GameCore.Instance.config.Value.rewardLevel[comboTypes.Length - 2];
            scoreR.Value += Mathf.FloorToInt(hiddenStack);
            scored?.Invoke(Mathf.FloorToInt(hiddenStack));
            hiddenStack = hiddenStack % 1f;
            comboProfit?.Invoke(comboTypes);        
        }

        public void DeliverPickups (KeyCode key, int count)
        {
            int slot = AppCore.Instance.activePlayers[key].UsingPlayerSlot;
            storage[slot] += count;
            for (int i = 0; i < count; i++)
            {
                innerScores.Add(innerPickupPool.GetObjectFromPool(this.inner.transform));
                innerScores.Last().gameObject.SetActive(true);
                innerScores.Last().SetType(slot);
                storageVisuals[slot].Add(innerScores.Last());
            }
        }


    }
}