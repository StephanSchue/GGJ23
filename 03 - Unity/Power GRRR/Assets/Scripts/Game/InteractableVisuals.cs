using UnityEngine;
using DG.Tweening;

namespace GGJ23.Game
{
    public class InteractableVisuals : MonoBehaviour
    {
        [Header("Components")]
        public Interactable interactable;
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer spriteRendererLightGlow;
        public SpriteRenderer notification;
        public Animator animator;

        [Header("Settings")]
        public RuntimeAnimatorController animatorController;
        public Color[] statusColor;
        public bool useStatusColor = true;
        public bool useAnimator = true;

        private bool animate = false;
        private bool fadingOut = false;
        private bool fadingIn = false;

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
                notification.gameObject.SetActive(false);
                fadingIn = false;
                fadingOut = false;
                notification.color = new Color(notification.color.r, notification.color.g, notification.color.b, 1f);
            }
            else
            {
                spriteRendererLightGlow.enabled = false;
                if (working)
                {
                    notification.gameObject.SetActive(false);
                    fadingIn = false;
                    fadingOut = false;
                    notification.color = new Color(notification.color.r, notification.color.g, notification.color.b, 1f);
                }
                else
                {
                    notification.gameObject.SetActive(true);
                    if (!fadingOut && !fadingIn)
                    {
                        fadingOut = true;
                        DOTween.ToAlpha(() => notification.color, x => notification.color = x, 0f, 1f).onComplete = () => SwitchFade();
                    }
                }
            }

            if (animate && useAnimator)
            {
                animator.SetBool("Broken", !working);
            }
        }

        private void SwitchFade()
        {
            if (!fadingIn && !fadingOut)
            { 
                    notification.color = new Color(notification.color.r, notification.color.g, notification.color.b, 1f);
                    return;
            }

            fadingIn = !fadingIn;
            fadingOut = !fadingOut;
            if (fadingIn)
            {
                        DOTween.ToAlpha(() => notification.color, x => notification.color = x, 1f, 1f).onComplete = () => SwitchFade();
            }
            else if (fadingOut)
            {
                        DOTween.ToAlpha(() => notification.color, x => notification.color = x, 0f, 1f).onComplete = () => SwitchFade();
            }
        }
    }
}