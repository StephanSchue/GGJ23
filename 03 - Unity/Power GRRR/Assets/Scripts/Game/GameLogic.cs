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
        private float _lossPoints = 0f;
        private float _currentTime = 1f; // 1 = first day, 1.5 = first night, 2 = second day etc.

        [Header("Events")]
        public UnityEvent OnDaySwitch;
        public UnityEvent OnNightSwitch;

        [Header("Design Tuning")]
        [Range(0f, 500f)] [SerializeField] private float _lossPointsNeeded = 100;
        [Range(5f, 30f)][SerializeField] private float _maxDifficulty = 15f;
        [Range(2.5f, 20f)][SerializeField] private float _timeToMaxDifficulty = 10f;
        [Range(1f, 50f)][SerializeField] private float _maxDifficultyFromDistance = 15f;
        [Range(200f, 2000f)][SerializeField] private float _distanceToMaxDifficulty = 500f;
        [Range(0.1f, 1.5f)][SerializeField] private float _difficultyFactorFromMultipleBreakages = 0.235f;

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

            // GenerateBrokenInteractables();
            OnDaySwitch.Invoke();
        }

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
                    //TODO hook this up properly
                    foreach (var inter in GenerateBrokenInteractables())
                    {
                        inter.Break();
                    }

                    GridLightController.RefreshInteractableStatus(InteractionController.Interactables);
                }
                else
                {
                    OnDaySwitch.Invoke();
                }
            }

            UpdatePoints(Time.deltaTime);
        }

        private void UpdatePoints(float deltaTime)
        {
            // Track two scores simultaneously - the actual points and the progress to game loss.
            // Gain points for just surviving.
            // Gain progress towards game loss if there are broken buildings. The rate increases if the buildings have been broken for long.
            // Undo progress towards game loss if there is a prolonged period of all buildings being fixed.

            _currentScore += deltaTime;

            bool allGood = true;
            if (InteractionController != null)
            {
                foreach (var inter in InteractionController.Interactables)
                {
                    if (inter.Status == InteractionStatus.Broken || inter.Status == InteractionStatus.BeingRepaired)
                    {
                        allGood = false;
                        _lossPoints += deltaTime;
                    }
                }

                if (allGood)
                {
                    _lossPoints -= deltaTime;
                }
            }

            if (_lossPoints >= _lossPointsNeeded)
            {
                // End the game
                Debug.LogError($"You lost the game lmao");
            }

            Debug.Log($"Points:{_currentScore}, LossPoints:{_lossPoints}");
        }

        private List<Interactable> GenerateBrokenInteractables()
        {
            Debug.Log("GENERATE BROKEN INTERACTABLES START");
            List<Interactable> tempInteractables = new();
            List<Interactable> possibleInteractables = new();

            foreach (var inter in InteractionController.Interactables)
            {
                if (inter.Status == InteractionStatus.Working)
                {
                    possibleInteractables.Add(inter);
                }
            }

            if (possibleInteractables.Count < 1)
            {
                return tempInteractables;
            }

            float difficultyFactor = Mathf.Lerp(1f, _maxDifficulty, (Mathf.InverseLerp(1f, _timeToMaxDifficulty, Mathf.Clamp(_currentTime, 1f, _timeToMaxDifficulty))));
            Debug.Log($"difficultyFactor to begin with: {difficultyFactor}");

            while (difficultyFactor >= 1f)
            {
                List<Interactable> tempPossibles = new(possibleInteractables);
                while(true)
                {
                    if (tempPossibles.Count < 1)
                    {
                        Debug.Log("GENERATE BROKEN INTERACTABLES END NR 1");
                        return tempInteractables;
                    }

                    // Get random possible interactable
                    Interactable candidate = tempPossibles[Random.Range(0, tempPossibles.Count -1)];
                    Debug.Log($"candidate: {candidate.gameObject.name}");
                    float distance = 0f;

                    if (tempInteractables.Count > 0)
                    {
                        distance = GetMinDistance(tempInteractables, candidate);
                    }

                    float difficultyFromDistance = Mathf.Lerp(0f, _maxDifficultyFromDistance, Mathf.InverseLerp(0f, _distanceToMaxDifficulty, distance));
                    float difficultyFromNumberOfInteractables = Mathf.Pow(tempInteractables.Count + 1, _difficultyFactorFromMultipleBreakages);

                    Debug.Log($"distance: {distance}");
                    Debug.Log($"difficultyFromDistance: {difficultyFromDistance}");
                    Debug.Log($"difficultyFromNumberOfInteractables: {difficultyFromNumberOfInteractables}");
                    Debug.Log($"total difficulty for this candidate: {difficultyFromDistance + difficultyFromNumberOfInteractables}");

                    if (difficultyFactor - (difficultyFromDistance + difficultyFromNumberOfInteractables) > -0.5f)
                    {
                        tempInteractables.Add(candidate);
                        possibleInteractables.Remove(candidate);
                        difficultyFactor -= difficultyFromDistance + difficultyFromNumberOfInteractables;
                        
                        Debug.Log($"Added valid candidate: {candidate.gameObject.name}");
                        Debug.Log($"leftover difficultyFactor: {difficultyFactor}");

                        break;
                    }
                    else
                    {
                        Debug.Log($"Removing invalid candidate, was too difficult: {candidate.gameObject.name}");
                        tempPossibles.Remove(candidate);
                    }
                }
            }


            Debug.Log("GENERATE BROKEN INTERACTABLES END");
            return tempInteractables;
        }

        private float GetMinDistance(List<Interactable> list, Interactable candidate)
        {
            float tempDistance = float.MaxValue;

            foreach (var inter in list)
            {
                tempDistance = Mathf.Min(tempDistance, Vector2.Distance(candidate.transform.position, inter.transform.position));
            }

            return tempDistance;
        }
    }
}