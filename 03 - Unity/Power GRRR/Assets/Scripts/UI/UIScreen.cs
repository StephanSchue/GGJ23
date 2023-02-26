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

        private LayoutGroup[] layoutGroups;

        protected UIController _uiController;

        private void Reset()
        {
            canvas = GetComponent<Canvas>();
            canvasGroup = GetComponent<CanvasGroup>();
            graphicRaycaster = GetComponent<GraphicRaycaster>();
        }

        private void Start()
        {
            layoutGroups = GetComponentsInChildren<LayoutGroup>();
        }

        public void Init(UIController uiController, bool visible)
        {
            _uiController = uiController;

            canvasGroup.alpha = visible ? 1f : 0f;
            canvas.enabled = canvasGroup.blocksRaycasts = canvasGroup.interactable = visible;
        }

        public virtual void Enter() { Show(); }
        public abstract void Tick(float dt, UIInputData inputData);
        public virtual void Exit() { Hide(); }
        
        public void DoAction(UIActionObject actionComponent)
        {
            DoAction(actionComponent.Action);
        }

        protected void DoAction(UIAction action) => _uiController.DoAction(action);

        protected void Show()
        {
            canvas.enabled = canvasGroup.blocksRaycasts = canvasGroup.interactable = graphicRaycaster.enabled = true;
            canvasGroup.DOFade(1f, 0.25f);
        }

        protected void Hide()
        {
            graphicRaycaster.enabled = false;
            canvasGroup.DOFade(0f, 0.25f).OnComplete(() => canvas.enabled = canvasGroup.blocksRaycasts = canvasGroup.interactable = false);
        }

        public virtual void Refresh()
        {
            Canvas.ForceUpdateCanvases();

            for (int i = 0; i < layoutGroups.Length; i++)
            {
                layoutGroups[i].enabled = false;
                layoutGroups[i].enabled = true;
            }

        }
    }
}