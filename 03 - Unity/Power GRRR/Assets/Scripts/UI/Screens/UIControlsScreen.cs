using DG.Tweening;
using GGJ23.Game;
using UnityEngine;

namespace GGJ23.UI
{
    public class UIControlsScreen : UIScreen
    {
        public CanvasGroup[] slides;

        public override void Tick(float dt, UIInputData inputData)
        {
            if (inputData.IsPressed(InputButton.Cancel))
            {
                DoAction(UIAction.Open_HelpScreen);
            }
        }

        public void OnInputDeviceChange(InputControlSchema inputControl)
        {
            for (int i = 0; i < slides.Length; i++)
            {
                slides[i].DOFade(i == (int)inputControl ? 1 : 0f, 0.25f);
            }
        }
    }
}
