using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Serialization;

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

        [Header("Day Graphics")]
        public SpriteRenderer spriteRenderer;
        public Animator animator;
        [FormerlySerializedAs("notificationSpriteRenderer")]
        public SpriteRenderer dayDotificationSpriteRenderer;
        public Animator dayDotificationAnimator;
        public Canvas progressBarCanvas;
        public Slider progressBarSlider;

        [Header("Night Graphics")]
        [FormerlySerializedAs("spriteRendererLightGlow")]
        public SpriteRenderer spriteRendererNightGlow;
        public SpriteRenderer nightDotificationSpriteRenderer;
        public Animator nightDotificationAnimator;

        [Header("Settings")]
        public RuntimeAnimatorController[] animatorControllers;
        public Image[] puzzleImages;
        public bool useAnimator = true;
        public bool useRandomize = true;
        public int nonRandomizedIndex = 0;

        public Sprite[] puzzleButtons;
        public Color puzzleSelectorColor = Color.red;

        private bool _hasAnimation = false;
        private bool _hasProgressbar = false;

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
            bool working = (status == InteractionStatus.Working || status == InteractionStatus.FreshlyRepaired);

            if (interactable.IsNight)
            {
                // --- Night ---
                SetGraphicsActive(true, working);
            }
            else
            {
                // --- Day ---
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
                SetGraphicsActive(false, working);
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

        public void SetGraphicsActive(bool isNight, bool isWorking)
        {
            // Day
            spriteRenderer.enabled = !isNight;
            animator.enabled = !isNight;
            dayDotificationSpriteRenderer.enabled = !isNight && !isWorking;
            dayDotificationAnimator.enabled = !isNight && !isWorking;
            progressBarCanvas.enabled = !isNight && !isWorking;
            progressBarSlider.enabled = !isNight && !isWorking;

            // Night
            spriteRendererNightGlow.enabled = isNight;
            nightDotificationSpriteRenderer.enabled = isNight && !isWorking;
            nightDotificationAnimator.enabled = isNight && !isWorking;
        }
    }
}