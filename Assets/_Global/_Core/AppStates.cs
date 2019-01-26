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
        float[] lastKeyDownTime = new float[8];
        float[] lastKeyUpTime = new float[8];
        

        public AwaitingPlayer (StateMachine<AppCore> machine) : base(machine)
        {
        }

        public override void Update ()
        {
            foreach (KeyCode vKey in Context.allowedKeys)
            {
                if (SimpleInput.GetKeyUp(vKey))
                {
                    if (!Context.activePlayers.Any(kvp => kvp.Key == vKey)) Context.AddPlayer(vKey);
                    else
                    {
                        Context.avaliableColors.Add(Context.activePlayers[vKey]);
                        Context.activePlayers.Remove(vKey);
                        Context.m_View.RemovePlayerCard(vKey);                        
                    }
                }
            }
        }
    }

    public class GameStarting : State<AppCore>
    {
        public GameStarting (StateMachine<AppCore> machine) : base(machine)
        {
        }

        public override void OnEntered ()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(Context.toLoad);
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
