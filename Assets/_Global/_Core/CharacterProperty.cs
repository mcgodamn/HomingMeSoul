using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace AngerStudio.HomingMeSoul.Game
{
    public class CharacterProperty : MonoBehaviour
    {
        GameObject m_collideLocation;
        public GameObject collideLocation
        {
            get{return m_collideLocation;}
            set{
                if (value == null && m_collideLocation != null)
                    if (m_collideLocation.GetComponent<SpriteRenderer>().sortingLayerName == "Supplies")
                        Destroy(m_collideLocation);
                m_collideLocation = value;
                if (value != null)
                    lastLocationPosition = m_collideLocation.transform.position;
            }
        }
        public Vector3 lastLocationPosition;

        public GameObject normalCharacter, dryCharacter;
        public SpriteRenderer glowRenderer;

        public int supplyPoint;

        public Vector3 ForwardVector;
        public Vector3 gravityAccelator;
        public Vector3 RotatePoint;
        public bool Ready = true;
        public bool canCollide = true;
        public FloatReference Stamina;

        public KeyCode m_key;

        public int typeIndex;

        public void PlayerMove()
        {
            ForwardVector += gravityAccelator;
            transform.position = transform.position + ForwardVector * Time.deltaTime;
            if (Stamina <= 0) ForwardVector = gravityAccelator;
        }

        public float GetSpeed()
        {
            // return Stamina.Value * GameCore.Instance.config.Value.speedMultiplier;
            return GameCore.Instance.config.Value.speedMultiplier;
        }

        void onHit(Collider2D other)
        {
            collideLocation = other.gameObject;
            faceLocation();
            canCollide = false;
            Ready = true;
        }

        public void faceLocation()
        {
            Vector2 direction = transform.position - collideLocation.transform.position;
            transform.up = direction;
        }

        public void setColor(Color color)
        {
            glowRenderer.color = color;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!canCollide || collideLocation == other.gameObject) return;
            if (other.gameObject.GetComponent<SpriteRenderer>().sortingLayerName == "Supplies")
            {
                if (other.gameObject.GetComponent<SupplyDrop>().typeIndex == typeIndex)
                {
                    onHit(other);
                    GameCore.Instance.EnterLocation(m_key);
                }
            }

            else if (other.gameObject.GetComponent<SpriteRenderer>().sortingLayerName == "Home")
            {
                onHit(other);
                GameCore.Instance.EnterHome(m_key);
            }
        }


    }
}