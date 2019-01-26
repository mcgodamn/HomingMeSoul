using System.Linq;
using System.Collections;
using System.Collections.Generic;
using BA_Studio.StatePattern;
using UnityEngine;

namespace AngerStudio.HomingMeSoul.Core
{

    public class Awaking : State<AppCore>
    {
        public Awaking (StateMachine<AppCore> machine) : base(machine)
        {
        }

        public override void OnEntered ()
        {
            ChangeState(new AwaitingPlayer(StateMachine));
        }

        public override void Update ()
        {
        }
    }

    public class AwaitingPlayer : State<AppCore>
    {
        
        public AwaitingPlayer (StateMachine<AppCore> machine) : base(machine)
        {
        }

        public override void Update ()
        {
            foreach (KeyCode k in Context.config.keySets)
                if (Input.GetKeyDown(k) && Context.activePlayers.All(p => p.Item1 == k )) Context.AddPlayer(k);
        }
    }

    public class GameStarting : State<AppCore>
    {
        public GameStarting (StateMachine<AppCore> machine) : base(machine)
        {
        }

        public override void Update ()
        {
            throw new System.NotImplementedException();
        }
    }

    public class GameOngoing : State<AppCore>
    {
        public GameOngoing (StateMachine<AppCore> machine) : base(machine)
        {
        }

        public override void Update ()
        {
            throw new System.NotImplementedException();
        }
    }

    public class GameFinished : State<AppCore>
    {
        public GameFinished (StateMachine<AppCore> machine) : base(machine)
        {
        }

        public override void Update ()
        {
            throw new System.NotImplementedException();
        }
    }

    public class NextGame : State<AppCore>
    {
        public NextGame (StateMachine<AppCore> machine) : base(machine)
        {
        }

        public override void Update ()
        {
            throw new System.NotImplementedException();
        }
    }

    public class AppPaused : State<AppCore>
    {
        public AppPaused  (StateMachine<AppCore> machine) : base(machine)
        {
        }

        public override void Update ()
        {
            throw new System.NotImplementedException();
        }
    }

    public class AppClosing : State<AppCore>
    {
        public AppClosing  (StateMachine<AppCore> machine) : base(machine)
        {
        }

        public override void Update ()
        {
            throw new System.NotImplementedException();
        }
    }

}
