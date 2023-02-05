using GGJ23.Game;
using GGJ23.Managment;
using UnityEngine;
using UnityEngine.UIElements;

namespace GGJ23.UI
{
    public class UIController : MonoBehaviour
    {
        [Header("Components")]
        public GameManager gameManager;
        public MovementController movementController;
        public UIDocument uiDocument;

        private VisualElement _root;

        private VisualElement _helpContainer;
        private VisualElement _pauseContainer;
        private VisualElement _gameOverContainer;

        private ProgressBar _progressbarEnergy;
        private ProgressBar _progressbarBoost;

        private Button _buttonPause;
        private Button _buttonPauseExit;
        private Button _buttonHelp;
        private Button _buttonHelpExit;
        private Button _buttonRestart;
        private Button _buttonExit;

        private Label _valueScore;

        private bool _pauseOpen = false;
        private bool _helpOpen = false;
        private bool _gameOverOpen = false;

        private void OnEnable()
        {
            _root = uiDocument.rootVisualElement;

            gameManager.OnGameOver.AddListener(OnGameOver);

            if (_root == null)
                return;

            // --- Progressbar ---
            _progressbarEnergy = _root.Q<ProgressBar>("ProgressbarEnergy");
            _progressbarBoost = _root.Q<ProgressBar>("ProgressbarBoost");

            // --- Buttons ---
            _buttonPause = _root.Q<Button>("ButtonPause");
            _buttonPauseExit = _root.Q<Button>("ButtonPauseExit");
            _buttonHelp = _root.Q<Button>("ButtonHelp");
            _buttonHelpExit = _root.Q<Button>("ButtonHelpExit");
            _buttonRestart = _root.Q<Button>("ButtonRestart");
            _buttonExit = _root.Q<Button>("ButtonExit");

            if (_buttonPause != null)
            {
                _buttonPause.clicked += TogglePause;
                _buttonPause.focusable = false;
            }

            if (_buttonPauseExit != null)
            {
                _buttonPauseExit.clicked += TogglePause;
                _buttonPauseExit.focusable = false;
            }

            if (_buttonHelp != null)
            {
                _buttonHelp.clicked += OnClickHelp;
                _buttonHelp.focusable = false;
            }

            if (_buttonHelpExit != null)
            {
                _buttonHelpExit.clicked += OnClickHelpExit;
                _buttonHelpExit.focusable = false;
            }

            if (_buttonRestart != null)
            {
                _buttonRestart.clicked += OnClickRestart;
                _buttonRestart.focusable = false;
            }
            
            if (_buttonExit != null)
            {
                _buttonExit.clicked += OnClickExit;
                _buttonExit.focusable = false;
            }

            // --- Value ---
            _valueScore = _root.Q<Label>("ValueScore");

            // --- Containers ---
            _pauseContainer = _root.Q<VisualElement>("ContainerPause");
            _helpContainer = _root.Q<VisualElement>("ContainerHelp");
            _gameOverContainer = _root.Q<VisualElement>("ContainerGameOver");
        }

        private void OnDisable()
        {
            if(_buttonPause != null)
                _buttonPause.clicked += OnClickHelp;

            if (_buttonPauseExit != null)
                _buttonPauseExit.clicked += TogglePause;

            if (_buttonHelp != null)
                _buttonHelp.clicked -= OnClickHelp;

            if (_buttonHelpExit != null)
                _buttonHelpExit.clicked -= OnClickHelpExit;

            if (_buttonRestart != null)
                _buttonRestart.clicked -= OnClickRestart;

            if (_buttonRestart != null)
                _buttonExit.clicked -= OnClickExit;
        }

        private void Update()
        {
            _progressbarEnergy.value = gameManager.Energy;

            if (movementController != null)
            {
                Debug.LogFormat("BoostInterval: {0}; BoostReady: {1}", movementController.BoostIntervalPercentage, movementController.BoostReady);
                _progressbarBoost.value = movementController.BoostIntervalPercentage * 100f;
                _progressbarBoost.title = movementController.BoostActive ? "Boost Active" : movementController.BoostReady ? "Boost Ready" : "Reload Boost";
            }

            if (Input.GetButtonDown("Submit"))
            {
                if (_gameOverOpen)
                {
                    OnGameOverExit();
                }
            }

            if (Input.GetButtonDown("Cancel"))
            {
                if(_gameOverOpen)
                {
                    OnGameOverExit();
                }
                else if(_helpOpen)
                {
                    OnClickHelpExit();
                }
                else
                {
                    TogglePause();
                } 
            }
        }

        private void OnGameOver()
        {
            Debug.Log("GameOver:Enter");
            _gameOverContainer.style.display = DisplayStyle.Flex;
            _valueScore.text = string.Format("{0}", gameManager.Score);
            _gameOverOpen = true;
        }

        private void OnGameOverExit()
        {
            Debug.Log("GameOver:Exit");
            _gameOverContainer.style.display = DisplayStyle.None;
            _gameOverOpen = false;
        }

        private void TogglePause()
        {
            _pauseOpen = !_pauseOpen;
            OnClickPause(_pauseOpen);
        }

        private void OnClickPause(bool active)
        {
            _pauseContainer.style.display = active ? DisplayStyle.Flex : DisplayStyle.None;
            gameManager.Pause(active);

            if (_buttonHelp != null)
            {
                _buttonHelp.focusable = active;
            }

            if (_buttonRestart != null)
            {
                _buttonRestart.focusable = active;
            }

            if (_buttonExit != null)
            {
                _buttonExit.focusable = active;
            }
        }

        private void OnClickHelp()
        {
            Debug.Log("Help:Enter");
            _helpContainer.style.display = DisplayStyle.Flex;
            _helpOpen = true;
        }

        private void OnClickHelpExit()
        {
            Debug.Log("Help:Exit");
            _helpContainer.style.display = DisplayStyle.None;
            _helpOpen = false;
        }

        private void OnClickRestart()
        {
            Debug.Log("Restart");
            OnGameOverExit();
            gameManager.Restart();
        }

        private void OnClickExit()
        {
            Debug.Log("Exit");
            gameManager.Exit();
        }
    }
}