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
