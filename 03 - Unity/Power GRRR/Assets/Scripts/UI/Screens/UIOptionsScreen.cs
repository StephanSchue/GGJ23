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
        public TMPro.TMP_Dropdown dropdown;

        public Sprite audioMuteActive;
        public Sprite audioMuteInactive;

        private GameObject lastSelection = null;

        public override void Enter()
        {
            base.Enter();
            Refresh();

            _uiController.eventSystem.SetSelectedGameObject(audioMusicVolume.gameObject);
        }

        public override void Exit()
        {
            base.Hide();
        }

        public override void Tick(float dt, UIInputData inputData)
        {
            if (_uiController.eventSystem.currentSelectedGameObject != null)
            {
                lastSelection = _uiController.eventSystem.currentSelectedGameObject;
            }
            else
            {
                _uiController.eventSystem.SetSelectedGameObject(lastSelection);
            }

            if (inputData.IsPressed(InputButton.Accept))
            {
                if (lastSelection == dropdown.gameObject)
                {
                    dropdown.value = dropdown.value + 1 < dropdown.options.Count ? dropdown.value+1 : 0;
                }
            }
            else if (inputData.IsPressed(InputButton.Function01))
            {
                DoAction(UIAction.Save_Options);
            }
            else if (inputData.IsPressed(InputButton.Function02))
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

            bool musicMute = _uiController.Settings.audioMusicMute || _uiController.Settings.audioMusicVolume == 0;
            bool sfxMute = _uiController.Settings.audioSFXMute || _uiController.Settings.audioSFXVolume == 0;

            if (audioMusicMute.targetGraphic is Image audioMusicMuteImage) { audioMusicMuteImage.sprite = musicMute ? audioMuteActive : audioMuteInactive; }
            if (audioSFXMute.targetGraphic is Image audioSFXMuteImage) { audioSFXMuteImage.sprite = sfxMute ? audioMuteActive : audioMuteInactive; }
        }

        public void UpdateAudioMusicVolume(float value)
        {
            _uiController.UpdateOptionInt(Setting.AudioMusicVolume, (int)value);
            Refresh();
        }

        public void ToggleAudioMusicMute()
        {
            _uiController.UpdateOptionBool(Setting.AudioMusicVolume, !_uiController.Settings.audioMusicMute);
            Refresh();
        }

        public void UpdateAudioSFXVolume(float value)
        {
            _uiController.UpdateOptionInt(Setting.AudioSFXVolume, (int)value);
            Refresh();
        }

        public void ToggleAudioSFXMute()
        {
            _uiController.UpdateOptionBool(Setting.AudioSFXVolume, !_uiController.Settings.audioSFXMute);
            Refresh();
        }
    }
}
