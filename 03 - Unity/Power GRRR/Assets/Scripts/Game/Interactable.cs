using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
        private InteractionStatus _status;
        private float _progress = 0f;
        public float _duration = 2f;
        public float _interactionRadius = 1f;

        public Slider progressBar;

        public InteractionStatus Status => _status;
        public float Progress => _progress;

        private bool _isNight = false;

        public bool IsNight => _isNight;

        public bool IsConnected { get; private set; } = true;

        private float _timeToBecomeBreakable = 10000f;
        private float _breakableTimer = 0f;

        private void Awake()
        {
            Initalize();
        }

        public void Initalize()
        {
            _status = InteractionStatus.Working;
            _progress = _duration;
        }

        void Update()
        {
            if (_status == InteractionStatus.FreshlyRepaired)
            {
                _breakableTimer += Time.deltaTime * 1000;
                if (_breakableTimer >= _timeToBecomeBreakable)
                {
                    _breakableTimer = 0f;
                    _progress = _duration;
                    _status = InteractionStatus.Working;
                }
            }
        }

        public void Process(float dt)
        {
            if (_status == InteractionStatus.Broken || _status == InteractionStatus.BeingRepaired)
            {
                if (_progress < 0f)
                {
                    _progress = 0f;
                    _status = InteractionStatus.FreshlyRepaired;
                    progressBar.gameObject.SetActive(false);
                    progressBar.value = 0f;
                    return;
                }

                _progress -= dt;
                _status = InteractionStatus.BeingRepaired;
                progressBar.gameObject.SetActive(true);
                progressBar.value = 1f - Mathf.Lerp(0f, 1f, Mathf.InverseLerp(0f, _duration, _progress));
            }
        }

        public void OnDaySwitch()
        {
            // Debug.Log("OnDaySwitch");
            _isNight = false;
        }

        public void OnNightSwitch()
        {
            // Debug.Log("OnNightSwitch");
            _isNight = true;
        }

        public void Break()
        {
            _status = InteractionStatus.Broken;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _interactionRadius);
        }
    }
}