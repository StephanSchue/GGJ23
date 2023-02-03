using UnityEngine;
using UnityEngine.UIElements;

namespace GGJ23.UI
{
    public class UIController : MonoBehaviour
    {
        public GameManager gameManager;
        public UIDocument uiDocument;

        private VisualElement _root;

        private VisualElement _helpContainer;
    
        private Button _buttonHelp;
        private Button _buttonHelpExit;
        private Button _buttonRestart;
        private Button _buttonExit;

        private void OnEnable()
        {
            _root = uiDocument.rootVisualElement;

            if (_root == null)
                return;

            // --- Buttons ---
            _buttonHelp = _root.Q<Button>("ButtonHelp");
            _buttonHelpExit = _root.Q<Button>("ButtonHelpExit");
            _buttonRestart = _root.Q<Button>("ButtonRestart");
            _buttonExit = _root.Q<Button>("ButtonExit");

            if (_buttonHelp != null)
                _buttonHelp.clicked += OnClickHelp;

            if (_buttonHelpExit != null)
                _buttonHelpExit.clicked += OnClickHelpExit;

            if (_buttonRestart != null)
                _buttonRestart.clicked += OnClickRestart;

            if (_buttonExit != null)
                _buttonExit.clicked += OnClickExit;

            // --- Containers ---
            _helpContainer = _root.Q<VisualElement>("ContainerHelp");
        }

        private void OnDisable()
        {
            if (_buttonHelp != null)
                _buttonHelp.clicked -= OnClickHelp;

            if (_buttonHelpExit != null)
                _buttonHelpExit.clicked -= OnClickHelpExit;

            if (_buttonRestart != null)
                _buttonRestart.clicked -= OnClickRestart;

            if (_buttonRestart != null)
                _buttonExit.clicked -= OnClickExit;
        }

        private void OnClickHelp()
        {
            Debug.Log("Help:Enter");
            _helpContainer.style.display = DisplayStyle.Flex;
        }

        private void OnClickHelpExit()
        {
            Debug.Log("Help:Exit");
            _helpContainer.style.display = DisplayStyle.None;
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