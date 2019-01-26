using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AngerStudio.HomingMeSoul.Game
{
        
    public class SupplyDrop : MonoBehaviour
    {
        public int typeIndex;

        public void Picked (int playerIndex)
        {
            GameCore.Instance.Picked(this);
        }
    }
}