using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AngerStudio.HomingMeSoul.Core;
using BA_Studio.StatePattern;

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

        public TrailRenderer trailRenderer;

        public int supplyPoint = 0;
        public int totalScore = 0;

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

        StateMachine<CharacterProperty> stateMachine;
        internal List<CharacterProperty> dragging = new List<CharacterProperty>();

        void Awake ()
        {
            this.audio = this.gameObject.AddComponent<AudioSource>();
            stateMachine = new StateMachine<CharacterProperty>(this);
        }


        public void PlayerUpdate()
        {
            stateMachine?.Update();
        }

        public float Speed
        {
            get => GameCore.Instance.config.Value.speedMultiplier;
        }

        internal void fitCircleCollider()
        {
            Vector2 dir = transform.up.normalized;
            transform.position = dir;
        }

        public void faceLocation()
        {
            Vector2 direction = transform.position - collideLocation.transform.position;
            transform.up = direction;
        }

        public bool isDry;
        public void SetDry(bool isDry)
        {
            this.isDry = isDry;
            normalCharacter.SetActive(!isDry);
            dryCharacter.SetActive(isDry);
        }

        public void setColor(Color color)
        {
            glowRenderer.color = color;
            trailRenderer.startColor = color;
            Color t = new Color(color.r, color.g, color.b, 0);
            trailRenderer.endColor = t;
        }

        public void ReturnSupply()
        {
            var supply = collideLocation.GetComponent<SupplyDrop>();
            if(supply)
            {
                supply.Picked(playerIndex);
            }
        }

        public void ReturnHome()
        {
            stateMachine.ChangeState(new AtHome(stateMachine));
        }

        CharacterProperty dragger;
        internal Vector3 relativePositionToDragger;
        bool IsDragging { get => dragging.Count > 0; }
        public void setDragger(CharacterProperty dragger)
        {
            this.dragger = dragger;
            relativePositionToDragger = transform.position - dragger.transform.position;
        }

        public void StartDrag (CharacterProperty c)
        {
            dragging.Add(c);

            c.StartBeingDragged(this);
        }

        public void StartBeingDragged (CharacterProperty c)
        {
            stateMachine.ChangeState(new Dragged(stateMachine, c));
        }

        internal void onHit(GameObject other)
        {
            collideLocation = other;
            faceLocation();
            Ready = true;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (collideLocation == other.gameObject || IsDragging) return;
            if (other.gameObject.CompareTag("Pickup"))
            {
                var supply = other.gameObject.GetComponent<SupplyDrop>();
                if (supply.typeIndex == typeIndex && !supply.Occupied)
                {
                    supplyPoint += 1;
                    supply.Occupied = true;
                    onHit(other.gameObject);
                    GameCore.Instance.EnterLocation(m_key);
                }
            }
            else if (other.gameObject.CompareTag("Home"))
            {
                ReturnHome();
                // foreach(var homie in dryHomies)
                // {
                //     homie.ReturnHome();
                // }
                // dryHomies.Clear();
            }
            else if (other.gameObject.CompareTag("Player"))
            {
                var homie = other.gameObject.GetComponent<CharacterProperty>();
                if (homie.stateMachine.CurrentState is FlyingDepleted)
                {
                    StartDrag(homie);
                }
            }
        }
    }
}