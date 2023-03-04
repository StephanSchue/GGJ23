using UnityEngine;

namespace GGJ23.Game.Config
{
    [CreateAssetMenu(fileName = "PickupConfig_Default", menuName = "ScriptableObjects/PickupConfig", order = 1)]
    public class PickupConfig : ScriptableObject
    {
        [SerializeField]
        private PickupType _type;
        [SerializeField]
        private float _interactionRadius = 1f;
        [SerializeField]
        private float _cooldownDuration = 10f;

        public PickupType Type => _type;
        public float InteractionRadius => _interactionRadius;
        public float CooldownDuration => _cooldownDuration;
    }
}