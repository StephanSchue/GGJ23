using GGJ23.Game;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ23.UI
{
    public class UIPauseScreen : UIScreen
    {
        public ContentSizeFitter controlsGroup;

        public override void Enter()
        {
            base.Enter();
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)controlsGroup.transform);
        }

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
            else if (inputData.IsPressed(InputButton.Function02))
            {
                DoAction(UIAction.Open_HelpScreen);
            }
            else if (inputData.IsPressed(InputButton.Function03))
            {
                DoAction(UIAction.Stop_Game);
            }
        }
    }
}
