using GGJ23.Game;
using GGJ23.Managment;
using UnityEngine;
using UnityEngine.EventSystems;

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
        Exit_Game
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
                StartScreen?.Init(uiController);
                GameScreen?.Init(uiController);
                GameOverScreen?.Init(uiController);
                PauseScreen?.Init(uiController);
                HelpScreen?.Init(uiController);
                OptionScreen?.Init(uiController);
                CreditsScreen?.Init(uiController);
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

        public UIScreenCollection screens;
        private UIState _currentState;
        private UIScreen _currentScreen => screens.GetCurrentScreen(_currentState);
        private UIInputData _inputData = new UIInputData();

        private void Awake()
        {
            screens.Init(this);
            SwitchState(UIState.StartScreen);
        }

        private void OnDisable()
        {
            
        }

        private void Update()
        {
            _inputData.SetButtonStatus(UIInputButton.Accept, Input.GetButtonDown("Submit"), Input.GetButton("Submit"), Input.GetButtonUp("Submit"));
            _inputData.SetButtonStatus(UIInputButton.Cancel, Input.GetButtonDown("Cancel"), Input.GetButton("Cancel"), Input.GetButtonUp("Cancel"));

            _currentScreen?.Tick(Time.deltaTime, _inputData);
        }

        #region Statemachine

        public void DoAction(UIAction uiAction)
        {
            Debug.Log($"UIController::DoAction {uiAction}");

            switch (uiAction)
            {
                case UIAction.Open_StartScreen:
                    break;
                case UIAction.Start_Game:
                    gameManager.StartGame();
                    break;
                case UIAction.Open_GameOverScreen:
                    break;
                case UIAction.Open_PauseScreen:
                    break;
                case UIAction.Open_HelpScreen:
                    break;
                case UIAction.Open_OptionScreen:
                    break;
                case UIAction.Open_CreditsScreen:
                    break;
                case UIAction.Exit_Game:
                    gameManager.Exit();
                    break;
            }
        }

        public void SwitchState(UIState newState)
        {
            _currentScreen?.Exit();
            _currentState = newState;
            _currentScreen.Enter();
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