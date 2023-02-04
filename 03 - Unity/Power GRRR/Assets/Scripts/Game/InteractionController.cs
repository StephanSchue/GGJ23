using UnityEngine;

namespace GGJ23.Game
{
    public class InteractionController : MonoBehaviour
    {
        public float interactionRadius = 1f;
        private Interactable[] _interactables;
        private Interactable _nearestInteractable;

        private void Awake()
        {
            _interactables = FindObjectsByType<Interactable>(FindObjectsSortMode.None);
        }

        private void Update()
        {
            if (Input.GetButton("Fire1"))
            {
                // Check for nearest Interactable
                float nearestDistance = float.MaxValue;
                _nearestInteractable = null;

                for (int i = 0; i < _interactables.Length; i++)
                {
                    float tmpDistance = Vector2.Distance(transform.position, _interactables[i].transform.position);

                    if (tmpDistance < (interactionRadius + _interactables[i]._interactionRadius) && tmpDistance < nearestDistance)
                    {
                        _nearestInteractable = _interactables[i];
                        nearestDistance = tmpDistance;
                    }
                }

                // Interact
                if (_nearestInteractable != null)
                {
                    _nearestInteractable.Process(Time.deltaTime);
                }
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position, interactionRadius);
        }
    }
}
