using UnityEngine;

namespace AngerStudio.HomingMeSoul.Core
{
    [System.Serializable]
    public class AppConfig
    {
        public Color[] playerColorPool;

        public int maxPlayers;

        public float countDownSecs;

        public Sprite[] usablePickupSprites;

        public GameObject supplyDropPrefab;
    }
}