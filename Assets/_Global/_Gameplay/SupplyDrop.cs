using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AngerStudio.HomingMeSoul.Game
{
        
    public class SupplyDrop : MonoBehaviour
    {
        public int typeIndex;
        public bool Occupied = false;
        public BoxCollider2D m_collider;

        void Awake()
        {
            m_collider = GetComponent<BoxCollider2D>();            
        }

        public void Picked (int playerIndex)
        {
            //TODO what to do with pindex
            GameCore.Instance.Picked(this);
        }
    }
}