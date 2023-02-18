using UnityEngine;
using UnityEngine.EventSystems;

namespace GGJ23.UI
{
    public class UIGameScreen : UIScreen
    {
        private bool _repairButtonHold = false;

        public void OnRepairButtonDown()
        {
            _repairButtonHold = true;
        }

        public void OnRepairButtonUp()
        {
            _repairButtonHold = false;
        }

        public override void Enter()
        {
            base.Enter();
            _repairButtonHold = false;
        }

        public override void Tick(float dt, UIInputData inputData)
        {
            if (inputData.IsPressed(UIInputButton.Accept) || _repairButtonHold)
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
