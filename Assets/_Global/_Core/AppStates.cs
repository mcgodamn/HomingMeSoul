using System.Linq;
using System.Collections;
using System.Collections.Generic;
using BA_Studio.StatePattern;
using BA_Studio.UnityLib.SingletonLocator;
using BA_Studio.UnityLib.GlobalAudio;
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
            Context.ToggleCastingscreen(false);
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
        bool keyed = false;
        public TitleScreen_ON (StateMachine<AppCore> machine) : base(machine)
        {
        }

        public override void OnEntered ()
        {

        }

        public override void Update ()
        {
            if (keyed) return;
            foreach (KeyCode k in Context.allowedKeys)
            {
                if (SimpleInput.GetKey(k))
                {
                    Context.titleT.DOLocalMoveY(Context.titleT.localPosition.y + (Context.titleT.transform as RectTransform).rect.height, 0.75f, true)
                        .OnComplete(() => Context.titleT.gameObject.SetActive(false))
                        .OnComplete(() => ChangeState(new AwaitingPlayer(StateMachine)));      
                    keyed = true;
                }
            }
        }
    }

    public class AwaitingPlayer : State<AppCore>
    {
        float[] lastKeyDownTime = new float[8];
        float[] lastKeyUpTime = new float[8];
        int countDown;

        public AwaitingPlayer (StateMachine<AppCore> machine) : base(machine)
        {
        }

        public override void OnEntered ()
        {
            Context.ToggleCastingscreen(true);
            CountdownEnd();
        }

        void CountdownEnd ()
        {
            if (Context.activePlayers.Count < 2)
            {
                countDown = Context.config.csCountDownSec;
                MEC.Timing.RunCoroutine(Tick());
            }
            else ChangeState(new GameStarting(StateMachine));
        }

        IEnumerator<float> Tick ()
        {
            while (countDown > 0)
            {
                GlobalAudio.PlayPreLoadedClipByID("Tick");
                countDown -= 1;
                Context.countDownText.text = countDown.ToString();
                yield return MEC.Timing.WaitForSeconds(1);
            }
            CountdownEnd();
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
            if (Input.GetKeyDown(KeyCode.F8)) ChangeState(new AppRestarting(StateMachine));
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

    public class AppRestarting : State<AppCore>
    {
        public AppRestarting  (StateMachine<AppCore> machine) : base(machine)
        {
        }

        public override void OnEntered ()
        {
            SingletonBehaviourLocator<AppCore>.Set(null);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Reloader", UnityEngine.SceneManagement.LoadSceneMode.Single);
        }

        public override void Update ()
        {
            throw new System.NotImplementedException();
        }
    }


}
