using UnityEngine;

namespace GGJ23.Game.Visuals
{
    public class CharacterVisuals : MonoBehaviour
    {
        // --- Variables ---
        [Header("Components")]
        public MovementController movementController;
        public InteractionController interactionController;

        public SpriteRenderer spriteRenderer;
        public SpriteRenderer nightSpriteRenderer;
        public Animator animator;        

        private PlayerDirection playerDirection;

        private void Update()
        {
            if (interactionController.IsNight)
            {
                // --- Set Visuals Night ---
                spriteRenderer.enabled = false;
                nightSpriteRenderer.enabled = true;
                animator.enabled = false;
                return;
            }

            // --- Set Visuals Day ---
            spriteRenderer.enabled = true;
            nightSpriteRenderer.enabled = false;
            animator.enabled = true;

            float dot = Vector3.Dot(Vector3.right, movementController.Direction);
            spriteRenderer.flipX = (dot < 0f);

            animator.SetBool("Working", interactionController.IsWorking);
            animator.SetFloat("Velocity", movementController.Velocity);
            animator.SetFloat("Boost", movementController.BoostActive ? 1f : 0f);

            // -- Set Directions --
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