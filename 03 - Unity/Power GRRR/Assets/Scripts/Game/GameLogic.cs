using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ23.Game
{
    public class GameLogic : MonoBehaviour
    {
        [SerializeField] private int _dayDurationMs = 60000;
        [SerializeField] private int _nightDurationMs = 10000;

        private InteractionController _interactionController;
        private bool _isNight = false;
        private float _currentScore = 0f;
        private float _currentTime = 1f; // 1 = first day, 1.5 = first night, 2 = second day etc.

        [Header("Events")]
        public UnityEvent OnDaySwitch;
        public UnityEvent OnNightSwitch;

        public bool IsNight => _isNight;

        private void Awake()
        {
            _interactionController = new();
            _interactionController.PopulateInteractables(FindObjectsByType<Interactable>(FindObjectsSortMode.None), OnDaySwitch, OnNightSwitch);
        }

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.deltaTime == 0f)
            {
                return;
            }

            bool lastIsNight = _isNight;

            float progressToNextTime = ((_isNight ? _nightDurationMs : _dayDurationMs) / (Time.deltaTime * 1000)) * 0.5f;
            _currentTime += progressToNextTime;
            _isNight = _currentTime % 1f >= 0.5f;

            if(lastIsNight != _isNight)
            {
                if(_isNight)
                {
                    OnNightSwitch.Invoke();
                }
                else
                {
                    OnDaySwitch.Invoke();
                }
            }

            Debug.Log($"Current time: {_currentTime}.\nIsNight: {_isNight.ToString()}");
        }
    }
}