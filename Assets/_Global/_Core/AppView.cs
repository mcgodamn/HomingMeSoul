using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;


namespace AngerStudio.HomingMeSoul.Core
{
    public class AppView : MonoBehaviour
    {
        [SerializeField]
        GameObject CardPrefab;

        List<(KeyCode,GameObject)> Cards = new List<(KeyCode, GameObject)>();

        public void RemovePlayerCard(KeyCode key)
        {
            foreach(var card in Cards)
            {
                if (card.Item1 == key)
                {
                    Destroy(card.Item2);
                    Cards.Remove(card);
                    break;
                }
            }

            ReorganizeView();
        }

        public void AddPlayerCard(KeyCode key, Color color)
        {
            //Initialize card
            GameObject card = Instantiate(CardPrefab, Vector3.zero, Quaternion.identity);


            Cards.Add((key,card));

            ReorganizeView();
        }

        void ReorganizeView()
        {

        }
    }
}