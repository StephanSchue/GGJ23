using UnityEngine;

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
        public InteractionStatus _status;
        public SpriteRenderer sprite;
        public float _progress = 0f;
        public float _duration = 2f;
        public float _interactionRadius = 1f;

        private void Awake()
        {
            Initalize();
        }

        public void Initalize()
        {
            _status = InteractionStatus.Free;
            sprite.color = Color.red;
            _progress = _duration;
        }

        public void Process(float dt)
        {
            if (_status < InteractionStatus.Finished)
            {
                if (_progress < 0f)
                {
                    sprite.color = Color.green;
                    _status = InteractionStatus.Finished;
                    return;
                }

                _progress -= dt;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, _interactionRadius);
        }
    }
}