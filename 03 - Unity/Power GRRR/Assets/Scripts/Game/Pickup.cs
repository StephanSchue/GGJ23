using UnityEngine;
using UnityEngine.Events;

namespace GGJ23.Game
{
    public enum PickupType
    {
        Boost
    }

    public enum PickupStatus
    {
        On,
        Off
    }

    public class Pickup : MonoBehaviour
    {
        public PickupType type;

        public float interactionRadius = 1f;
        public float cooldownDuration = 10f;

        private PickupStatus _status;
        private float _cooldownTimer = 0f;
        private bool _isNight = false;

        public PickupStatus Status => _status;
        public bool IsNight => _isNight;

        private void Awake()
        {
            
        }

        public void Initialize()
        {
            _status = PickupStatus.On;
        }

        public void RegisterEvents(UnityEvent onDaySwitch, UnityEvent onNightSwitch)
        {
            onDaySwitch.AddListener(OnDaySwitch);
            onNightSwitch.AddListener(OnNightSwitch);
        }

        public void Process(float dt)
        {
            if (_status == PickupStatus.Off)
            {
                if (_cooldownTimer < 0f)
                {
                    _status = PickupStatus.On;
                }
                else
                {
                    _cooldownTimer -= dt;
                }
            }
        }

        public void Apply()
        {
            if (_status != PickupStatus.On)
                return;

            _status = PickupStatus.Off;
            _cooldownTimer = cooldownDuration;
        }

        #region Events

        private void OnDaySwitch()
        {
            // Debug.Log("OnDaySwitch");
            _isNight = false;
        }

        private void OnNightSwitch()
        {
            // Debug.Log("OnNightSwitch");
            _isNight = true;
        }

        #endregion

        #region Editor

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
        }

        #endregion
    }
}