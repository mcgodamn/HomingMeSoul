using UnityEngine;

namespace AngerStudio.HomingMeSoul.Game
{

    [System.Serializable]
    public class GameConfig
    {
        public FloatReference characterStatminaDecayRate, supplyRevolutionSpeed;

        public float[] gravityZoneSteps = {5, 10, 15, 20, 25};

        public float supplyWeightLV1 = 0.7f, supplyWeightLV2 = 0.2f, supplyWeightLV3 = 0.1f;

        public float badGuySpawnChancePerMinute = 0.1f;

        public bool building_Exchange, building_Upgrade, building_Casino;

        public GameObject[] foodSupplyPrefabs, moneySupplyPrefabs, bookSupplyPrefabs;

        public float[] gravityZonesRevolutionSpeeds = { 3f, 5f, 8f, 12f, 16f, 20f, 25f, 30f, 35f, 40f, 48f, 56f };

    }
}