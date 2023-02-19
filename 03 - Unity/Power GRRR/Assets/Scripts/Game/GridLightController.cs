using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Collections.Generic;

namespace GGJ23.Game
{
    public class GridLightController : MonoBehaviour
    {
        private LineRenderer[] lineRenderers;
        public float energyConnectionDistance = 1f;

        private Interactable[] _interactables;
        private Prop[] _props;

        private Dictionary<int, List<Transform>> roots = new Dictionary<int, List<Transform>>();

        public void PopulateInteractables(Interactable[] interactables, Prop[] props, UnityEvent OnDaySwitch, UnityEvent OnNightSwitch)
        {
            lineRenderers = GetComponentsInChildren<LineRenderer>();

            _interactables = interactables;
            _props = props;

            RefreshInteractableStatus();

            OnDaySwitch.AddListener(this.OnDaySwitch);
            OnNightSwitch.AddListener(this.OnNightSwitch);
        }

        public void RefreshInteractableStatus()
        {
            int count = 0;
            roots.Clear();

            List<Transform> transforms = new List<Transform>();

            for (int i = 0; i < _interactables.Length; i++)
            {
                if(_interactables[i].Status != InteractionStatus.Broken && _interactables[i].Status != InteractionStatus.BeingRepaired)
                {
                    transforms.Add(_interactables[i].transform);
                }
            }

            for (int i = 0; i < _props.Length; i++)
            {
                transforms.Add(_props[i].transform);
            }

            // --- Interactables && Props ---
            for (int i = 0; i < transforms.Count; i++)
            {
                bool contains = false;
                foreach (var root in roots)
                {
                    if (root.Value.Contains(transforms[i]))
                    {
                        contains = true;
                        break;
                    }
                }

                if (!contains)
                {
                    if(count < lineRenderers.Length)
                    {
                        Vector3 origin = transforms[i].position;
                        LineRenderer lineRenderer = lineRenderers[count];
                        lineRenderer.positionCount = transforms.Count;
                        int lineCount = 0;
                        lineRenderer.SetPosition(lineCount++, origin);
                        roots.Add(count, new List<Transform>() { transforms[i] });

                        for (int x = 0; x < transforms.Count; x++)
                        {
                            if (Vector2.Distance(origin, transforms[x].position) < energyConnectionDistance
                                && !roots[count].Contains(transforms[x]))
                            {
                                lineRenderer.SetPosition(lineCount++, transforms[x].position);
                                origin = transforms[x].position;
                                roots[count].Add(transforms[x]);
                            }
                        }

                        lineRenderer.positionCount = lineCount;
                        count++;
                    } 
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