using UnityEngine;

namespace GGJ23.UI
{
    public class UIGameOverScreen : UIScreen
    {
        public TMPro.TextMeshProUGUI score;

        public override void Enter()
        {
            base.Enter();
            score.text = $"Score: {_uiController.gameManager.Score}";
        }

        public override void Tick(float dt, UIInputData inputData)
        {
            if (inputData.IsPressed(UIInputButton.Accept))
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
