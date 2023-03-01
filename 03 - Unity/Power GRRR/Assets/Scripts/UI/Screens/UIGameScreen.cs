using GGJ23.Game;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GGJ23.UI
{
    public class UIGameScreen : UIScreen
    {
        public Image gameProgress;
        public CanvasGroup interactinPad;
        private bool _uiRepairButtonHold = false;

        public void OnRepairButtonDown()
        {
            _uiRepairButtonHold = true;
        }

        public void OnRepairButtonUp()
        {
            _uiRepairButtonHold = false;
        }

        public override void Enter()
        {
            base.Enter();
            _uiRepairButtonHold = false;
        }

        public override void Tick(float dt, UIInputData inputData)
        {
            gameProgress.fillAmount = _uiController.gameManager.Energy;

            interactinPad.alpha = _uiController.interactionController.IsWorking ? 1f : 0f;
            interactinPad.interactable = interactinPad.blocksRaycasts = _uiController.interactionController.IsWorking;

            if (inputData.IsPressed(InputButton.Accept) || inputData.IsHold(InputButton.Accept) || _uiRepairButtonHold)
            {
                DoAction(UIAction.Gameplay_Repair);
            }
            else if (inputData.IsPressed(InputButton.Function01))
            {
                DoAction(UIAction.Gameplay_Boost);
            }
            else if (inputData.IsPressed(InputButton.Function03))
            {
                DoAction(UIAction.Open_PauseScreen);
            }
        }
    }
}
