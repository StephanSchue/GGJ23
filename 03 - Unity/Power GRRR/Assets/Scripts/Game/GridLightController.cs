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

        public bool includeProps = false;

        private Interactable[] _interactables;
        private Prop[] _props;

        private Dictionary<int, List<Transform>> roots = new Dictionary<int, List<Transform>>();
        private int _rootCount = 0;

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
            _rootCount = 0;
            roots.Clear();

            List<Transform> transforms = new List<Transform>();

            // --- Interactables ---
            for (int i = 0; i < _interactables.Length; i++)
            {
                if(!_interactables[i].IsBroken)
                {
                    transforms.Add(_interactables[i].transform);
                }
            }

            // --- Props ---
            if (includeProps)
            {
                for (int i = 0; i < _props.Length; i++)
                {
                    transforms.Add(_props[i].transform);
                }
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
                    if(_rootCount < lineRenderers.Length)
                    {
                        Vector3 origin = transforms[i].position;
                        LineRenderer lineRenderer = lineRenderers[_rootCount];
                        lineRenderer.positionCount = transforms.Count;
                        int lineCount = 0;
                        lineRenderer.SetPosition(lineCount++, origin);
                        roots.Add(_rootCount, new List<Transform>() { transforms[i] });

                        for (int x = 0; x < transforms.Count; x++)
                        {
                            if (Vector2.Distance(origin, transforms[x].position) < energyConnectionDistance
                                && !roots[_rootCount].Contains(transforms[x]))
                            {
                                lineRenderer.SetPosition(lineCount++, transforms[x].position);
                                origin = transforms[x].position;
                                roots[_rootCount].Add(transforms[x]);
                            }
                        }

                        lineRenderer.positionCount = lineCount;
                        this._rootCount++;
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
                lineRenderers[i].enabled = i < _rootCount;
            }
        }
    }
}