using GGJ23.Game;
using GGJ23.Managment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.Users;
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
        CreditsScreen,
        ControlsScreen
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
        Open_ControlsScreen,
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

        public Vector2 Axis01; // Analogstick X/Y
        public Vector2 Axis02; // DPad

        public void SetButtonStatus(InputButton button, UIInputStatus status)
        {
            Debug.Log($"SetButtonStatus: {button}: {status}");

            switch (button)
            {
                case InputButton.Accept:
                    Accept = status;
                    break;
                case InputButton.Cancel:
                    Cancel = status;
                    break;
                case InputButton.Function01:
                    Function01 = status;
                    break;
                case InputButton.Function02:
                    Function02 = status;
                    break;
                case InputButton.Function03:
                    Function03 = status;
                    break;
                default:
                    break;
            }
        }

        public UIInputStatus GetButtonStatus(InputButton button)
        {
            switch (button)
            {
                case InputButton.Accept:
                    return Accept;
                case InputButton.Cancel:
                    return Cancel;
                case InputButton.Function01:
                    return Function01;
                case InputButton.Function02:
                    return Function02;
                case InputButton.Function03:
                    return Function03;
                default:
                    return UIInputStatus.None;
            }
        }

        public bool IsPressed(InputButton button)
        {
            return GetButtonStatus(button) == UIInputStatus.Press;
        }

        public bool IsHold(InputButton button)
        {
            return GetButtonStatus(button) == UIInputStatus.Hold;
        }

        public bool IsRelease(InputButton button)
        {
            return GetButtonStatus(button) == UIInputStatus.Release;
        }
    }

    public enum Setting
    {
        AudioMusicVolume,
        AudioMusicMute,
        AudioSFXVolume,
        AudioSFXMute,
    }

    public struct Settings
    {
        public int audioMusicVolume;
        public bool audioMusicMute;
        public int audioSFXVolume;
        public bool audioSFXMute;
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
            public UIScreen ControlsScreen;
            public UIScreen OptionScreen;
            public UIScreen CreditsScreen;

            public void Init(UIController uiController)
            {
                StartScreen?.Init(uiController, true);
                GameScreen?.Init(uiController, false);
                GameOverScreen?.Init(uiController, false);
                PauseScreen?.Init(uiController, false);
                HelpScreen?.Init(uiController, false);
                ControlsScreen?.Init(uiController, false);
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
                    case UIState.ControlsScreen:
                        return ControlsScreen;
                    default:
                        return null;
                }
            }
        }

        // --- Variables ---
        [Header("Components")]
        public Canvas canvas;
        public GameManager gameManager;
        public MovementController movementController;
        public InteractionController interactionController;
        public PlayerInput playerInput;
        public EventSystem eventSystem;
        public AudioMixer audioMixer;

        public UIScreenCollection screens;
        public TMPro.TextMeshProUGUI debugText;

        public bool startGameDirect = false;

        private UIState _currentState;
        private UIScreen _currentScreen => screens.GetCurrentScreen(_currentState);
        private UIInputData _inputData = new UIInputData();

        private Settings _settings;

        private bool startGameDirectExecuted = false;

        private UIInputContextEvent[] inputContextEvents;

        // --- Properties ---
        public Settings Settings => _settings;

        public bool Paused => gameManager.Paused;

        private void Awake()
        {
            canvas.enabled = true;
            screens.Init(this);

            inputContextEvents = FindObjectsByType<UIInputContextEvent>(FindObjectsSortMode.None);

            SwitchState(UIState.StartScreen);

            gameManager.OnGameOver.AddListener(GameOver);
        }

        private void Start()
        {
            LoadOptions();
            UpdateControlSchema(InputControlSchema.Keyboard);
        }

        private void OnEnable()
        {
            InputUser.onChange += OnInputDeviceChange;
        }

        private void OnDisable()
        {
            InputUser.onChange -= OnInputDeviceChange;
        }

        private void Update()
        {
            if(startGameDirect && !startGameDirectExecuted)
            {
                SwitchState(UIState.GameScreen);
                gameManager.StartGame();
                startGameDirectExecuted = true;
            }

            _currentScreen?.Tick(Time.deltaTime, _inputData);
            UpdateMovement();

            debugText.text = $"Score: {gameManager.Score.ToString("0.00")}; Energy: {(int)(gameManager.Energy * 100f)}; Boosts: {movementController.BoostCount}"; 
        }

        private void LateUpdate()
        {
            UpdateButtonStatus(InputButton.Accept);
            UpdateButtonStatus(InputButton.Cancel);
            UpdateButtonStatus(InputButton.Function01);
            UpdateButtonStatus(InputButton.Function02);
            UpdateButtonStatus(InputButton.Function03);
        }

        #region Statemachine

        public void DoAction(UIAction uiAction)
        {
            //Debug.Log($"UIController::DoAction {uiAction}");

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
                case UIAction.Open_ControlsScreen:
                    SwitchState(UIState.ControlsScreen);
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

        private void OnInputDeviceChange(InputUser user, InputUserChange change, InputDevice device)
        {
            if (change == InputUserChange.ControlSchemeChanged)
            {  
                InputControlSchema controlSchema = InputControlSchema.Keyboard;
                switch (user.controlScheme.Value.name)
                {
                    case "Keyboard":
                        controlSchema = InputControlSchema.Keyboard;
                        break;
                    case "PlaystationController":
                        controlSchema = InputControlSchema.PlaystationController;
                        break;
                    case "XboxController":
                        controlSchema = InputControlSchema.XboxController;
                        break;
                }

                UpdateControlSchema(controlSchema);
            }
        }

        private void UpdateControlSchema(InputDevice device)
        {
            if (device != null)
            {
                UpdateControlSchema(device is DualShockGamepad ? InputControlSchema.PlaystationController : InputControlSchema.XboxController);
            }
            else
            {
                UpdateControlSchema(InputControlSchema.Keyboard);
            }
        }

        private void UpdateControlSchema(InputControlSchema controlSchema)
        {
            Debug.Log($"UpdateControlSchema: {controlSchema}");

            for (int i = 0; i < inputContextEvents.Length; i++)
            {
                inputContextEvents[i].OnInputDeviceChange(controlSchema);
            }

            if(screens.ControlsScreen is UIControlsScreen controlsScreen) { controlsScreen.OnInputDeviceChange(controlSchema); }
        }
        
        private void OnConfirm(InputValue value)
        {
            _inputData.SetButtonStatus(InputButton.Accept, value.isPressed ? UIInputStatus.Press : UIInputStatus.Release);
        }

        private void OnCancel(InputValue value)
        {
            _inputData.SetButtonStatus(InputButton.Cancel, value.isPressed ? UIInputStatus.Press : UIInputStatus.Release);
        }

        private void OnFunction01(InputValue value)
        {
            _inputData.SetButtonStatus(InputButton.Function01, value.isPressed ? UIInputStatus.Press : UIInputStatus.Release);
        }

        private void OnFunction02(InputValue value)
        {
            _inputData.SetButtonStatus(InputButton.Function02, value.isPressed ? UIInputStatus.Press : UIInputStatus.Release);
        }

        private void OnFunction03(InputValue value)
        {
            _inputData.SetButtonStatus(InputButton.Function03, value.isPressed ? UIInputStatus.Press : UIInputStatus.Release);
        }

        private void OnAxis01(InputValue value)
        {
            _inputData.Axis01 = value.Get<Vector2>().normalized;
        }

        private void OnAxis02(InputValue value)
        {
            _inputData.Axis02 = value.Get<Vector2>().normalized;
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
                && !IsPointerOverUIObject())
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

        private void UpdatePuzzleInput()
        {

        }

        private bool IsPointerOverUIObject()
        {
            //check mouse
            if (EventSystem.current.IsPointerOverGameObject())
                return true;

            //check touch
            if (Input.touchCount > 0 && Input.touches[0].phase == UnityEngine.TouchPhase.Began)
            {
                if (EventSystem.current.IsPointerOverGameObject(Input.touches[0].fingerId))
                    return true;
            }

            return false;
        }

        private void UpdateButtonStatus(InputButton button)
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
            audioMixer.SetFloat("MusicVolume", _settings.audioMusicVolume > 0 && !_settings.audioMusicMute ? Mathf.Log10(_settings.audioMusicVolume / 10f) * 20 : -80f);
            audioMixer.SetFloat("SFXVolume", _settings.audioSFXVolume > 0 && !_settings.audioSFXMute ? Mathf.Log10(_settings.audioSFXVolume / 10f) * 20 : -80f);

            if (_settings.language > -1)
            {
                var locale = LocalizationSettings.AvailableLocales.Locales[Mathf.Clamp(_settings.language, 0, LocalizationSettings.AvailableLocales.Locales.Count)];
                LocalizationSettings.SelectedLocale = locale;
            }
        }

        private void LoadOptions()
        {
            _settings.audioMusicVolume = PlayerPrefs.GetInt("AudioMusicVolume", 10);
            _settings.audioMusicMute = PlayerPrefs.GetInt("AudioMusicMute", 0) == 1;
            _settings.audioSFXVolume = PlayerPrefs.GetInt("AudioSFXVolume", 10);
            _settings.audioSFXMute = PlayerPrefs.GetInt("AudioSFXMute", 0) == 1;
            _settings.language = PlayerPrefs.GetInt("Language", -1);

            ApplyOptions();
        }

        private void SaveOptions()
        {
            PlayerPrefs.SetInt("AudioMusicVolume", _settings.audioMusicVolume);
            PlayerPrefs.SetInt("AudioMusicMute", _settings.audioMusicMute ? 1 : 0);
            PlayerPrefs.SetInt("AudioSFXVolume", _settings.audioSFXVolume);
            PlayerPrefs.SetInt("AudioSFXMute", _settings.audioSFXMute ? 1 : 0);

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
            _settings.audioMusicVolume = 10;
            _settings.audioMusicMute = false;
            _settings.audioSFXVolume = 10;
            _settings.audioSFXMute = false;

            PlayerPrefs.SetInt("AudioMusicVolume", _settings.audioMusicVolume);
            PlayerPrefs.SetInt("AudioMusicMute", _settings.audioMusicMute ? 1 : 0);
            PlayerPrefs.SetInt("AudioSFXVolume", _settings.audioSFXVolume);
            PlayerPrefs.SetInt("AudioSFXMute", _settings.audioSFXMute ? 1 : 0);

            ApplyOptions();

            _currentScreen.Refresh();
        }

        public void UpdateOptionInt(Setting option, int value)
        {
            switch (option)
            {
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

        public void UpdateOptionBool(Setting option, bool value)
        {
            switch (option)
            {
                case Setting.AudioMusicVolume:
                    _settings.audioMusicMute = value;
                    ApplyOptions();
                    break;
                case Setting.AudioSFXVolume:
                    _settings.audioSFXMute = value;
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
    }
}