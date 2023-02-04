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

        public InteractionController InteractionController;
        public GridLightController GridLightController;
        public CameraController CameraController;

        private bool _isNight = false;
        private float _currentScore = 0f;
        private float _currentTime = 1f; // 1 = first day, 1.5 = first night, 2 = second day etc.

        [Header("Events")]
        public UnityEvent OnDaySwitch;
        public UnityEvent OnNightSwitch;

        public bool IsNight => _isNight;
        
        // Start is called before the first frame update
        void Start()
        {
            var interactables = FindObjectsByType<Interactable>(FindObjectsSortMode.None);

            if (InteractionController != null)
                InteractionController.PopulateInteractables(interactables, OnDaySwitch, OnNightSwitch);

            if (GridLightController != null)
                GridLightController.PopulateInteractables(interactables, OnDaySwitch, OnNightSwitch);

            if (CameraController != null)
                CameraController.Populate(OnDaySwitch, OnNightSwitch);
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.deltaTime == 0f)
            {
                return;
            }

            float progressToNextTime = ((Time.deltaTime * 1000) / (_isNight ? _nightDurationMs : _dayDurationMs)) * 0.5f;
            bool lastIsNight = _isNight;

            _currentTime += progressToNextTime;
            _isNight = _currentTime % 1f >= 0.5f;

            if (lastIsNight != _isNight)
            {
                if (_isNight)
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

        private List<Interactable> GenerateBrokenInteractables()
        {
            List<Interactable> tempInteractables = new();
            List<Interactable> possibleInteractables = new();

            foreach (var inter in InteractionController.Interactables)
            {
                if (inter.Status == InteractionStatus.Free)
                {
                    possibleInteractables.Add(inter);
                }
            }

            float difficultyFactor = Mathf.Lerp(1f, 15f, (Mathf.InverseLerp(1f, 10f, Mathf.Clamp(_currentTime, 1f, 10f))));
            Debug.Log(difficultyFactor);

            while (difficultyFactor > 0.5f)
            {
                List<Interactable> tempPossibles = new(possibleInteractables);
                while(true)
                {
                    // Get random possible interactable
                    Interactable candidate = tempPossibles[Random.Range(0, tempPossibles.Count -1)];
                    float distance = 0f;

                    if (tempInteractables.Count != 0)
                    {
                        // distance = GetMinDistance(tempInteractables, candidate);
                    }

                    float difficultyFromDistance = Mathf.Lerp(0f, 2f, Mathf.InverseLerp(0f, 1000f, distance));
                    float difficultyFromNumberOfInteractables = Mathf.Pow(tempInteractables.Count + 1, 1.035f);

                    if (difficultyFactor > difficultyFromDistance + difficultyFromNumberOfInteractables)
                    {
                        tempInteractables.Add(candidate);
                        possibleInteractables.Remove(candidate);
                        difficultyFactor -= difficultyFromDistance + difficultyFromNumberOfInteractables;
                        break;
                    }
                }
            }

            return tempInteractables;
        }
    }
}