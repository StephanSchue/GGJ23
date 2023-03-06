using GGJ23.Game;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

namespace GGJ23.UI
{
    public class UIGameScreen : UIScreen
    {
        // Progress
        [Header("Game Progress")]
        public Image gameProgress;
        public RectTransform gameProgressBarTransform;
        public Animator gameProgressBarAnimator;
        public RectTransform gameProgressEnergyStatusTransform;
        public Animator gameProgressEnergyStatusAnimator;
        public Animator gameProgressEnergyOverlayAnimator;
        public TextMeshProUGUI scoreLabel;

        [Header("Game Progress")]
        public UIImageProgress timeTileNextPhaseImage;

        // Input
        [Header("Input")]
        public CanvasGroup interactionPad;
        public Button boostIcon;
        public TextMeshProUGUI boostLabel;
        public Animator boostAnimator;
        public ContentSizeFitter controlsGroup;

        public Button interactButton;
        public Button boostButton;

        [Header("Direction Helper")]
        public Image[] directionHelperImages;
        public Vector2 directionHelperMinMaxDistance;
        public Vector2 directionHelperMinMaxAlpha;

        private bool _uiRepairButtonHold = false;
        private bool _uiTurboButtonHold = false;

        private bool _isNight = false;

        private int _boostCount = 0;
        private bool _boostActive = false;

        private Vector2 _energyBarSize;

        private EnergyStatus energyStatus = EnergyStatus.None;

        public override void Enter()
        {
            base.Enter();
            energyStatus = EnergyStatus.None;
            _uiRepairButtonHold = false;
            
            _isNight = false;
            interactButton.gameObject.SetActive(!_isNight);
            boostButton.gameObject.SetActive(!_isNight);

            _energyBarSize = new Vector2(390f, 30f);

            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)controlsGroup.transform);
        }

        public override void Tick(float dt, UIInputData inputData)
        {
            // -- Energy Progress ---
            gameProgress.fillAmount = _uiController.gameManager.Energy;
            gameProgressEnergyStatusTransform.sizeDelta = new Vector2(Mathf.Max(30, _energyBarSize.x * _uiController.gameManager.Energy), _energyBarSize.y);

            // --- Status ---
            if(_uiController.gameManager.Energy < 0.3f)
            {
                gameProgressBarAnimator.SetInteger("Status", 2); // Critical
                gameProgressEnergyOverlayAnimator.SetInteger("Status", 1);
            }
            else if(_uiController.gameManager.Energy < 0.99f)
            {
                gameProgressBarAnimator.SetInteger("Status", 0); // OK
                gameProgressEnergyOverlayAnimator.SetInteger("Status", 0);
            }
            else
            {
                gameProgressBarAnimator.SetInteger("Status", 1); // Balanced
                gameProgressEnergyOverlayAnimator.SetInteger("Status", 0);
            }

            if (_uiController.gameManager.EnergyStatus != energyStatus)
            {
                // --- Pointer ---
                switch (_uiController.gameManager.EnergyStatus)
                {
                    case EnergyStatus.Increase:
                        gameProgressEnergyStatusAnimator.SetInteger("Status", 1);
                        break;
                    case EnergyStatus.Decrease:
                        gameProgressEnergyStatusAnimator.SetInteger("Status", 2);
                        break;
                    default:
                        gameProgressEnergyStatusAnimator.SetInteger("Status", 0);
                        break;
                }
                
                energyStatus = _uiController.gameManager.EnergyStatus;
            }

            // --- Input Pad ---
            interactionPad.alpha = _uiController.interactionController.IsWorking ? 1f : 0f;
            interactionPad.interactable = interactionPad.blocksRaycasts = _uiController.interactionController.IsWorking;

            // --- Boost ---
            boostIcon.interactable = _uiController.movementController.BoostCount > 0;
            boostLabel.text = $"{_uiController.movementController.BoostCount}/{_uiController.movementController.config.MaxBoostPickups}";

            if (!_boostActive && _uiController.movementController.BoostActive) { boostAnimator.SetTrigger("Use"); }
            _boostActive = _uiController.movementController.BoostActive;

            if (_uiController.movementController.BoostCount > _boostCount) { boostAnimator.SetTrigger("Collect"); }
            _boostCount = _uiController.movementController.BoostCount;

            // --- Score Label --
            if (_uiController.gameManager.gameLogic.IsFirstDay)
            {
                scoreLabel.text = $"{_uiController.gameManager.Score.ToString("000")}";
            }
            else if (energyStatus == EnergyStatus.Balanced)
            {
                scoreLabel.text = $"{_uiController.gameManager.Score.ToString("000")} +++";
            }
            else if (energyStatus == EnergyStatus.Increase)
            {
                scoreLabel.text = $"{_uiController.gameManager.Score.ToString("000")} ++";
            }
            else
            {
                scoreLabel.text = $"{_uiController.gameManager.Score.ToString("000")} +";
            }

            // --- Input ---
            if (_uiController.gameManager.gameLogic.IsNight != _isNight)
            {
                _isNight = _uiController.gameManager.gameLogic.IsNight;
                interactButton.gameObject.SetActive(!_isNight);
                boostButton.gameObject.SetActive(!_isNight);
                boostIcon.gameObject.SetActive(!_isNight);
            }

            timeTileNextPhaseImage.Progess(_uiController.gameManager.gameLogic.CurrentTime % 1);

            // Direction Helper
            if(!_isNight && _uiController.interactionController.HasNearestBrokenInteractable)
            {
                PlayerDirection direction;
                var directionVector = _uiController.interactionController.NearestBrokenDirection;

                if (Vector2.Dot(Vector2.up, directionVector) > 0.5f)
                {
                    direction = PlayerDirection.Up;
                }
                else if (Vector2.Dot(Vector2.up, directionVector) < -0.5f)
                {
                    direction = PlayerDirection.Down;
                }
                else if (Vector2.Dot(Vector2.right, directionVector) > 0.5f)
                {
                    direction = PlayerDirection.Right;
                }
                else
                {
                    direction = PlayerDirection.Left;
                }

                for (int i = 0; i < directionHelperImages.Length; i++)
                {
                    if (i == (int)direction)
                    {
                        directionHelperImages[i].enabled = true;
                        Color color = directionHelperImages[i].color;
                        float distance = _uiController.interactionController.NearestBrokenDistance;
                        directionHelperImages[i].color = new Color(color.r, color.g, color.b, 
                            Mathf.Lerp(directionHelperMinMaxAlpha.x, directionHelperMinMaxAlpha.y, 
                            Mathf.Max(0f, distance- directionHelperMinMaxDistance.x) / directionHelperMinMaxDistance.y));
                    }
                    else
                    {
                        directionHelperImages[i].enabled = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < directionHelperImages.Length; i++)
                {
                    directionHelperImages[i].enabled = false;
                }
            }

            // --- Buttons ---
            if (inputData.IsPressed(InputButton.Accept) || inputData.IsHold(InputButton.Accept) || _uiRepairButtonHold)
            {
                DoAction(UIAction.Gameplay_Repair);
            }
            else if (inputData.IsPressed(InputButton.Function01) || _uiTurboButtonHold)
            {
                DoAction(UIAction.Gameplay_Boost);
            }
            else if (inputData.IsPressed(InputButton.Function03))
            {
                DoAction(UIAction.Open_PauseScreen);
            }
        }

        // --- Buttons ---
        public void OnRepairButtonDown()
        {
            _uiRepairButtonHold = true;
        }

        public void OnRepairButtonUp()
        {
            _uiRepairButtonHold = false;
        }

        public void OnTurboButtonDown()
        {
            _uiTurboButtonHold = true;
        }

        public void OnTurboButtonUp()
        {
            _uiTurboButtonHold = false;
        }
    }
}
