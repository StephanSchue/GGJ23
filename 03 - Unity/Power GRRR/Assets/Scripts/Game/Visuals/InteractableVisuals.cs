using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GGJ23.Game.Visuals
{
    public enum InteractableType
    {
        Tree = 0,
        Flower,
        Cactus
    }

    public class InteractableVisuals : MonoBehaviour
    {
        // --- Variables ---
        [Header("Components")]
        public Interactable interactable;
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer spriteRendererLightGlow;
        public SpriteRenderer notificationSpriteRenderer;
        public Animator animator;
        public Canvas progressBarCanvas;
        public Slider progressBarSlider;
        public Image[] puzzleImages;
        
        [Header("Settings")]
        public RuntimeAnimatorController[] animatorControllers;
        public Color[] statusColor;
        public bool useStatusColor = true;
        public bool useAnimator = true;
        public bool useRandomize = true;
        public int nonRandomizedIndex = 0;

        public Sprite[] puzzleButtons;
        public Color puzzleSelectorColor = Color.red;

        private bool _hasAnimation = false;
        private bool _hasProgressbar = false;

        private bool _fadingOut = false;
        private bool _fadingIn = false;

        private int _index = 0;
        private bool _puzzleRefreshPending = false;

        private void Awake()
        {
            _hasAnimation = useAnimator && animatorControllers != null;
            _hasProgressbar = progressBarCanvas != null && progressBarSlider != null;

            Initalize();

            interactable.OnInitialize.AddListener(Initalize);
            interactable.OnPuzzleRefresh.AddListener(PuzzleRefresh);
        }

        private void Initalize()
        {
            if (_hasAnimation)
            {
                if (useRandomize)
                {
                    SetAnimator(Random.Range(0, animatorControllers.Length));
                }
                else
                {
                    SetAnimator(nonRandomizedIndex);
                }
            }
        }

        private void PuzzleRefresh()
        {
            _puzzleRefreshPending = true;
        }

        // Update is called once per frame
        private void Update()
        {
            var status = interactable.Status;
         
            if (useStatusColor)
            {
                spriteRenderer.color = statusColor[(int)status];
            }

            bool working = (status == InteractionStatus.Working || status == InteractionStatus.FreshlyRepaired);

            if (interactable.IsNight)
            {
                // --- Night ---
                spriteRendererLightGlow.enabled = true;
                spriteRenderer.enabled = false;
                notificationSpriteRenderer.enabled = false;

                if(_hasAnimation) { animator.enabled = false; }

                _fadingIn = false;
                _fadingOut = false;
                notificationSpriteRenderer.color = new Color(notificationSpriteRenderer.color.r, notificationSpriteRenderer.color.g, notificationSpriteRenderer.color.b, 1f);
            }
            else
            {
                // --- Day ---
                spriteRendererLightGlow.enabled = false;
                spriteRenderer.enabled = true;

                if (_hasAnimation) { animator.enabled = true; }

                // Puzzle update
                for (int i = 0; i < puzzleImages.Length; i++)
                {
                    if (interactable.Status == InteractionStatus.BeingRepaired && i < interactable.puzzle.MaxNumber)
                    {
                        if (!puzzleImages[i].gameObject.activeSelf || _puzzleRefreshPending) { puzzleImages[i].gameObject.SetActive(true); puzzleImages[i].sprite = puzzleButtons[interactable.puzzle.Buttons[i]]; }

                        puzzleImages[i].color = i == interactable.puzzle.Index ? puzzleSelectorColor : Color.white;
                    }
                    else
                    {
                        if (puzzleImages[i].gameObject.activeSelf)
                        {
                            puzzleImages[i].gameObject.SetActive(false);
                        }
                    }
                }

                _puzzleRefreshPending = false;

                if (working)
                {
                    notificationSpriteRenderer.enabled = false;
                    _fadingIn = false;
                    _fadingOut = false;
                    notificationSpriteRenderer.color = new Color(notificationSpriteRenderer.color.r, notificationSpriteRenderer.color.g, notificationSpriteRenderer.color.b, 1f);
                }
                else
                {
                    notificationSpriteRenderer.enabled = true;
                    if (!_fadingOut && !_fadingIn)
                    {
                        _fadingOut = true;
                        DOTween.ToAlpha(() => notificationSpriteRenderer.color, x => notificationSpriteRenderer.color = x, 0f, 1f).onComplete = () => SwitchFade();
                    }
                }
            }

            // --- Broken Status ---
            if (_hasAnimation && useAnimator)
            {
                animator.SetBool("Broken", !working);
            }

            // --- Progressbar ---
            if (_hasProgressbar)
            {
                progressBarSlider.value = interactable.Progress;
                progressBarCanvas.enabled = !working && interactable.Status == InteractionStatus.BeingRepaired;
            }
                
        }

        private void SetAnimator(int index)
        {
            _index = index;
            animator.runtimeAnimatorController = animatorControllers[_index];
            animator.Rebind();
        }

        private void SwitchFade()
        {
            if (!_fadingIn && !_fadingOut)
            { 
                notificationSpriteRenderer.color = new Color(notificationSpriteRenderer.color.r, notificationSpriteRenderer.color.g, notificationSpriteRenderer.color.b, 1f);
                return;
            }

            _fadingIn = !_fadingIn;
            _fadingOut = !_fadingOut;

            if (_fadingIn)
            {
                DOTween.ToAlpha(() => notificationSpriteRenderer.color, x => notificationSpriteRenderer.color = x, 1f, 1f).onComplete = () => SwitchFade();
            }
            else if (_fadingOut)
            {
                DOTween.ToAlpha(() => notificationSpriteRenderer.color, x => notificationSpriteRenderer.color = x, 0f, 1f).onComplete = () => SwitchFade();
            }
        }
    }
}