using UnityEngine;
using UnityEngine.Events;

namespace GGJ23.UI
{
    [System.Serializable]
    public class InputContextSpriteEvent : UnityEvent<Sprite> { }

    public class UIInputContextSpriteEvent : MonoBehaviour
    {
        public UIInputButton inputButton = UIInputButton.Accept;

        [SerializeField]
        private InputContextSpriteEvent _onContextChanged;
    }
}