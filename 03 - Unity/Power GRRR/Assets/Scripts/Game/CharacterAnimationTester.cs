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

            if (direction == PlayerDirection.Top) animator.SetTrigger("top");
            else if (direction == PlayerDirection.Right) animator.SetTrigger("right");
            else if (direction == PlayerDirection.Bottom) animator.SetTrigger("bottom");

            animator.SetBool("Working", working);
            animator.SetFloat("Boost", boost);
        }
    }

}