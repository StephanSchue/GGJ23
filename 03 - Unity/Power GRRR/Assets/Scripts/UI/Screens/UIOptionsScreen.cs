using GGJ23.Game;
using UnityEngine;
using UnityEngine.UI;

namespace GGJ23.UI
{
    public class UIOptionsScreen : UIScreen
    {
        public Slider audioMusicVolume;
        public Button audioMusicMute;
        public Slider audioSFXVolume;
        public Button audioSFXMute;

        public Sprite audioMuteActive;
        public Sprite audioMuteInactive;

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
            audioMusicVolume.value = _uiController.Settings.audioMusicVolume;
            audioSFXVolume.value = _uiController.Settings.audioSFXVolume;

            if(audioMusicMute.targetGraphic is Image audioMusicMuteImage) { audioMusicMuteImage.sprite = _uiController.Settings.audioMusicMute ? audioMuteActive : audioMuteInactive; }
            if (audioSFXMute.targetGraphic is Image audioSFXMuteImage) { audioSFXMuteImage.sprite = _uiController.Settings.audioSFXMute ? audioMuteActive : audioMuteInactive; }
        }

        public void UpdateAudioMusicVolume(float value)
        {
            _uiController.UpdateOptionInt(Setting.AudioMusicVolume, (int)value);
        }

        public void ToggleAudioMusicMute()
        {
            _uiController.UpdateOptionBool(Setting.AudioMusicVolume, !_uiController.Settings.audioMusicMute);

            if (audioMusicMute.targetGraphic is Image audioMusicMuteImage) { audioMusicMuteImage.sprite = _uiController.Settings.audioMusicMute ? audioMuteActive : audioMuteInactive; }
        }

        public void UpdateAudioSFXVolume(float value)
        {
            _uiController.UpdateOptionInt(Setting.AudioSFXVolume, (int)value);
        }

        public void ToggleAudioSFXMute()
        {
            _uiController.UpdateOptionBool(Setting.AudioSFXVolume, !_uiController.Settings.audioSFXMute);

            if (audioSFXMute.targetGraphic is Image audioSFXMuteImage) { audioSFXMuteImage.sprite = _uiController.Settings.audioSFXMute ? audioMuteActive : audioMuteInactive; }
        }
    }
}
