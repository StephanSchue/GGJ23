using GGJ23.Game.Config;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ23.Game
{
    public enum InteractionStatus
    {
        Working,
        Broken,
        BeingRepaired,
        FreshlyRepaired
    }

    public class Interactable : MonoBehaviour
    {
        // --- Variables ---
        [Header("Settings")]
        public InteractableConfig config;

        private InteractionStatus _status;
        private float _progress = 0f;
        private bool _isNight = false;

        private float _timeToBecomeBreakable = 10000f;
        private float _breakableTimer = 0f;

        // --- Properties ---
        public InteractionStatus Status => _status;
        public float InteractionRadius => config.InteractionRadius;
        public float Progress => 1f - Mathf.Lerp(0f, 1f, Mathf.InverseLerp(0f, config.Duration, _progress));
        public bool IsNight => _isNight;

        #region Init

        private void Awake()
        {
            Initalize();
        }

        #endregion

        #region Public methods

        public void Initalize()
        {
            _status = InteractionStatus.Working;
            _progress = config.Duration;
        }

        public void RegisterEvents(UnityEvent onDaySwitch, UnityEvent onNightSwitch)
        {
            onDaySwitch.AddListener(OnDaySwitch);
            onNightSwitch.AddListener(OnNightSwitch);
        }

        public void Process(float dt)
        {
            if (_status == InteractionStatus.FreshlyRepaired)
            {
                _breakableTimer += dt * 1000;
                if (_breakableTimer >= _timeToBecomeBreakable)
                {
                    _breakableTimer = 0f;
                    _progress = config.Duration;
                    _status = InteractionStatus.Working;
                }
            }
        }

        public void ProcessInteraction(float dt)
        {
            if (_status == InteractionStatus.Broken || _status == InteractionStatus.BeingRepaired)
            {
                if (_progress < 0f)
                {
                    _progress = 0f;
                    _status = InteractionStatus.FreshlyRepaired;
                    return;
                }

                _progress -= dt;
                _status = InteractionStatus.BeingRepaired;
            }
        }

        public void Break()
        {
            _status = InteractionStatus.Broken;
        }

        #endregion

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
            if(config != null)
            {
                Gizmos.DrawWireSphere(transform.position, config.InteractionRadius);
            }
        }

        #endregion
    }
}