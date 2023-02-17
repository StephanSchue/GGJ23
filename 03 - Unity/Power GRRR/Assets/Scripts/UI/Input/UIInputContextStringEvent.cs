using UnityEngine;
using UnityEngine.Events;

namespace GGJ23.UI
{
    [System.Serializable]
    public class InputContextStringEvent : UnityEvent<string> { }

    public class UIInputContextStringEvent : MonoBehaviour
    {
        public UIInputButton inputButton = UIInputButton.Accept;

        [SerializeField]
        private InputContextStringEvent _onContextChanged;
    }
}