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
        private InteractionStatus _status;
        private float _progress = 0f;
        public float _duration = 2f;
        public float _interactionRadius = 1f;

        public InteractionStatus Status => _status;
        public float Progress => _progress;

        private bool _isNight = false;

        public bool IsNight => _isNight;

        public bool IsConnected { get; private set; } = true;

        private void Awake()
        {
            Initalize();
        }

        public void Initalize()
        {
            _status = InteractionStatus.Working;
            _progress = _duration;
        }

        public void Process(float dt)
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