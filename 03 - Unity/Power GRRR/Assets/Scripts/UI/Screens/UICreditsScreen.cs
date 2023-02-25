using GGJ23.Game;
using UnityEngine;

namespace GGJ23.UI
{
    public class UICreditsScreen : UIScreen
    {
        public override void Tick(float dt, UIInputData inputData)
        {
            if (inputData.IsPressed(InputButton.Cancel))
            {
                DoAction(UIAction.Open_StartScreen);
            }
        }
    }
}
