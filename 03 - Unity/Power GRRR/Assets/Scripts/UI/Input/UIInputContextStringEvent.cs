using GGJ23.Game;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ23.UI
{
    [System.Serializable]
    public class InputContextStringEvent : UnityEvent<string> { }

    public class UIInputContextStringEvent : UIInputContextEvent
    {
        [SerializeField]
        private InputContextStringEvent _onContextChanged;

        public override void OnInputDeviceChange(InputControlSchema inputControl) 
        {
            var label = contextTable.GetContextLabel(inputControl, inputButton);
            _onContextChanged.Invoke(label);
        }
    }
}