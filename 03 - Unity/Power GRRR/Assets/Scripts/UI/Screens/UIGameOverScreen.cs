using GGJ23.Game;
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
            if (inputData.IsPressed(InputButton.Accept))
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
