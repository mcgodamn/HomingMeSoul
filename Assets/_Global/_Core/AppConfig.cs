using UnityEngine;

namespace AngerStudio.HomingMeSoul.Core
{
    [System.Serializable]
    public class AppConfig
    {
        public Color[] playerColorPool;

        public int maxPlayers;

        public Sprite[] usablePickupSprites;

        public GameObject supplyDropPrefab;
    }
}