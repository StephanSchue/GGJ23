using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        private void Awake()
        {
            _interactionController = new();
            _interactionController.PopulateInteractables(FindObjectsByType<Interactable>(FindObjectsSortMode.None));
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

            float progressToNextTime = ((Time.deltaTime * 1000) / (_isNight ? _nightDurationMs : _dayDurationMs)) * 0.5f;

            _currentTime += progressToNextTime;
            _isNight = _currentTime % 1f >= 0.5f;
        }
    }
}