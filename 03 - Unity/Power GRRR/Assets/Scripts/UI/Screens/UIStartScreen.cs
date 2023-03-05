using GGJ23.Game;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ23.UI
{
    public class UIStartScreen : UIScreen
    {
        public ContentSizeFitter controlsGroup;
        public Button startButton;
        public Animator startButtonAnimator;

        public override void Enter()
        {
            base.Enter();
            _uiController.eventSystem.SetSelectedGameObject(startButton.gameObject);
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)controlsGroup.transform);

            startButtonAnimator.enabled = true;
        }

        public override void Exit()
        {
            base.Exit();
            startButtonAnimator.enabled = false;
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
