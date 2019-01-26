using System.Linq;
using System.Collections;
using System.Collections.Generic;
using BA_Studio.StatePattern;
using UnityEngine;
using DG.Tweening;

namespace AngerStudio.HomingMeSoul.Core
{

    public class Awaking : State<AppCore>
    {
        public Awaking (StateMachine<AppCore> machine) : base(machine)
        {
        }

        public override void OnEntered ()
        {
            ChangeState(new TitleScreen(StateMachine));
        }

        public override void Update ()
        {
        }
    }

    public class TitleScreen : State<AppCore>
    {
        public TitleScreen (StateMachine<AppCore> machine) : base(machine)
        {
        }

        public override void OnEntered ()
        {
             Context.titleGroup.DOFade(1, 0.43f).OnComplete(() => ChangeState(new TitleScreen_ON(StateMachine)));    
        }

        public override void Update ()
        {
        }
    }
    
    public class TitleScreen_ON : State<AppCore>
    {
        public TitleScreen_ON (StateMachine<AppCore> machine) : base(machine)
        {
        }

        public override void OnEntered ()
        {

        }

        public override void Update ()
        {
            foreach (KeyCode k in Context.allowedKeys)
            {
                if (SimpleInput.GetKey(k))
                    Context.titleT.DOLocalMoveY(Context.titleT.localPosition.y + 900, 0.75f, true)
                        .OnComplete(() => Context.titleT.gameObject.SetActive(false))
                        .OnComplete(() => ChangeState(new AwaitingPlayer(StateMachine)));      
            }
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
                if (SimpleInput.GetKeyDown(vKey))
                {
                    Context.TogglePlayer(vKey);
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
