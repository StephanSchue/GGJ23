using UnityEngine;
using UnityEngine.UI;

namespace GGJ23.UI
{
    public class UIOptionsScreen : UIScreen
    {
        public Slider audioMasterVolume;
        public Slider audioMusicVolume;
        public Slider audioSFXVolume;

        public override void Enter()
        {
            base.Enter();
            Refresh();
        }

        public override void Exit()
        {
            base.Hide();
        }

        public override void Tick(float dt, UIInputData inputData)
        {
            if (inputData.IsPressed(InputButton.Accept))
            {
                DoAction(UIAction.Save_Options);
            }
            else if (inputData.IsPressed(InputButton.Function01))
            {
                DoAction(UIAction.Reset_Options);
            }
            else if (inputData.IsPressed(InputButton.Cancel))
            {
                DoAction(UIAction.Open_StartScreen);
            }
        }
        
        public override void Refresh()
        {
            base.Refresh();
            audioMasterVolume.value = _uiController.Settings.audioMasterVolume;
            audioMusicVolume.value = _uiController.Settings.audioMusicVolume;
            audioSFXVolume.value = _uiController.Settings.audioSFXVolume;
        }

        public void UpdateAudioMasterVolume(float value)
        {
            _uiController.UpdateOptionInt(Setting.AudioMasterVolume, (int)value);
        }

        public void UpdateAudioMusicVolume(float value)
        {
            _uiController.UpdateOptionInt(Setting.AudioMusicVolume, (int)value);
        }

        public void UpdateAudioSFXVolume(float value)
        {
            _uiController.UpdateOptionInt(Setting.AudioSFXVolume, (int)value);
        }
    }
}
