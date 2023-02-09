using UnityEngine;
using DG.Tweening;

namespace GGJ23.Game
{
    public enum InteractableType
    {
        Tree = 0,
        Flower,
        Cactus
    }

    public class InteractableVisuals : MonoBehaviour
    {
        [Header("Components")]
        public Interactable interactable;
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer spriteRendererLightGlow;
        public SpriteRenderer notification;
        public Animator animator;

        [Header("Settings")]
        public RuntimeAnimatorController[] animatorControllers;
        public Color[] statusColor;
        public bool useStatusColor = true;
        public bool useAnimator = true;
        public bool useRandomize = true;

        private bool animate = false;
        private bool fadingOut = false;
        private bool fadingIn = false;

        private int index = 0;

        private void Awake()
        {
            animate = useAnimator && animatorControllers != null;

            if(animate && useRandomize)
                Ramdomize();
        }

        public void Ramdomize()
        {
            index = Random.Range(0, animatorControllers.Length);
            animator.runtimeAnimatorController = animatorControllers[index];
            animator.Rebind();
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
                spriteRendererLightGlow.enabled = true;
                spriteRenderer.gameObject.SetActive(false);
                notification.gameObject.SetActive(false);

                if(animate)
                    animator.enabled = false;

                fadingIn = false;
                fadingOut = false;
                notification.color = new Color(notification.color.r, notification.color.g, notification.color.b, 1f);
            }
            else
            {
                spriteRendererLightGlow.enabled = false;
                spriteRenderer.gameObject.SetActive(true);

                if (animate)
                    animator.enabled = true;

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