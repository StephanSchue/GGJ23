using UnityEngine;

namespace GGJ23.Game
{
    public class MovementController : MonoBehaviour
    {
        public float Speed = 1f;

        private Rigidbody2D _rigidbody2D;
        private Vector2 _movement = new Vector2();
        private bool _isMoving;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            _movement.x = Input.GetAxis("Horizontal");
            _movement.y = Input.GetAxis("Vertical");

            if(_movement.magnitude > 0.1f)
            {
                _movement.Normalize();
                _rigidbody2D.MovePosition((Vector2)transform.position + (_movement * (Speed * Time.deltaTime)));
                _isMoving = true;
            }
            else
            {
                _isMoving = false;
            }
        }
    }
}