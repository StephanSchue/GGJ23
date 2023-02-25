using GGJ23.Game;
using UnityEngine;

namespace GGJ23.UI
{
    public abstract class UIInputContextEvent : MonoBehaviour
    {
        public InputContextTable contextTable;
        public InputButton inputButton = InputButton.Accept;

        public virtual void OnInputDeviceChange(InputControlSchema inputControl) { }
    }
}