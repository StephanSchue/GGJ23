using GGJ23.Game;
using UnityEngine;

namespace GGJ23.UI
{
    public class UIHelpScreen : UIScreen
    {
        public GameObject buttonBackToStartScreen;
        public GameObject buttonBackToPauseScreen;

        private bool _enteredFromPauseScreen => _uiController.Paused;

        public override void Enter()
        {
            base.Enter();

            buttonBackToStartScreen.SetActive(!_enteredFromPauseScreen);
            buttonBackToPauseScreen.SetActive(_enteredFromPauseScreen);
        }

        public override void Tick(float dt, UIInputData inputData)
        {
            if (inputData.IsPressed(InputButton.Function01))
            {
                DoAction(UIAction.Open_ControlsScreen);
            }
            else if (inputData.IsPressed(InputButton.Cancel))
            {
                if (_enteredFromPauseScreen) { DoAction(UIAction.Open_PauseScreen); }
                else { DoAction(UIAction.Open_StartScreen);  }
            }
        }
    }
}
