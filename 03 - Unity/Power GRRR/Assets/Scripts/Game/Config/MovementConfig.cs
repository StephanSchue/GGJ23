using UnityEngine;

namespace GGJ23.Game.Config
{
    [CreateAssetMenu(fileName = "MovementConfig_Default", menuName = "ScriptableObjects/MovementConfig", order = 1)]
    public class MovementConfig : ScriptableObject
    {
        [SerializeField]
        private float _speed = 1f;
        [SerializeField]
        private float _boostInterval = 2f;
        [SerializeField]
        private float _boostDuration = 2f;
        [SerializeField]
        private float _boostSpeed = 1.5f;
        [SerializeField]
        private bool _stopAtNight = false;
        [SerializeField]
        private int _maxBoostPickups = 1;

        public float Speed => _speed;
        public float BoostInterval => _boostInterval;
        public float BoostThreshold => _boostInterval;
        public float BoostDuration => _boostDuration;
        public float BoostSpeed => _boostSpeed;
        public bool StopAtNight => _stopAtNight;
        public int MaxBoostPickups => _maxBoostPickups;
    }
}