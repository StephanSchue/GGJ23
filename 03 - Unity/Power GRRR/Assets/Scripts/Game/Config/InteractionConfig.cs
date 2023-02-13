using UnityEngine;

namespace GGJ23.Game.Config
{
    [CreateAssetMenu(fileName = "InteractionConfig_Default", menuName = "ScriptableObjects/InteractionConfig", order = 1)]
    public class InteractionConfig : ScriptableObject
    {
        [SerializeField]
        private float _interactionRadius = 1f;
        [SerializeField]
        private Vector2 _interactionOffset = Vector2.zero;

        public float InteractionRadius => _interactionRadius;
        public Vector2 InteractionOffset => _interactionOffset;
    }
}