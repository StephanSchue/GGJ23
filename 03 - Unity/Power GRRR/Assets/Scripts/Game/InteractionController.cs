using GGJ23.Game.Config;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ23.Game
{
    public class InteractionController : MonoBehaviour
    {
        // --- Variables ---
        public InteractionConfig config;
        
        private Interactable[] _interactables;
        private Interactable _nearestInteractable;

        private bool _isWorking = false;
        private bool _interactButtonPressed = false;

        private bool _blockInput = false;

        // --- Events ---
        public UnityEvent OnFixStart;
        public UnityEvent OnFixOnFixAbourt;
        public UnityEvent OnFixOnFixComplete;

        // --- Properties ---
        public Interactable[] Interactables { get => _interactables; }

        public bool IsWorking => _isWorking;

        public bool IsNight { get { return _interactables.Length > 0 ? _interactables[0].IsNight : false; } }


        #region Public methods
        public void PopulateInteractables(Interactable[] interactables, UnityEvent onDaySwitch, UnityEvent onNightSwitch)
        {
            _interactables = interactables;

            for(int i = 0; i < _interactables.Length; i++)
            {
                _interactables[i].RegisterEvents(onDaySwitch, onNightSwitch);
            }
        }

        public void RegisterEvents(EffectContoller effectContoller)
        {
            OnFixStart.AddListener(() => effectContoller.OnFixStart.Invoke());
            OnFixOnFixAbourt.AddListener(() => effectContoller.OnFixOnFixAbourt.Invoke());
            OnFixOnFixComplete.AddListener(() => effectContoller.OnFixOnFixComplete.Invoke());
        }

        #endregion

        public void PressInteractButton()
        {
            _interactButtonPressed = true;
        }

        public void LeaveInteractButton()
        {
            _interactButtonPressed = false;
        }

        public void BlockInput(bool block)
        {
            _blockInput = block;
        }

        private void Update()
        {
            // Interate trough all registered Interactables
            for (int i = 0; i < _interactables.Length; i++)
            {
                _interactables[i].Process(Time.deltaTime);
            }

            if(_blockInput)
            {
                _interactButtonPressed = false;
                return;
            }

            // Iterate through Input on nearest Interactble
            if (Input.GetButton("Fire1"))
                _interactButtonPressed = true;

            if (_interactButtonPressed)
            {
                // Check for nearest Interactable
                float nearestDistance = float.MaxValue;
                _nearestInteractable = null;

                for (int i = 0; i < _interactables.Length; i++)
                {
                    float tmpDistance = Vector2.Distance(transform.position + (Vector3)config.InteractionOffset, _interactables[i].transform.position);

                    if (tmpDistance < (config.InteractionRadius + _interactables[i].InteractionRadius)
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
                    _nearestInteractable.ProcessInteraction(Time.deltaTime);

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

            _interactButtonPressed = false;
        }

        #region Editor

        private void OnDrawGizmos()
        {
            if (config != null)
                Gizmos.DrawWireSphere(transform.position + (Vector3)config.InteractionOffset, config.InteractionRadius);
        }

        #endregion
    }
}
