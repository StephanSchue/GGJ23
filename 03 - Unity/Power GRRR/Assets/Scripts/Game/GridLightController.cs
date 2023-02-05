using UnityEngine;
using UnityEngine.Events;
using System.Linq;

namespace GGJ23.Game
{
    public class GridLightController : MonoBehaviour
    {
        public LineRenderer[] lineRenderers;

        private Interactable[] _interactables;

        public void PopulateInteractables(Interactable[] interactables, UnityEvent OnDaySwitch, UnityEvent OnNightSwitch)
        {
            _interactables = interactables;
            
            for (int i = 0; i < lineRenderers.Length; i++)
            {
                lineRenderers[i].positionCount = _interactables.Length;
                for (int x = 0; x < _interactables.Length; x++)
                {
                    lineRenderers[i].SetPosition(x, _interactables[x].transform.position);
                }
            }

            OnDaySwitch.AddListener(this.OnDaySwitch);
            OnNightSwitch.AddListener(this.OnNightSwitch);
        }

        public void RefreshInteractableStatus(Interactable[] interactables)
        {
            _interactables = interactables.Where(x => x.Status != InteractionStatus.Broken && x.Status != InteractionStatus.BeingRepaired).ToArray();

            for (int i = 0; i < lineRenderers.Length; i++)
            {
                lineRenderers[i].positionCount = _interactables.Length;
                for (int x = 0; x < _interactables.Length; x++)
                {
                    lineRenderers[i].SetPosition(x, _interactables[x].transform.position);
                }
            }
        }

        private void OnDaySwitch()
        {
            for (int i = 0; i < lineRenderers.Length; i++)
            {
                lineRenderers[i].enabled = false;
            }
        }

        private void OnNightSwitch()
        {
            for (int i = 0; i < lineRenderers.Length; i++)
            {
                lineRenderers[i].enabled = true;
            }
        }
    }
}