using GGJ23.Game.Config;
using System.Collections.Generic;
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
        [System.Serializable]
        public class Puzzle
        {
            public const int PuzzleMaxNumber = 4;

            public int[] buttons;
            public int index;
            public int maxNumber;

            public void Generate(int buttonNumber)
            {
                maxNumber = buttonNumber;
                buttons = new int[maxNumber];

                int currentNumber = 0;

                for (int i = 0; i < buttons.Length; i++)
                {
                    if(currentNumber < PuzzleMaxNumber)
                    {
                        buttons[i] = currentNumber;
                        ++currentNumber;
                    }
                    else
                    {
                        currentNumber = 0;
                        buttons[i] = i;
                    }
                }

                Shuffle(buttons);
            }

            public bool Evaluate(int input, out bool puzzleComplete)
            {
                if(buttons[index] == input)
                {
                    if(index + 1 < maxNumber)
                    {
                        // in progress
                        ++index;
                        puzzleComplete = false;
                    }
                    else
                    {
                        // complete
                        puzzleComplete = true;
                    }

                    return true;
                }
                else
                {
                    // failed
                    index = 0;
                    puzzleComplete = false;
                    return false;
                }
            }

            // --- Shuffle ---

            private static System.Random rng = new System.Random();

            public static void Shuffle<T>(IList<T> list)
            {
                int n = list.Count;
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    T value = list[k];
                    list[k] = list[n];
                    list[n] = value;
                }
            }
        }

        // --- Variables ---
        [Header("Settings")]
        public InteractableConfig config;

        public UnityEvent OnInitialize;

        public Puzzle puzzle = new Puzzle();
        public bool brokenOnStart = false;

        private InteractionStatus _status;
        private float _progress = 0f;
        private bool _isNight = false;

        private float _timeToBecomeBreakable = 10000f;
        private float _breakableTimer = 0f;



        // --- Properties ---
        public InteractionStatus Status => _status;
        public float InteractionRadius => config.InteractionRadius;
        public float Progress => 1f - Mathf.Lerp(0f, 1f, Mathf.InverseLerp(0f, config.Duration, _progress));
        public bool IsNight => _isNight;

        #region Init

        private void Awake()
        {
            Initalize();
        }

        #endregion

        #region Public methods

        public void Initalize()
        {
            _status = InteractionStatus.Working;
            _progress = config.Duration;

            if (brokenOnStart)
            {
                Break();
            }

            if (OnInitialize != null) { OnInitialize.Invoke(); }
        }

        public void RegisterEvents(UnityEvent onDaySwitch, UnityEvent onNightSwitch)
        {
            onDaySwitch.AddListener(OnDaySwitch);
            onNightSwitch.AddListener(OnNightSwitch);
        }

        public void Process(float dt)
        {
            if (_status == InteractionStatus.FreshlyRepaired)
            {
                _breakableTimer += dt * 1000;
                if (_breakableTimer >= _timeToBecomeBreakable)
                {
                    _breakableTimer = 0f;
                    _progress = config.Duration;
                    _status = InteractionStatus.Working;
                }
            }
        }

        public void ProcessInteraction(float dt)
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

        public void Break()
        {
            _status = InteractionStatus.Broken;
            puzzle.Generate(4);
        }

        #endregion

        #region Events

        private void OnDaySwitch()
        {
            // Debug.Log("OnDaySwitch");
            _isNight = false;
        }

        private void OnNightSwitch()
        {
            // Debug.Log("OnNightSwitch");
            _isNight = true;
        }

        #endregion

        #region Editor

        private void OnDrawGizmos()
        {
            if(config != null)
            {
                Gizmos.DrawWireSphere(transform.position, config.InteractionRadius);
            }
        }

        #endregion
    }
}