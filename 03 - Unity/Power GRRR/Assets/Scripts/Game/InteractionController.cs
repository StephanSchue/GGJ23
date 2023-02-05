using UnityEngine;
using UnityEngine.Events;

namespace GGJ23.Game
{
    public class InteractionController : MonoBehaviour
    {
        public float interactionRadius = 1f;
        public Vector2 interactionOffset = Vector2.zero;

        public Interactable[] Interactables {get => _interactables;}
        private Interactable[] _interactables;
        private Interactable _nearestInteractable;

        public UnityEvent OnFixStart;
        public UnityEvent OnFixOnFixAbourt;
        public UnityEvent OnFixOnFixComplete;

        private bool _isWorking = false;

        public bool IsWorking => _isWorking;

        public bool IsNight {get { return _interactables.Length > 0 ? _interactables[0].IsNight : false;}}

        public void PopulateInteractables(Interactable[] interactables, UnityEvent OnDaySwitch, UnityEvent OnNightSwitch)
        {
            _interactables = interactables;

            for(int i = 0; i < _interactables.Length; i++)
            {
                OnDaySwitch.AddListener(_interactables[i].OnDaySwitch);
                OnNightSwitch.AddListener(_interactables[i].OnNightSwitch);
            }
        }

        public void RegisterEvents(EffectContoller effectContoller)
        {
            OnFixStart.AddListener(() => effectContoller.OnFixStart.Invoke());
            OnFixOnFixAbourt.AddListener(() => effectContoller.OnFixOnFixAbourt.Invoke());
            OnFixOnFixComplete.AddListener(() => effectContoller.OnFixOnFixComplete.Invoke());
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
                    float tmpDistance = Vector2.Distance(transform.position + (Vector3)interactionOffset, _interactables[i].transform.position);

                    if (tmpDistance < (interactionRadius + _interactables[i]._interactionRadius)
                        && !_interactables[i].IsNight
                        && (_interactables[i].Status == InteractionStatus.Broken 
                            || _interactables[i].Status == InteractionStatus.BeingRepaired)
                        && tmpDistance < nearestDistance)
                    {
                        _nearestInteractable = _interactables[i];
                        nearestDistance = tmpDistance;
                    }
                }

                // Interact
                if (_nearestInteractable != null)
                {
                    var lastStatus = _nearestInteractable.Status;
                    _nearestInteractable.Process(Time.deltaTime);

                    if (!_isWorking)
                    {
                        OnFixStart.Invoke();
                    }
                    else if(lastStatus != _nearestInteractable.Status
                        && _nearestInteractable.Status == InteractionStatus.FreshlyRepaired)
                    {
                        OnFixOnFixComplete.Invoke();
                    }

                    _isWorking = true;
                }
                else
                {
                    _isWorking = false;
                }
            }
            else
            {
                if (_isWorking
                    && _nearestInteractable != null
                    && (_nearestInteractable.Status == InteractionStatus.Broken 
                        || (_nearestInteractable.Status == InteractionStatus.BeingRepaired)))
                {
                    OnFixOnFixAbourt.Invoke();
                }

                _isWorking = false;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(transform.position + (Vector3)interactionOffset, interactionRadius);
        }
    }
}
