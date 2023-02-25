using UnityEngine;
using UnityEngine.Events;

namespace GGJ23.UI
{
    [System.Serializable]
    public class InputContextSpriteEvent : UnityEvent<Sprite> { }

    public class UIInputContextSpriteEvent : UIInputContextEvent
    {
        [SerializeField]
        private InputContextSpriteEvent _onContextChanged;

        public override void OnInputDeviceChange(InputControlSchema inputControl)
        {
            var sprite = contextTable.GetContextBGSprite(inputControl, inputButton);
            if(sprite != null) { _onContextChanged.Invoke(sprite); }
        }
    }
}