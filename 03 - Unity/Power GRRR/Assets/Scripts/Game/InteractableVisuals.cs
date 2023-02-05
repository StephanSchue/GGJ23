using UnityEngine;

namespace GGJ23.Game
{
    public class InteractableVisuals : MonoBehaviour
    {
        [Header("Components")]
        public Interactable interactable;
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer spriteRendererLightGlow;
        public Animator animator;

        [Header("Settings")]
        public RuntimeAnimatorController animatorController;
        public Color[] statusColor;
        public bool useStatusColor = true;
        public bool useAnimator = true;

        private bool animate = false;

        private void Awake()
        {
            animator.runtimeAnimatorController = animatorController;
            animate = animatorController != null;
        }

        // Update is called once per frame
        private void Update()
        {
            var status = interactable.Status;
         
            if(useStatusColor)
                spriteRenderer.color = statusColor[(int)status];

            bool working = (status == InteractionStatus.Working || status == InteractionStatus.FreshlyRepaired);

            if (interactable.IsNight)
            {
                spriteRendererLightGlow.enabled = working;
            }
            else
            {
                spriteRendererLightGlow.enabled = false;
            }

            if (animate && useAnimator)
            {
                animator.SetBool("Broken", !working);
            }
        }
    }
}