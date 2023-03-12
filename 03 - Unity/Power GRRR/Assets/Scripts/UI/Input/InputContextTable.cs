using UnityEngine;

namespace GGJ23.Game
{
    public enum InputControlSchema
    {
        Keyboard = 0,
        XboxController,
        PlaystationController,
        WebGLGamepad,
    }

    public enum InputButton
    {
        Accept,
        Cancel,
        Function01,
        Function02,
        Function03,
    }

    [CreateAssetMenu(fileName = "InputContextTable", menuName = "ScriptableObjects/InputContextTable", order = 5)]
    public class InputContextTable : ScriptableObject
    {
        [System.Serializable]
        public struct InputContextEntry
        {
            public string label;
            public Sprite sprite;
        }

        [System.Serializable]
        public struct InputContextTableEntry
        {
            [SerializeField] private InputContextEntry _accept;
            [SerializeField] private InputContextEntry _cancel;
            [SerializeField] private InputContextEntry _function01;
            [SerializeField] private InputContextEntry _function02;
            [SerializeField] private InputContextEntry _function03;

            public string GetLabel(InputButton inputButton)
            {
                switch (inputButton)
                {
                    case InputButton.Accept: return _accept.label;
                    case InputButton.Cancel: return _cancel.label;
                    case InputButton.Function01: return _function01.label;
                    case InputButton.Function02: return _function02.label;
                    case InputButton.Function03: return _function03.label;
                    default: return "";
                }
            }

            public Sprite GetBGSprite(InputButton inputButton)
            {
                switch (inputButton)
                {
                    case InputButton.Accept: return _accept.sprite;
                    case InputButton.Cancel: return _cancel.sprite;
                    case InputButton.Function01: return _function01.sprite;
                    case InputButton.Function02: return _function02.sprite;
                    case InputButton.Function03: return _function03.sprite;
                    default: return null;
                }
            }
        }

        [SerializeField] private InputContextTableEntry _keyboard;
        [SerializeField] private InputContextTableEntry _xBoxController;
        [SerializeField] private InputContextTableEntry _playstationController;

        public Sprite GetContextBGSprite(InputControlSchema controlSchema, InputButton button)
        {
            switch (controlSchema)
            {
                case InputControlSchema.Keyboard: return _keyboard.GetBGSprite(button);
                case InputControlSchema.XboxController: return _xBoxController.GetBGSprite(button);
                case InputControlSchema.PlaystationController: return _playstationController.GetBGSprite(button);
                case InputControlSchema.WebGLGamepad: return _xBoxController.GetBGSprite(button);
                default: return null;
            }
        }

        public string GetContextLabel(InputControlSchema controlSchema, InputButton button)
        {
            switch (controlSchema)
            {
                case InputControlSchema.Keyboard: return _keyboard.GetLabel(button);
                case InputControlSchema.XboxController: return _xBoxController.GetLabel(button);
                case InputControlSchema.PlaystationController: return _playstationController.GetLabel(button);
                case InputControlSchema.WebGLGamepad: return _xBoxController.GetLabel(button);
                default: return null;
            }
        }
    }
}