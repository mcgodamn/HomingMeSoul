using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AngerStudio.HomingMeSoul.Game
{
        
    public class SupplyDrop : MonoBehaviour
    {
        public SupplyType type;

        [Range(0, 2)]
        public int level;

        public void Picked ()
        {
            GameCore.Instance.Picked(this);
        }
    }
}