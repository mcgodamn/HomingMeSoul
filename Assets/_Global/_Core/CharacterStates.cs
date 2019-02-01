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
            Context.SetDry(false);
            GameCore.Instance.CharacterGoHome(Context.m_key);

            Context.onHit(GameCore.Instance.scoreBase.gameObject);
            Context.fitCircleCollider();
        }

        public override void Update()
        {           

            if (Time.frameCount > lastDeliverFrame + GameCore.Instance.config.Value.updatesDelayBetweenDeliver)
            {
                GameCore.Instance.scoreBase.gameObject.GetComponent<ScoreBase>().DeliverPickups(Context.m_key, 1);
                Context.totalScore += 1;
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

        public override void Update()
        {
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
            Context.ForwardVector = Context.gravityAccelator;
        }
    }

    public class SpinningWith : State<CharacterProperty>
    {
        public SpinningWith (StateMachine<CharacterProperty> machine) : base(machine)
        {
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
        }
    }

    public class SpinningAt : State<CharacterProperty>
    {
        public SpinningAt (StateMachine<CharacterProperty> machine) : base(machine)
        {
        }

        public override void Update()
        {
            throw new System.NotImplementedException();
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