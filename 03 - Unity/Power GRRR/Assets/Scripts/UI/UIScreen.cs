using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace GGJ23.UI
{
    public abstract class UIScreen : MonoBehaviour
    {
        public Canvas canvas;
        public CanvasGroup canvasGroup;
        public GraphicRaycaster graphicRaycaster;

        protected UIController _uiController;

        public void Init(UIController uiController)
        {
            _uiController = uiController;
        }

        public abstract void Enter();
        public abstract void Tick(float dt, UIInputData inputData);
        public abstract void Exit();

        public void DoAction(UIAction action)
        {
            _uiController.DoAction(action);
        }

        protected void Show()
        {
            canvas.enabled = canvasGroup.blocksRaycasts = canvasGroup.interactable = graphicRaycaster.enabled = true;
            canvasGroup.DOFade(1f, 0.25f);
        }

        protected void Hide()
        {
            graphicRaycaster.enabled = false;
            canvasGroup.DOFade(1f, 0.25f).OnComplete(() => canvas.enabled = canvasGroup.blocksRaycasts = canvasGroup.interactable = false);
        }
    }
}