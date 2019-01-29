using System.Collections;
using System.Collections.Generic;
using BA_Studio.StatePattern;
using UnityEngine;

namespace AngerStudio.HomingMeSoul.Game
{

    public class GamePreparing : State<GameCore>
    {
        public GamePreparing (StateMachine<GameCore> machine) : base(machine)
        {
        }

        public override void OnEntered ()
        {
            Context.Prepare();
            Context.CreaterPlayers();
            ChangeState(new GameStarting(StateMachine));
        }

        public override void Update ()
        {
        }
    }

    public class GameStarting : State<GameCore>
    {
        public GameStarting (StateMachine<GameCore> machine) : base(machine)
        {
        }
        public override void OnEntered ()
        {
            ChangeState(new GameOngoing(StateMachine));
        } 

        public override void Update ()
        {
            throw new System.NotImplementedException();
        }
    }

    public class GameOngoing : State<GameCore>
    {
        float GAME_TIME = 60;
        float countDown;

        public GameOngoing (StateMachine<GameCore> machine) : base(machine)
        {
            countDown = Time.time;
        }

        float lastSupplyTime, lastPassiveSpGainTime;

        public override void Update ()
        {
            Context.ReceieveInput();
            Context.DecentStamina();
            Context.PlayerMove();
            Context.RotatePlayerInHome();
            Context.RotatePlayerOnLocation();

            
            //Rotate the supply belt
            for (int i = 0; i < Context.gravityZones.Value.Length; i++) Context.gravityZones.Value[i].transform.Rotate(Vector3.back * Context.config.Value.gravityZonesRevolutionSpeeds[i] * Time.deltaTime);
            //Spawning supplies...
            if (Context.SuppliesSum < Context.config.Value.minSupplyDrops)
            {
                Context.SpawnSupplyInRandomZone(Context.GetLeastPickupTypeIndex());
                lastSupplyTime = Time.time;
            }
            if (Time.time - lastSupplyTime > 3f && Context.SuppliesSum < Context.config.Value.maxSupplyDrops)
            {
                Context.SpawnSupplyInRandomZone(Context.GetLeastPickupTypeIndex());
                lastSupplyTime = Time.time;
            }
            //Free SP
            if (Time.time - lastPassiveSpGainTime > Context.config.Value.passiveSPGainDelayInSconds)
            {
                Context.sp.Value += 1;
                lastPassiveSpGainTime = Time.time;
            }

            //Controlling bad guys...
            //Random events...

            if (Time.time - countDown > GAME_TIME)
            {
                ChangeState(new GameFinished(StateMachine));
            }
            else
            {
                Context.countDownText.text = ((int)(GAME_TIME - (Time.time - countDown))).ToString();
            }
        }
    }

    public class GamePaused : State<GameCore>
    {
        public GamePaused (StateMachine<GameCore> machine) : base(machine)
        {
        }

        public override void Update ()
        {
            throw new System.NotImplementedException();
        }
    }

    public class GameFinished : State<GameCore>
    {
        public GameFinished (StateMachine<GameCore> machine) : base(machine)
        {
        }

        public override void OnEntered()
        {
            Context.ShowFinishUI();
        }

        public override void Update ()
        {
            if (SimpleInput.GetKeyDown(KeyCode.Space))
            {
                
            }
        }
    }
}