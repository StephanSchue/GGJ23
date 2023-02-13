using UnityEngine;

namespace GGJ23.Game.Config
{
    [CreateAssetMenu(fileName = "InteractableConfig_Default", menuName = "ScriptableObjects/InteractableConfig", order = 1)]
    public class InteractableConfig : ScriptableObject
    {
        [SerializeField]
        private float _duration = 2f;
        [SerializeField]
        private float _interactionRadius = 1f;

        public float Duration => _duration;
        public float InteractionRadius => _interactionRadius;
    }
}