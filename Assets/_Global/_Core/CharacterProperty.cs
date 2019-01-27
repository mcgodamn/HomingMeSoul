using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AngerStudio.HomingMeSoul.Core;


namespace AngerStudio.HomingMeSoul.Game
{
    public class CharacterProperty : MonoBehaviour
    {
        GameObject m_collideLocation;
        public GameObject collideLocation
        {
            get{return m_collideLocation;}
            set{
                m_collideLocation = value;
                if (value != null)
                    lastLocationPosition = m_collideLocation.transform.position;
            }
        }
        public Vector3 lastLocationPosition;

        public GameObject normalCharacter, dryCharacter;
        public SpriteRenderer glowRenderer;

        public int supplyPoint = 0;

        public Vector3 ForwardVector;
        public Vector3 gravityAccelator;
        public Vector3 RotatePoint;
        public bool Ready = true;
        public bool canCollide = true;
        public FloatReference Stamina;

        public KeyCode m_key;
        public int playerIndex;

        public int typeIndex;

        public AudioSource audio;

        void Awake ()
        {
            this.audio = this.gameObject.AddComponent<AudioSource>();
        }

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

        void fitCircleCollider()
        {
            Vector2 dir = transform.up.normalized;
            transform.position = dir;
        }

        public void faceLocation()
        {
            Vector2 direction = transform.position - collideLocation.transform.position;
            transform.up = direction;
        }

        public void setIsDry(bool isDry)
        {
            normalCharacter.SetActive(!isDry);
            dryCharacter.SetActive(isDry);
        }

        public void setColor(Color color)
        {
            glowRenderer.color = color;
        }

        public void ReturnSupply()
        {
            var supply = collideLocation.GetComponent<SupplyDrop>();
            if(supply)
            {
                supply.Picked(playerIndex);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (!canCollide || collideLocation == other.gameObject) return;
            if (other.gameObject.CompareTag("Pickup"))
            {
                var supply = other.gameObject.GetComponent<SupplyDrop>();
                if (supply.typeIndex == typeIndex && !supply.Occupied)
                {
                    supplyPoint += 1;
                    supply.Occupied = true;
                    onHit(other);
                    GameCore.Instance.EnterLocation(m_key);
                }
            }
            else if (other.gameObject.CompareTag("Home"))
            {
                onHit(other);
                other.gameObject.GetComponent<ScoreBase>().DeliverPickups(m_key,supplyPoint);
                supplyPoint = 0;
                fitCircleCollider();
                GameCore.Instance.EnterHome(m_key);
            }
        }


    }
}