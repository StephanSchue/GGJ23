using GGJ23.Game;
using UnityEngine;

namespace GGJ23.Editor
{
    public class CharacterAnimationTester : MonoBehaviour
    {
        public Animator animator;
        public float velocity = 0f;
        public PlayerDirection direction = PlayerDirection.Right;
        public bool working = false;
        public float boost = 0f;

        private void Reset()
        {
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            animator.SetFloat("Velocity", velocity);

            if (direction == PlayerDirection.Up) { animator.SetTrigger("Top"); }
            else if (direction == PlayerDirection.Right) { animator.SetTrigger("Right"); }
            else if (direction == PlayerDirection.Down) { animator.SetTrigger("Bottom"); }

            animator.SetBool("Working", working);
            animator.SetFloat("Boost", boost);
        }
    }

}