using UnityEngine;

namespace GGJ23.UI
{
    [CreateAssetMenu(fileName = "UIActionObject", menuName = "ScriptableObjects/UIActionObject", order = 4)]
    public class UIActionObject : ScriptableObject
    {
        [SerializeField]
        private UIAction _action;

        public UIAction Action => _action;
    }
}