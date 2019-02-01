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
        public GameObject dockedAt
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
        public int scoreThisGame = 0;

        public Vector3 ForwardVector;
        public Vector3 gravityAccelator;
        public Vector3 RotatePoint;
        public FloatReference Stamina;

        public KeyCode keyCode;
        public int playerIndex;

        public int typeIndex;

        public AudioSource audio;

        internal StateMachine<CharacterProperty> stateMachine;
        internal List<CharacterProperty> dragging = new List<CharacterProperty>();

        internal Collider2D collider;

        void Awake ()
        {
            this.audio = this.gameObject.AddComponent<AudioSource>();
            stateMachine = new StateMachine<CharacterProperty>(this);
            //stateMachine.debugLogOutput += (s) => Debug.Log(s);
            collider = this.GetComponent<Collider2D>();
            cFilterOthers = new ContactFilter2D ();
            cFilterOthers.useTriggers = true;
            cFilterOthers.useLayerMask = true;
            cFilterOthers.SetLayerMask(LayerMask.GetMask("GameInteraction"));
        }


        public void PlayerInitOnGameStart ()
        {
            stateMachine?.ChangeState(new AtHome(stateMachine));
        }

        public void DoUpdate()
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
            Vector2 direction = transform.position - dockedAt.transform.position;
            transform.up = direction;
        }

        public bool isDry;
        public void SetDry(bool isDry)
        {
            this.isDry = isDry;
            normalCharacter.SetActive(!isDry);
            dryCharacter.SetActive(isDry);
        }

        public void SetColor(Color color)
        {
            glowRenderer.color = color;
            trailRenderer.startColor = color;
            Color t = new Color(color.r, color.g, color.b, 0);
            trailRenderer.endColor = t;
        }

        public void TryCollectPickup()
        {
            var supply = dockedAt.GetComponent<SupplyDrop>();
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

        public void TryStartDrag (GameObject g)
        {
            CharacterProperty c = g.GetComponent<CharacterProperty>();
            if (c == null) return;
            if (c.stateMachine.CurrentState is FlyingDepleted)
            {
                dragging.Add(c);

                c.StartBeingDragged(this);
            }
        }

        public void StartRotateOn (SupplyDrop pickup)
        {
            stateMachine?.ChangeState(new RotatingOn(stateMachine, pickup.transform));
            supplyPoint += 1;
            pickup.Occupied = true;
            onHit(pickup.gameObject);
            GameCore.Instance.EnterLocation(this);
        }

        public void StartBeingDragged (CharacterProperty c)
        {
            stateMachine.ChangeState(new Dragged(stateMachine, c));
        }

        public void StopBeingDragged ()
        {
            stateMachine.ChangeState(new FlyingDepleted(stateMachine));
        }

        internal void onHit(GameObject other)
        {
            dockedAt = other;
            faceLocation();
        }

        private void OnTriggerEnter2D (Collider2D other)
        {
            colliding = colliding ?? new List<Collider2D>();
            collidingThisFrame = collidingThisFrame ?? new List<Collider2D>();
            collidingThisFrame.Add(other);
            colliding.Add(other);
        }

        private void OnTriggerExit2D (Collider2D other)
        {
            colliding = colliding ?? new List<Collider2D>();
            collidingThisFrame = collidingThisFrame ?? new List<Collider2D>();
            colliding.Remove(other);
        }

        void LateUpdate ()
        {
            collidingThisFrame?.Clear();
        }

        public ContactFilter2D cFilterOthers;

        internal List<Collider2D> colliding, collidingThisFrame;
    }
}