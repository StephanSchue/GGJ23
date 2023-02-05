using UnityEngine;

namespace GGJ23.Game
{
    public class CharacterVisuals : MonoBehaviour
    {
        public MovementController movementController;
        public InteractionController interactionController;

        public SpriteRenderer spriteRenderer;
        public Animator animator;

        public SpriteRenderer nightSprite;

        private PlayerDirection playerDirection;

        private void Update()
        {
            if (interactionController.IsNight)
            {
                spriteRenderer.gameObject.SetActive(false);
                nightSprite.gameObject.SetActive(true);
                return;
            }

            spriteRenderer.gameObject.SetActive(true);
            nightSprite.gameObject.SetActive(false);
            float dot = Vector3.Dot(Vector3.right, movementController.Direction);
            spriteRenderer.flipX = (dot < 0f);

            animator.SetBool("Working", interactionController.IsWorking);
            animator.SetFloat("Velocity", movementController.Velocity);

            if (playerDirection != movementController.PlayerDirection)
            {
                switch (movementController.PlayerDirection)
                {
                    case PlayerDirection.Top: animator.SetTrigger("Top"); break;
                    case PlayerDirection.Right: animator.SetTrigger("Right"); break;
                    case PlayerDirection.Bottom: animator.SetTrigger("Bottom"); break;
                    case PlayerDirection.Left: animator.SetTrigger("Right"); break; // Since we mirror the character
                }

                playerDirection = movementController.PlayerDirection;
            }
        }
    }

}