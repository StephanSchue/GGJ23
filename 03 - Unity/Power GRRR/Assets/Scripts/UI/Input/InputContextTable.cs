using UnityEngine;

namespace GGJ23.UI
{
    [CreateAssetMenu(fileName = "InputContextTable", menuName = "ScriptableObjects/InputContextTable", order = 5)]
    public class InputContextTable : ScriptableObject
    {
        [System.Serializable]
        public struct InputContextTableEntry
        {
            [SerializeField] private string _device_xbox;
            [SerializeField] private UIInputButton _button;
            [SerializeField] private string _device_playstation;
            [SerializeField] private string _device_mouse_keyboard;
            [SerializeField] private string _device_touch;
        }

        [SerializeField]
        private InputContextTableEntry[] _entries;
    }
}