using UnityEngine;

namespace GGJ23.UI
{
    public class UIPauseScreen : UIScreen
    {
        public override void Tick(float dt, UIInputData inputData)
        {
            if (inputData.IsPressed(UIInputButton.Accept))
            {
                DoAction(UIAction.Continue_Game);
            }
            else if (inputData.IsPressed(UIInputButton.Function01))
            {
                DoAction(UIAction.Restart_Game);
            }
            else if (inputData.IsPressed(UIInputButton.Function03))
            {
                DoAction(UIAction.Stop_Game);
            }
        }
    }
}
