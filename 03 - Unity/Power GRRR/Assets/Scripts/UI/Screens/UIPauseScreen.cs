using GGJ23.Game;
using UnityEngine;

namespace GGJ23.UI
{
    public class UIPauseScreen : UIScreen
    {
        public override void Tick(float dt, UIInputData inputData)
        {
            if (inputData.IsPressed(InputButton.Accept))
            {
                DoAction(UIAction.Continue_Game);
            }
            else if (inputData.IsPressed(InputButton.Function01))
            {
                DoAction(UIAction.Restart_Game);
            }
            else if (inputData.IsPressed(InputButton.Function03))
            {
                DoAction(UIAction.Stop_Game);
            }
        }
    }
}
