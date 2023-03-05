using DG.Tweening;
using GGJ23.Game;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ23.UI
{
    public class UIControlsScreen : UIScreen
    {
        public CanvasGroup[] slides;
        public Button[] schemaButtons;

        public override void Tick(float dt, UIInputData inputData)
        {
            if (inputData.IsPressed(InputButton.Cancel))
            {
                DoAction(UIAction.Open_HelpScreen);
            }
        }

        public void OnInputDeviceChange(InputControlSchema inputControl)
        {
            ChangeShema((int)inputControl);
        }

        public void ChangeShema(int index)
        {
            for (int i = 0; i < slides.Length; i++)
            {
                slides[i].DOFade(i == index ? 1 : 0f, 0.25f);
            }

            _uiController.eventSystem.SetSelectedGameObject(schemaButtons[index].gameObject);
        }
    }
}
