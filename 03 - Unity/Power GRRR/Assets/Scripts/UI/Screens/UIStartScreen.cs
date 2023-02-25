using GGJ23.Game;
using UnityEngine.UI;

namespace GGJ23.UI
{
    public class UIStartScreen : UIScreen
    {
        public override void Enter()
        {
            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Tick(float dt, UIInputData inputData)
        {
            if (inputData.IsPressed(InputButton.Accept))
            {
                DoAction(UIAction.Start_Game);
            }
            else if (inputData.IsPressed(InputButton.Cancel))
            {
                DoAction(UIAction.Exit_Game);
            }
            else if (inputData.IsPressed(InputButton.Function01))
            {
                DoAction(UIAction.Open_HelpScreen);
            }
            else if (inputData.IsPressed(InputButton.Function03))
            {
                DoAction(UIAction.Open_OptionScreen);
            }
            else if (inputData.IsPressed(InputButton.Function02))
            {
                DoAction(UIAction.Open_CreditsScreen);
            }
        }
    }
}
