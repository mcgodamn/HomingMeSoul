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

        public override void Update ()
        {
        }
    }

    public class GameStarting : State<GameCore>
    {
        public GameStarting (StateMachine<GameCore> machine) : base(machine)
        {
        }

        public override void Update ()
        {
            throw new System.NotImplementedException();
        }
    }

    public class GameOngoing : State<GameCore>
    {
        public GameOngoing (StateMachine<GameCore> machine) : base(machine)
        {
        }

        public override void Update ()
        {
            //Rotate the supply belt
            for (int i = 0; i < Context.gravityZones.Value.Length; i++) Context.gravityZones.Value[i].transform.Rotate(Vector3.back * Context.config.Value.gravityZonesRevolutionSpeeds[i] * Time.deltaTime);

            //Spawning supplies...
            //Controlling bad guys...
            //Random events...
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

        public override void Update ()
        {
            throw new System.NotImplementedException();
        }
    }
}