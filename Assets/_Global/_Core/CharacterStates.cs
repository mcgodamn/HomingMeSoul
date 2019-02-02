using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BA_Studio.StatePattern;

namespace AngerStudio.HomingMeSoul.Game
{
    public class AtHome : State<CharacterProperty>
    {
        int lastDeliverFrame;
        public AtHome(StateMachine<CharacterProperty> machine) : base(machine)
        {
        }

        public override void OnEntered ()
        {

            Context.Stamina.Value = GameCore.Instance.config.Value.staminaChargeNumber;

            Context.SetDry(false);
            GameCore.Instance.CharacterGoHome(Context);

            Context.onHit(GameCore.Instance.scoreBase.gameObject);
            Context.fitCircleCollider();
            foreach (CharacterProperty cp in Context.dragging.ToArray()) cp.StopBeingDragged(isBackToHome:true);
        }

        public override void Update()
        {           

            if (SimpleInput.GetKeyDown(Context.keyCode))
            {
                ChangeState(new Flying(StateMachine));
                return;
            }

            if (Time.frameCount > lastDeliverFrame + GameCore.Instance.config.Value.updatesDelayBetweenDeliver && Context.supplyPoint > 0)
            {
                GameCore.Instance.scoreBase.gameObject.GetComponent<ScoreBase>().DeliverPickups(Context.keyCode, 1);
                Context.scoreThisGame += 1;
                Context.supplyPoint -= 1;

                lastDeliverFrame = Time.frameCount;
            }

            GameCore.Instance.RotatePlayerInHome(Context);

        }
    }
    
    public class Flying : State<CharacterProperty>
    {

        public Flying (StateMachine<CharacterProperty> machine) : base(machine)
        {
        }

        public override void OnEntered ()
        {
            GameCore.Instance.Undock(Context, true);
        }


        Collider2D[] cache = new Collider2D[10];

        public override void Update()
        {
            foreach (Collider2D c in Context.collidingThisFrame)
            {
                if (c.gameObject.CompareTag("Pickup"))
                {
                    var supply = c.gameObject.GetComponent<SupplyDrop>();
                    if (supply.typeIndex == Context.typeIndex && !supply.Occupied)
                    {
                        Context.StartRotateOn(supply);
                        return;
                    }
                }                    
                else if (c.gameObject.CompareTag("Home"))
                {
                    Context.ReturnHome();
                }
                else if (c.gameObject.CompareTag("Player"))
                {
                    Context.TryStartDrag(c.gameObject);
                }
            }   

            GameCore.Instance.UpdateMomentum(Context);
            Context.ForwardVector += Context.gravityAccelator;
            Context.transform.position = Context.transform.position + Context.ForwardVector * Time.deltaTime;
            
            float decay = GameCore.Instance.config.Value.characterStatminaDecayRate * Time.deltaTime;
            Context.Stamina.Value -= decay;
            if (Context.Stamina.Value < 0)
            {
                Context.Stamina.Value = 0;
                ChangeState(new FlyingDepleted(StateMachine));
            }
        }
    }
    
    public class FlyingDepleted : State<CharacterProperty>
    {
        public FlyingDepleted (StateMachine<CharacterProperty> machine) : base(machine)
        {
        }

        public override void OnEntered ()
        {
            Context.SetDry(true);
        }

        public override void Update()
        {
            foreach (Collider2D c in Context.collidingThisFrame)
            {
                if (c.gameObject.CompareTag("Home"))
                {
                    Context.ReturnHome();
                }
            }   

            GameCore.Instance.UpdateMomentum(Context);
            Context.ForwardVector = Context.gravityAccelator;
            Context.transform.position = Context.transform.position + Context.ForwardVector * Time.deltaTime;
        }
    }

    public class RotatingOn : State<CharacterProperty>
    {
        Transform rotatingOn;
        public RotatingOn (StateMachine<CharacterProperty> machine, Transform target) : base(machine)
        {
            this.rotatingOn = target;
        }

        public override void Update()
        {
            if (SimpleInput.GetKeyDown(Context.keyCode))
            {
                ChangeState(new Flying(StateMachine));
                return;
            }

            GameCore.Instance.RotatePlayerOnLocation(Context, rotatingOn.position);
        }
    }

    public class SpinningAt : State<CharacterProperty>
    {
        Vector3 startPos;
        public SpinningAt (StateMachine<CharacterProperty> machine, Vector3 startPos) : base(machine)
        {
            this.startPos = startPos;
        }

        public override void Update()
        {
            if (SimpleInput.GetKeyDown(Context.keyCode))
            {
                ChangeState(new Flying(StateMachine));
                return;
            }
            GameCore.Instance.SpinCharacterAtPosition(Context, startPos);
        }
    }

    public class Dragged : State<CharacterProperty>
    {
        CharacterProperty dragger;
        public Dragged (StateMachine<CharacterProperty> machine, CharacterProperty dragger) : base(machine)
        {
            this.dragger = dragger;
        }

        public override void OnLeaving ()
        {
            dragger.dragging.Remove(Context);
        }

        public override void Update()
        {
            Context.transform.position = dragger.transform.position + Context.relativePositionToDragger;
        }
    }
}