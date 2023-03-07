using GGJ23.Game;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;

namespace GGJ23.UI
{
    public class UIGameOverScreen : UIScreen
    {
        public LocalizeStringEvent scoreLocalizeStringEvent;
        public ContentSizeFitter controlsGroup;

        public override void Enter()
        {
            base.Enter();
            var time = scoreLocalizeStringEvent.StringReference["score"] as IntVariable;
            time.Value = (int)_uiController.gameManager.Score;

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)controlsGroup.transform);
            _uiController.SetPauseAudioVolume();
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

        public override void Exit()
        {
            base.Exit();
            _uiController.ResetAudioVolume();
        }
    }
}
