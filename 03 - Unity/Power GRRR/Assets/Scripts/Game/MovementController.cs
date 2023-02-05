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
        public float Speed = 1f;
        public float BoostInterval = 2f;
        public float BoostThreshold = 0.5f;
        public float BoostDuration = 2f;
        public float BoostSpeed = 1.5f;

        private Rigidbody2D _rigidbody2D;
        private Vector2 _movement = new Vector2();

        private float _boostInputTimer = 0f;
        private float _boostEffectTimer = 0f;

        public bool IsMoving { get; private set; }
        public Vector2 Direction { get; private set; }
        public PlayerDirection PlayerDirection { get; private set; } = PlayerDirection.Right;
        public float Velocity { get; private set; }

        public float BoostIntervalPercentage => _boostInputTimer / BoostInterval;
        public bool BoostReady => _boostInputTimer < BoostThreshold;
        public bool BoostActive => _boostEffectTimer > 0f;

        [Header("Events")]
        public UnityEvent OnMovementStart;
        public UnityEvent OnMovementStep;
        public UnityEvent OnMovementStop;
        public UnityEvent OnMovementBoost;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _boostInputTimer = BoostInterval;
            _boostEffectTimer = 0f;
        }

        public void RegisterEvents(EffectContoller effectContoller)
        {
            OnMovementStart.AddListener(() => effectContoller.OnMovementStart.Invoke());
            OnMovementStep.AddListener(() => effectContoller.OnMovementStep.Invoke());
            OnMovementStop.AddListener(() => effectContoller.OnMovementStop.Invoke());
            OnMovementBoost.AddListener(() => effectContoller.OnMovementBoost.Invoke());
        }

        private void FixedUpdate()
        {
            _movement.x = Input.GetAxis("Horizontal");
            _movement.y = Input.GetAxis("Vertical");

            float velocity = _movement.magnitude;

            if (velocity > 0.1f
                && _boostInputTimer < BoostThreshold
                 && Input.GetButtonDown("Fire1"))
            {
                _boostEffectTimer = BoostDuration;
                _boostInputTimer = BoostInterval;
            }
            else
            {
                if(_boostInputTimer < 0f)
                {
                    _boostInputTimer = BoostInterval;
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

                float speed = _boostEffectTimer > 0f ? BoostSpeed : Speed;
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