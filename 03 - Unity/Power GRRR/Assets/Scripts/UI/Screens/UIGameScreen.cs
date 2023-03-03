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

        [Header("Game Progress")]
        public Image timeTileNextPhaseImage;

        // Input
        [Header("Input")]
        public CanvasGroup interactionPad;
        public Button turboIcon;
        public TextMeshProUGUI turboLabel;

        public Button interactButton;
        public Button boostButton;

        private bool _uiRepairButtonHold = false;
        private bool _uiTurboButtonHold = false;

        private bool _isNight = false;

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

            // --- Turbo ---
            turboIcon.interactable = _uiController.movementController.BoostCount > 0 && !_uiController.movementController.BoostActive;
            turboLabel.text = $"{_uiController.movementController.BoostCount}/{_uiController.movementController.config.MaxBoostPickups}";

            // --- Input ---
            if (_uiController.gameManager.gameLogic.IsNight != _isNight)
            {
                _isNight = _uiController.gameManager.gameLogic.IsNight;
                interactButton.gameObject.SetActive(!_isNight);
                boostButton.gameObject.SetActive(!_isNight);
                turboIcon.gameObject.SetActive(!_isNight);
            }

            timeTileNextPhaseImage.color = _isNight ? Color.white : Color.black;

            if (_isNight) { timeTileNextPhaseImage.fillAmount = ((_uiController.gameManager.gameLogic.CurrentTime - 0.5f) % 1f) / 0.5f; }
            else { timeTileNextPhaseImage.fillAmount = (_uiController.gameManager.gameLogic.CurrentTime % 1f) / 0.5f; }


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
