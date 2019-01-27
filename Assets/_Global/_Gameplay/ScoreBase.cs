using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using AngerStudio.HomingMeSoul.Core;
using BA_Studio.UnityLib.General.CommonEvents;

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

        void Update ()
        {
            if (Time.frameCount > lastConsumeFrame + GameCore.Instance.config.Value.updatesDelayBetweenProfits)
            {
                Profit (CheckCombos());
                lastConsumeFrame = Time.frameCount;
            }
        }

        ICollection<int> CheckCombos ()
        {
            List<int> t = new List<int>();

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