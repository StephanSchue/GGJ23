using UnityEngine;

namespace GGJ23.UI
{
    public class UIGameScreen : UIScreen
    {
        public override void Tick(float dt, UIInputData inputData)
        {
            if (inputData.IsPressed(UIInputButton.Accept))
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
