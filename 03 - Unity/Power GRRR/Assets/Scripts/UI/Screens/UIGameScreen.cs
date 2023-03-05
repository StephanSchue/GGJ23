using GGJ23.Game;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace GGJ23.UI
{
    public class UIGameScreen : UIScreen
    {
        // Progress
        [Header("Game Progress")]
        public Image gameProgress;
        public Image progressDirection;
        public Sprite progressDirectionSpriteBalanced;
        public Sprite progressDirectionSpriteIncrease;
        public Sprite progressDirectionSpriteDecrease;
        public TextMeshProUGUI scoreLabel;

        [Header("Game Progress")]
        public UIImageProgress timeTileNextPhaseImage;

        // Input
        [Header("Input")]
        public CanvasGroup interactionPad;
        public Button boostIcon;
        public TextMeshProUGUI boostLabel;
        public Animator boostAnimator;

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

        private EnergyStatus energyStatus = EnergyStatus.None;

        public override void Enter()
        {
            base.Enter();
            energyStatus = EnergyStatus.None;
            _uiRepairButtonHold = false;
            
            _isNight = false;
            interactButton.gameObject.SetActive(!_isNight);
            boostButton.gameObject.SetActive(!_isNight);
        }

        public override void Tick(float dt, UIInputData inputData)
        {
            // -- Energy Progress ---
            gameProgress.fillAmount = _uiController.gameManager.Energy;

            if (_uiController.gameManager.EnergyStatus != energyStatus)
            {
                switch (_uiController.gameManager.EnergyStatus)
                {
                    case EnergyStatus.Increase:
                        progressDirection.sprite =progressDirectionSpriteIncrease;
                        break;
                    case EnergyStatus.Decrease:
                        progressDirection.sprite = progressDirectionSpriteDecrease;
                        break;
                    default:
                        progressDirection.sprite = progressDirectionSpriteBalanced;
                        break;
                }

                energyStatus = _uiController.gameManager.EnergyStatus;
            }

            // --- Input Pad ---
            interactionPad.alpha = _uiController.interactionController.IsWorking ? 1f : 0f;
            interactionPad.interactable = interactionPad.blocksRaycasts = _uiController.interactionController.IsWorking;

            // --- Boost ---
            boostIcon.interactable = _uiController.movementController.BoostCount > 0 && !_uiController.movementController.BoostActive;
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
