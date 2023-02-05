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

        private Rigidbody2D _rigidbody2D;
        private Vector2 _movement = new Vector2();

        public bool IsMoving { get; private set; }
        public Vector2 Direction { get; private set; }
        public PlayerDirection PlayerDirection { get; private set; } = PlayerDirection.Right;
        public float Velocity { get; private set; }

        [Header("Events")]
        public UnityEvent OnMovementStart;
        public UnityEvent OnMovementStep;
        public UnityEvent OnMovementStop;
        public UnityEvent OnMovementBoost;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void RegisterEvents(EffectContoller effectContoller)
        {
            OnMovementStart.AddListener(() => effectContoller.OnMovementStart.Invoke());
            OnMovementStep.AddListener(() => effectContoller.OnMovementStep.Invoke());
            OnMovementStop.AddListener(() => effectContoller.OnMovementStop.Invoke());
            OnMovementBoost.AddListener(() => effectContoller.OnMovementBoost.Invoke());
        }

        private void Update()
        {
            _movement.x = Input.GetAxis("Horizontal");
            _movement.y = Input.GetAxis("Vertical");

            float velocity = _movement.magnitude;

            if(velocity > 0.1f)
            {
                _movement.Normalize();
                _rigidbody2D.MovePosition((Vector2)transform.position + _movement * Time.deltaTime * Speed);
                
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