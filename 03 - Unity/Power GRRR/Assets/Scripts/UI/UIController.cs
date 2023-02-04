using UnityEngine;
using UnityEngine.UIElements;

namespace GGJ23.UI
{
    public class UIController : MonoBehaviour
    {
        [Header("Components")]
        public GameManager gameManager;
        public UIDocument uiDocument;

        private VisualElement _root;

        private VisualElement _helpContainer;
        private VisualElement _pauseContainer;

        private ProgressBar _progressbarEnergy;
        private Button _buttonPause;
        private Button _buttonPauseExit;
        private Button _buttonHelp;
        private Button _buttonHelpExit;
        private Button _buttonRestart;
        private Button _buttonExit;

        private bool _focused = false;
        private bool _helpOpen = false;

        private void OnEnable()
        {
            _root = uiDocument.rootVisualElement;

            if (_root == null)
                return;

            // --- Progressbar ---
            _progressbarEnergy = _root.Q<ProgressBar>("ProgressbarEnergy");

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

            // --- Containers ---
            _pauseContainer = _root.Q<VisualElement>("ContainerPause");
            _helpContainer = _root.Q<VisualElement>("ContainerHelp");
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

            if (Input.GetButtonDown("Cancel"))
            {
                if(_helpOpen)
                {
                    OnClickHelpExit();
                }
                else
                {
                    TogglePause();
                } 
            }
        }

        private void TogglePause()
        {
            _focused = !_focused;
            OnClickPause(_focused);
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
            gameManager.Restart();
        }

        private void OnClickExit()
        {
            Debug.Log("Exit");
            gameManager.Exit();
        }
    }
}