using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AngerStudio.HomingMeSoul.Game
{
        
    public class SupplyDrop : MonoBehaviour
    {
        public int typeIndex;
        public bool Occupied = false;

        public void Picked (int playerIndex)
        {
            //TODO what to do with pindex
            GameCore.Instance.PickedBy(this, playerIndex);
        }
    }
}