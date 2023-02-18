using GGJ23.Game.Config;
using UnityEngine;
using UnityEngine.Events;

namespace GGJ23.Game
{
    public enum PlayerDirection
    {
        Top = 0,
        Right,
        Bottom,
        Left
    }

    public class MovementController : MonoBehaviour
    {
        public MovementConfig config;

        private Rigidbody2D _rigidbody2D;
        private Vector2 _movement = new Vector2();

        private float _boostInputTimer = 0f;
        private float _boostEffectTimer = 0f;

        private bool _isNight = false;
        private bool _boostButtonPressed = false;

        private bool _blockInput = false;

        public bool IsMoving { get; private set; }
        public Vector2 Direction { get; private set; }
        public PlayerDirection PlayerDirection { get; private set; } = PlayerDirection.Right;
        public float Velocity { get; private set; }

        public float BoostIntervalPercentage => _boostInputTimer / config.BoostInterval;
        public bool BoostReady => _boostInputTimer < config.BoostThreshold;
        public bool BoostActive => _boostEffectTimer > 0f;

        [Header("Events")]
        public UnityEvent OnMovementStart;
        public UnityEvent OnMovementStep;
        public UnityEvent OnMovementStop;
        public UnityEvent OnMovementBoost;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _boostInputTimer = config.BoostInterval;
            _boostEffectTimer = 0f;
        }

        public void RegisterEvents(EffectContoller effectContoller, UnityEvent onDayEvent, UnityEvent onNightEvent)
        {
            OnMovementStart.AddListener(() => effectContoller.OnMovementStart.Invoke());
            OnMovementStep.AddListener(() => effectContoller.OnMovementStep.Invoke());
            OnMovementStop.AddListener(() => effectContoller.OnMovementStop.Invoke());
            OnMovementBoost.AddListener(() => effectContoller.OnMovementBoost.Invoke());

            onDayEvent.AddListener(() => _isNight = false);
            onNightEvent.AddListener(() => _isNight = true);
        }

        public void PressBoost()
        {
            _boostButtonPressed = true;
        }

        public void BlockInput(bool block)
        {
            _blockInput = block;
        }

        private void FixedUpdate()
        {
            if (config.StopAtNight 
                && _isNight)
            {
                if (IsMoving)
                {
                    OnMovementStop.Invoke();
                }

                Velocity = 0f;
                IsMoving = false;

                return;
            }

            _movement.x = Input.GetAxis("Horizontal");
            _movement.y = Input.GetAxis("Vertical");

            if (Input.GetButtonDown("Fire2"))
                _boostButtonPressed = true;  

            // mouse input
            if (_movement.sqrMagnitude > 0.1f)
            {
                // If external device input is pressed don't use mouse
            }
            else if (Input.GetMouseButton(0))
            {
                _movement = Input.mousePosition - Camera.main.WorldToScreenPoint(transform.position);

                if (_movement.sqrMagnitude > 0.5f)
                    _movement.Normalize();
                else
                    _movement = Vector2.zero;
            }

            float velocity = _movement.magnitude;

            if (velocity > 0.1f
                && _boostInputTimer < config.BoostThreshold
                 && _boostButtonPressed)
            {
                _boostEffectTimer = config.BoostDuration;
                _boostInputTimer = config.BoostInterval;
                _boostButtonPressed = false;
            }
            else
            {
                if(_boostInputTimer < 0f)
                {
                    _boostInputTimer = config.BoostInterval;
                }

                _boostInputTimer -= Time.fixedDeltaTime;
            }

            if (_boostEffectTimer > 0f)
            {
                _boostEffectTimer -= Time.fixedDeltaTime;
            }

            if (velocity > 0.1f)
            {
                _movement.Normalize();

                float speed = _boostEffectTimer > 0f ? config.BoostSpeed : config.Speed;
                _rigidbody2D.MovePosition((Vector2)transform.position + _movement * Time.fixedDeltaTime * speed);
                
                Direction = _movement;

                if (Vector2.Dot(Vector2.up, Direction) > 0.5f)
                {
                    PlayerDirection = PlayerDirection.Top;
                }
                else if (Vector2.Dot(Vector2.up, Direction) < -0.5f)
                {
                    PlayerDirection = PlayerDirection.Bottom;
                }
                else if (Vector2.Dot(Vector2.right, Direction) > 0.5f)
                {
                    PlayerDirection = PlayerDirection.Right;
                }
                else
                {
                    PlayerDirection = PlayerDirection.Left;
                }

                if(!IsMoving)
                {
                    OnMovementStart.Invoke();
                }
                
                Velocity = velocity;
                IsMoving = true;
            }
            else
            {
                if (IsMoving)
                {
                    OnMovementStop.Invoke();
                }

                Velocity = 0f;
                IsMoving = false;
            }
        }
    }
}