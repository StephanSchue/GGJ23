using GGJ23.Game.Config;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace GGJ23.Game
{
    public enum PlayerDirection
    {
        Up = 0,
        Right,
        Down,
        Left
    }

    public class MovementController : MonoBehaviour
    {
        public MovementConfig config;

        private Rigidbody2D _rigidbody2D;
        private Vector2 _movement = new Vector2();

        private float _boostEffectTimer = 0f;

        private bool _isNight = false;
        private bool _boostButtonPressed = false;

        private bool _blockInput = false;
        private Vector2 _boost = Vector2.zero;
        private int _boostCount = 0;

        private PlayerDirection _boostDirection = PlayerDirection.Right;

        public bool IsMoving { get; private set; }
        public Vector2 Direction { get; private set; }
        public PlayerDirection PlayerDirection { get; private set; } = PlayerDirection.Right;
        public float Velocity { get; private set; }

        public bool BoostActive => _boostEffectTimer > 0f;
        public int BoostCount => _boostCount; 

        [Header("Events")]
        public UnityEvent OnMovementStart;
        public UnityEvent OnMovementStep;
        public UnityEvent OnMovementStop;
        public UnityEvent OnMovementBoost;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _boostEffectTimer = 0f;
            _boostCount = 0;
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

        public void UpdateMovement(Vector2 axis)
        {
            _movement = axis;
        }

        public bool AddBoostPickup()
        {
            if ((_boostCount+1) > config.MaxBoostPickups)
                return false;

            ++_boostCount;
            return true;
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

            if (_blockInput)
            {
                _movement = Vector2.zero;
            }

            float dt = Time.fixedDeltaTime;

            // --- Boost ---
            float velocity = _movement.magnitude;

            if(_boostButtonPressed && _boostCount > 0)
            {
                // Apply Boost
                _boostEffectTimer = config.BoostDuration;
                _boostDirection = PlayerDirection;
                --_boostCount;
            }

            if (_boostEffectTimer > 0f)
            {
                _boostEffectTimer -= dt;
            }

            _boostButtonPressed = false;

            // -- Movement ---
            if (velocity > 0.1f)
            {
                _movement.Normalize();
                
                _boost = _boostEffectTimer > 0f ? GetVectorForPlayerDirection(_boostDirection) : Vector2.zero;
                _rigidbody2D.MovePosition((Vector2)transform.position + (_movement * dt * config.Speed) + (_boost * dt * config.BoostSpeed));
                
                Direction = _movement;

                if (Vector2.Dot(Vector2.up, Direction) > 0.5f)
                {
                    PlayerDirection = PlayerDirection.Up;
                }
                else if (Vector2.Dot(Vector2.up, Direction) < -0.5f)
                {
                    PlayerDirection = PlayerDirection.Down;
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

        private Vector2 GetVectorForPlayerDirection(PlayerDirection playerDirection)
        {
            switch (playerDirection)
            {
                case PlayerDirection.Up: return Vector2.up;
                case PlayerDirection.Right: return Vector2.right;
                case PlayerDirection.Down: return Vector2.down;
                case PlayerDirection.Left: return Vector2.left;
                default: return Vector2.zero;
            }
        }
    }
}