using UnityEngine;

namespace GGJ23.UI
{
    public class UICreditsScreen : UIScreen
    {
        public override void Tick(float dt, UIInputData inputData)
        {
            if (inputData.IsPressed(UIInputButton.Cancel))
            {
                DoAction(UIAction.Open_StartScreen);
            }
        }
    }
}
