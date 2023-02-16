using UnityEngine.UI;

namespace GGJ23.UI
{
    public class UIStartScreen : UIScreen
    {
        public Button startButton;
        public Button helpButton;
        public Button optionsButton;
        public Button creditsButton;
        public Button exitButton;

        public void Start()
        {
            startButton.onClick.AddListener(() => DoAction(UIAction.Start_Game));
            helpButton.onClick.AddListener(() => DoAction(UIAction.Open_HelpScreen));
            optionsButton.onClick.AddListener(() => DoAction(UIAction.Open_OptionScreen));
            creditsButton.onClick.AddListener(() => DoAction(UIAction.Open_CreditsScreen));
            exitButton.onClick.AddListener(() => DoAction(UIAction.Exit_Game));
        }

        public override void Enter()
        {
            Show();
        }

        public override void Exit()
        {
            Hide();
        }

        public override void Tick(float dt, UIInputData inputData)
        {
            if (inputData.IsPressed(UIInputButton.Accept))
            {
                DoAction(UIAction.Start_Game);
            }
            else if (inputData.IsPressed(UIInputButton.Cancel))
            {
                DoAction(UIAction.Exit_Game);
            }
            else if (inputData.IsPressed(UIInputButton.Function01))
            {
                DoAction(UIAction.Open_HelpScreen);
            }
            else if (inputData.IsPressed(UIInputButton.Function02))
            {
                DoAction(UIAction.Open_OptionScreen);
            }
            else if (inputData.IsPressed(UIInputButton.Function03))
            {
                DoAction(UIAction.Open_CreditsScreen);
            }
        }
    }
}
