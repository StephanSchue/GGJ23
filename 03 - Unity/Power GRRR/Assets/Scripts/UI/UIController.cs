using GGJ23.Game;
using GGJ23.Managment;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace GGJ23.UI
{
    [System.Serializable]
    public enum UIState
    {
        None,
        StartScreen,
        GameScreen,
        GameOverScreen,
        PauseScreen,
        HelpScreen,
        OptionScreen,
        CreditsScreen
    }

    [System.Serializable]
    public enum UIAction
    {
        None,
        Open_StartScreen,
        Start_Game,
        Open_GameOverScreen,
        Open_PauseScreen,
        Open_HelpScreen,
        Open_OptionScreen,
        Open_CreditsScreen,
        Exit_Game,
        Save_Options,
        Reset_Options,
        Gameplay_Repair,
        Gameplay_Boost,
        Continue_Game,
        Restart_Game,
        Stop_Game,
    }

    public enum UIInputButton
    {
        Accept,
        Cancel,
        Function01,
        Function02,
        Function03,
        Up,
        Right,
        Down,
        Left,
    }

    public enum UIInputStatus
    {
        None,
        Press,
        Hold,
        Release
    }
    
    public class UIInputData
    {
        public UIInputStatus Accept; // A
        public UIInputStatus Cancel; // B
        public UIInputStatus Function01; // X
        public UIInputStatus Function02; // Y
        public UIInputStatus Function03; // Menu

        public UIInputStatus Up;
        public UIInputStatus Right;
        public UIInputStatus Down;
        public UIInputStatus Left;

        public void SetButtonStatus(UIInputButton button, bool press, bool hold, bool release)
        {
            var status = UIInputStatus.None;
            if (press) { status = UIInputStatus.Press; }
            else if (hold) { status = UIInputStatus.Hold; }
            else if (release) { status = UIInputStatus.Release; }

            switch (button)
            {
                case UIInputButton.Accept:
                    Accept = status;
                    break;
                case UIInputButton.Cancel:
                    Cancel = status;
                    break;
                case UIInputButton.Function01:
                    Function01 = status;
                    break;
                case UIInputButton.Function02:
                    Function02 = status;
                    break;
                case UIInputButton.Function03:
                    Function03 = status;
                    break;
                case UIInputButton.Up:
                    Up = status;
                    break;
                case UIInputButton.Right:
                    Right = status;
                    break;
                case UIInputButton.Down:
                    Down = status;
                    break;
                case UIInputButton.Left:
                    Left = status;
                    break;
                default:
                    break;
            }
        }

        public UIInputStatus GetButtonStatus(UIInputButton button)
        {
            switch (button)
            {
                case UIInputButton.Accept:
                    return Accept;
                case UIInputButton.Cancel:
                    return Cancel;
                case UIInputButton.Function01:
                    return Function01;
                case UIInputButton.Function02:
                    return Function02;
                case UIInputButton.Function03:
                    return Function03;
                case UIInputButton.Up:
                    return Up;
                case UIInputButton.Right:
                    return Right;
                case UIInputButton.Down:
                    return Down;
                case UIInputButton.Left:
                    return Left;
                default:
                    return UIInputStatus.None;
            }
        }

        public bool IsPressed(UIInputButton button)
        {
            return GetButtonStatus(button) == UIInputStatus.Press;
        }

        public bool IsHold(UIInputButton button)
        {
            return GetButtonStatus(button) == UIInputStatus.Hold;
        }

        public bool IsRelease(UIInputButton button)
        {
            return GetButtonStatus(button) == UIInputStatus.Release;
        }
    }

    public enum Setting
    {
        AudioMasterVolume,
        AudioMusicVolume,
        AudioSFXVolume,
    }

    public struct Settings
    {
        public int audioMasterVolume;
        public int audioMusicVolume;
        public int audioSFXVolume;
        public int language;
    }

    public class UIController : MonoBehaviour
    {
        [System.Serializable]
        public struct UIScreenCollection
        {
            public UIScreen StartScreen;
            public UIScreen GameScreen;
            public UIScreen GameOverScreen;
            public UIScreen PauseScreen;
            public UIScreen HelpScreen;
            public UIScreen OptionScreen;
            public UIScreen CreditsScreen;

            public void Init(UIController uiController)
            {
                StartScreen?.Init(uiController, true);
                GameScreen?.Init(uiController, false);
                GameOverScreen?.Init(uiController, false);
                PauseScreen?.Init(uiController, false);
                HelpScreen?.Init(uiController, false);
                OptionScreen?.Init(uiController, false);
                CreditsScreen?.Init(uiController, false);
            }

            public UIScreen GetCurrentScreen(UIState index)
            {
                switch (index)
                {
                    case UIState.None:
                        return null;
                    case UIState.StartScreen:
                        return StartScreen;
                    case UIState.GameScreen:
                        return GameScreen;
                    case UIState.GameOverScreen:
                        return GameOverScreen;
                    case UIState.PauseScreen:
                        return PauseScreen;
                    case UIState.HelpScreen:
                        return HelpScreen;
                    case UIState.OptionScreen:
                        return OptionScreen;
                    case UIState.CreditsScreen:
                        return CreditsScreen;
                    default:
                        return null;
                }
            }
        }

        [Header("Components")]
        public GameManager gameManager;
        public MovementController movementController;
        public InteractionController interactionController;
        public EventSystem eventSystem;
        public AudioMixer audioMixer;

        public UIScreenCollection screens;
        private UIState _currentState;
        private UIScreen _currentScreen => screens.GetCurrentScreen(_currentState);
        private UIInputData _inputData = new UIInputData();

        private Settings _settings;

        public Settings Settings => _settings;

        private void Awake()
        {
            screens.Init(this);
            SwitchState(UIState.StartScreen);
        }

        private void Start()
        {
            LoadOptions();
        }

        private void OnDisable()
        {
            
        }

        private void Update()
        {
            _currentScreen?.Tick(Time.deltaTime, _inputData);
        }

        #region Statemachine

        public void DoAction(UIAction uiAction)
        {
            Debug.Log($"UIController::DoAction {uiAction}");

            switch (uiAction)
            {
                case UIAction.Open_StartScreen:
                    SwitchState(UIState.StartScreen);
                    LoadOptions();
                    break;
                case UIAction.Start_Game:
                    SwitchState(UIState.GameScreen);
                    gameManager.StartGame();
                    break;
                case UIAction.Open_GameOverScreen:
                    SwitchState(UIState.GameOverScreen);
                    break;
                case UIAction.Open_PauseScreen:
                    SwitchState(UIState.PauseScreen);
                    gameManager.Pause(true);
                    break;
                case UIAction.Open_HelpScreen:
                    SwitchState(UIState.HelpScreen);
                    break;
                case UIAction.Open_OptionScreen:
                    SwitchState(UIState.OptionScreen);
                    break;
                case UIAction.Save_Options:
                    SaveOptions();
                    SwitchState(UIState.StartScreen);
                    break;
                case UIAction.Reset_Options:
                    ResetOptions();
                    break;
                case UIAction.Open_CreditsScreen:
                    SwitchState(UIState.CreditsScreen);
                    break;
                case UIAction.Exit_Game:
                    gameManager.Exit();
                    break;
                case UIAction.Gameplay_Repair:
                    interactionController.PressInteractButton();
                    break;
                case UIAction.Gameplay_Boost:
                    movementController.PressBoost();
                    break;
                case UIAction.Continue_Game:
                    SwitchState(UIState.GameScreen);
                    gameManager.ContinueGame();
                    break;
                case UIAction.Restart_Game:
                    SwitchState(UIState.GameScreen);
                    gameManager.RestartGame();
                    break;
                case UIAction.Stop_Game:
                    SwitchState(UIState.StartScreen);
                    gameManager.StopGame();
                    break;
            }
        }

        public void SwitchState(UIState newState)
        {
            _currentScreen?.Exit();
            _currentState = newState;
            _currentScreen.Enter();
        }

        public void RefreshUI()
        {
            StartCoroutine(InternalRefreshUI());
        }

        private IEnumerator InternalRefreshUI()
        {
            yield return new WaitForEndOfFrame();
            _currentScreen.Refresh();
        }

        #endregion

        #region Input

        private void OnConfirm(InputValue value)
        {
            _inputData.SetButtonStatus(UIInputButton.Accept, value.isPressed, value.isPressed, !value.isPressed);
        }

        private void OnCancel(InputValue value)
        {
            _inputData.SetButtonStatus(UIInputButton.Cancel, value.isPressed, value.isPressed, !value.isPressed);
        }

        private void OnFunction01(InputValue value)
        {
            _inputData.SetButtonStatus(UIInputButton.Function01, value.isPressed, value.isPressed, !value.isPressed);
        }

        private void OnFunction02(InputValue value)
        {
            _inputData.SetButtonStatus(UIInputButton.Function02, value.isPressed, value.isPressed, !value.isPressed);
        }

        private void OnFunction03(InputValue value)
        {
            _inputData.SetButtonStatus(UIInputButton.Function03, value.isPressed, value.isPressed, !value.isPressed);
        }

        #endregion

        #region Options

        private void ApplyOptions()
        {
            audioMixer.SetFloat("MasterVolume", _settings.audioMasterVolume > 0 ? Mathf.Log10(_settings.audioMasterVolume / 10f) * 20 : -80f);
            audioMixer.SetFloat("MusicVolume", _settings.audioMusicVolume > 0 ? Mathf.Log10(_settings.audioMusicVolume / 10f) * 20 : -80f);
            audioMixer.SetFloat("SFXVolume", _settings.audioSFXVolume > 0 ? Mathf.Log10(_settings.audioSFXVolume / 10f) * 20 : -80f);

            if (_settings.language > -1)
            {
                var locale = LocalizationSettings.AvailableLocales.Locales[Mathf.Clamp(_settings.language, 0, LocalizationSettings.AvailableLocales.Locales.Count)];
                LocalizationSettings.SelectedLocale = locale;
            }
        }

        private void LoadOptions()
        {
            _settings.audioMasterVolume = PlayerPrefs.GetInt("AudioMasterVolume", 10);
            _settings.audioMusicVolume = PlayerPrefs.GetInt("AudioMusicVolume", 10);
            _settings.audioSFXVolume = PlayerPrefs.GetInt("AudioSFXVolume", 10);
            _settings.language = PlayerPrefs.GetInt("Language", -1);

            ApplyOptions();
        }

        private void SaveOptions()
        {
            PlayerPrefs.SetInt("AudioMasterVolume", _settings.audioMasterVolume);
            PlayerPrefs.SetInt("AudioMusicVolume", _settings.audioMusicVolume);
            PlayerPrefs.SetInt("AudioSFXVolume", _settings.audioSFXVolume);

            for (int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; i++)
            {
                if (LocalizationSettings.SelectedLocale == LocalizationSettings.AvailableLocales.Locales[i])
                {
                    _settings.language = i;
                    break;
                }
            }

            PlayerPrefs.SetInt("Language", _settings.language);

            PlayerPrefs.Save();
        }

        private void ResetOptions()
        {
            _settings.audioMasterVolume = 10;
            _settings.audioMusicVolume = 10;
            _settings.audioSFXVolume = 10;

            PlayerPrefs.SetInt("AudioMasterVolume", _settings.audioMasterVolume);
            PlayerPrefs.SetInt("AudioMusicVolume", _settings.audioMusicVolume);
            PlayerPrefs.SetInt("AudioSFXVolume", _settings.audioSFXVolume);

            ApplyOptions();

            _currentScreen.Refresh();
        }

        public void UpdateOptionInt(Setting option, int value)
        {
            switch (option)
            {
                case Setting.AudioMasterVolume:
                    _settings.audioMasterVolume = Mathf.Clamp(value, 0, 10);
                    ApplyOptions();
                    break;
                case Setting.AudioMusicVolume:
                    _settings.audioMusicVolume = Mathf.Clamp(value, 0, 10);
                    ApplyOptions();
                    break;
                case Setting.AudioSFXVolume:
                    _settings.audioSFXVolume = Mathf.Clamp(value, 0, 10);
                    ApplyOptions();
                    break;
            }
        }

        #endregion

        //private void OnGameOver()
        //{
        //    Debug.Log("GameOver:Enter");
        //    //_gameOverContainer.style.display = DisplayStyle.Flex;
        //    //_valueScore.text = string.Format("{0}", gameManager.Score);
        //    _gameOverOpen = true;
        //}

        //private void OnGameOverExit()
        //{
        //    Debug.Log("GameOver:Exit");
        //    //_gameOverContainer.style.display = DisplayStyle.None;
        //    _gameOverOpen = false;
        //}

        //private void OnClickBoost()
        //{
        //    Debug.Log("UIController:OnClickBoost");
        //    movementController.PressBoost();
        //}

        //private void OnClickInteractEnter()
        //{
        //    Debug.Log("UIController:OnClickInteractEnter");
        //    interactionController.PressInteractButton();
        //}

        //private void OnClickInteractExit()
        //{
        //    Debug.Log("UIController:OnClickInteractExit");
        //    interactionController.LeaveInteractButton();
        //}

        //private void TogglePause()
        //{
        //    _pauseOpen = !_pauseOpen;
        //    OnClickPause(_pauseOpen);
        //}

        //private void OnClickPause(bool active)
        //{
        //    //_pauseContainer.style.display = active ? DisplayStyle.Flex : DisplayStyle.None;
        //    gameManager.Pause(active);

        //    //if (_buttonHelp != null)
        //    //{
        //    //    _buttonHelp.focusable = active;
        //    //}

        //    //if (_buttonRestart != null)
        //    //{
        //    //    _buttonRestart.focusable = active;
        //    //}

        //    //if (_buttonExit != null)
        //    //{
        //    //    _buttonExit.focusable = active;
        //    //}
        //}

        //private void OnClickHelp()
        //{
        //    Debug.Log("Help:Enter");
        //    //_helpContainer.style.display = DisplayStyle.Flex;
        //    _helpOpen = true;
        //}

        //private void OnClickHelpExit()
        //{
        //    Debug.Log("Help:Exit");
        //    //_helpContainer.style.display = DisplayStyle.None;
        //    _helpOpen = false;
        //}

        //private void OnClickRestart()
        //{
        //    Debug.Log("UIController::Restart");
        //    OnGameOverExit();
        //    gameManager.Restart();
        //}

        //private void OnClickExit()
        //{
        //    Debug.Log("UIController::Exit");
        //    gameManager.Exit();
        //}
    }
}