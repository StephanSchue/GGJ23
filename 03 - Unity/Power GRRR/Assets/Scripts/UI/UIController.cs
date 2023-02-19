using GGJ23.Game;
using GGJ23.Managment;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using static UnityEngine.InputSystem.InputAction;

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

        public Vector2 Axis01; // X/Y

        public void SetButtonStatus(UIInputButton button, UIInputStatus status)
        {
            Debug.Log($"SetButtonStatus: {button}: {status}");

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

        // --- Variables ---
        [Header("Components")]
        public GameManager gameManager;
        public MovementController movementController;
        public InteractionController interactionController;
        public EventSystem eventSystem;
        public AudioMixer audioMixer;
        public PlayerInput playerInput;

        public UIScreenCollection screens;
        public TMPro.TextMeshProUGUI debugText;

        private UIState _currentState;
        private UIScreen _currentScreen => screens.GetCurrentScreen(_currentState);
        private UIInputData _inputData = new UIInputData();

        private Settings _settings;

        // --- Properties ---
        public Settings Settings => _settings;

        private void Awake()
        {
            screens.Init(this);
            SwitchState(UIState.StartScreen);

            gameManager.OnGameOver.AddListener(GameOver);
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
            UpdateMovement();

            debugText.text = $"Score: {gameManager.Score.ToString("0.00")}; Energy: {(int)(gameManager.Energy * 100f)}"; 
        }

        private void LateUpdate()
        {
            UpdateButtonStatus(UIInputButton.Accept);
            UpdateButtonStatus(UIInputButton.Cancel);
            UpdateButtonStatus(UIInputButton.Function01);
            UpdateButtonStatus(UIInputButton.Function02);
            UpdateButtonStatus(UIInputButton.Function03);
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
            _inputData.SetButtonStatus(UIInputButton.Accept, value.isPressed ? UIInputStatus.Press : UIInputStatus.Release);
        }

        private void OnCancel(InputValue value)
        {
            _inputData.SetButtonStatus(UIInputButton.Cancel, value.isPressed ? UIInputStatus.Press : UIInputStatus.Release);
        }

        private void OnFunction01(InputValue value)
        {
            _inputData.SetButtonStatus(UIInputButton.Function01, value.isPressed ? UIInputStatus.Press : UIInputStatus.Release);
        }

        private void OnFunction02(InputValue value)
        {
            _inputData.SetButtonStatus(UIInputButton.Function02, value.isPressed ? UIInputStatus.Press : UIInputStatus.Release);
        }

        private void OnFunction03(InputValue value)
        {
            _inputData.SetButtonStatus(UIInputButton.Function03, value.isPressed ? UIInputStatus.Press : UIInputStatus.Release);
        }

        private void OnMovement(InputValue value)
        {
            _inputData.Axis01 = value.Get<Vector2>().normalized;
        }

        private void UpdateMovement()
        {
            // mouse input
            if (_inputData.Axis01.sqrMagnitude > 0.1f)
            {
                // If external device input is pressed don't use mouse
                movementController.UpdateMovement(_inputData.Axis01);
            }
            else if (Input.GetMouseButton(0)
                && !EventSystem.current.IsPointerOverGameObject())
            {
                Vector2 move = Input.mousePosition - Camera.main.WorldToScreenPoint(movementController.transform.position);

                if (move.sqrMagnitude > 0.5f)
                    move.Normalize();
                else
                    move = Vector2.zero;

                movementController.UpdateMovement(move);
            }
            else
            {
                movementController.UpdateMovement(Vector2.zero);
            }
        }

        private void UpdateButtonStatus(UIInputButton button)
        {
            if (_inputData.GetButtonStatus(button) == UIInputStatus.Press)
                _inputData.SetButtonStatus(button, UIInputStatus.Hold);
            else if (_inputData.GetButtonStatus(button) == UIInputStatus.Release)
                _inputData.SetButtonStatus(button, UIInputStatus.None);
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

        #region GameOver

        private void GameOver()
        {
            DoAction(UIAction.Open_GameOverScreen);
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