using UnityEngine;
using UnityEngine.Events;

namespace GGJ23.Game
{
    public enum InteractionStatus
    {
        Free,
        Progress,
        Finished
    }

    public class Interactable : MonoBehaviour
    {
        private InteractionStatus _status;
        private float _progress = 0f;
        public float _duration = 2f;
        public float _interactionRadius = 1f;

        public InteractionStatus Status => _status;
        public float Progress => _progress;

        private void Awake()
        {
            Initalize();
        }

        public void Initalize()
        {
            _status = InteractionStatus.Free;
            _progress = _duration;
        }

        public void Process(float dt)
        {
            if (_status < InteractionStatus.Finished)
            {
                if (_progress < 0f)
                {
                    _progress = 0f;
                    _status = InteractionStatus.Finished;
                    return;
                }

                _progress -= dt;
                _status = InteractionStatus.Progress;
            }
        }

        public void OnDaySwitch()
        {
            Debug.Log("OnDaySwitch");
        }

        public void OnNightSwitch()
        {
            Debug.Log("OnNightSwitch");
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _interactionRadius);
        }
    }
}