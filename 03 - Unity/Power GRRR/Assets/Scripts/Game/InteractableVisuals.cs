using UnityEngine;

namespace GGJ23.Game
{
    public class InteractableVisuals : MonoBehaviour
    {
        public Interactable interactable;
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer spriteLightGlow;

        public Animator animator;
        public RuntimeAnimatorController animatorController;

        private bool animate = false;

        public Color[] statusColor;

        private void Awake()
        {
            animator.runtimeAnimatorController = animatorController;
            animate = animatorController != null;
        }

        // Update is called once per frame
        private void Update()
        {
            var status = interactable.Status;
            spriteRenderer.color = statusColor[(int)status];

            bool working = (status == InteractionStatus.Working && status == InteractionStatus.FreshlyRepaired);

            if (interactable.IsNight)
            {
                spriteLightGlow.enabled = working;
            }
            else
            {
                spriteLightGlow.enabled = false;
            }

            if (animate)
            {
                animator.SetBool("Broken", !working);
            }
        }
    }
}