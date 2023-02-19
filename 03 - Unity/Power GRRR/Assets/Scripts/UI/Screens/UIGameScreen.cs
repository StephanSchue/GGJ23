using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ23.UI
{
    public class UIGameScreen : UIScreen
    {
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
            if (inputData.IsPressed(UIInputButton.Accept) || inputData.IsHold(UIInputButton.Accept) || _uiRepairButtonHold)
            {
                DoAction(UIAction.Gameplay_Repair);
            }
            else if (inputData.IsPressed(UIInputButton.Function01))
            {
                DoAction(UIAction.Gameplay_Boost);
            }
            else if (inputData.IsPressed(UIInputButton.Function03))
            {
                DoAction(UIAction.Open_PauseScreen);
            }
        }
    }
}
