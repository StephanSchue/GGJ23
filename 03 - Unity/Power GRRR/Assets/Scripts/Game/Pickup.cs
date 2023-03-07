using GGJ23.Game.Config;
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
        public PickupConfig config;

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
                else if(!_isNight)
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
            _cooldownTimer = config.CooldownDuration;
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
            if (config != null)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, config.InteractionRadius);
            }
        }

        #endregion
    }
}