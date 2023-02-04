using UnityEngine;

namespace GGJ23.Game
{
    public class InteractableVisuals : MonoBehaviour
    {
        public Interactable interactable;
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer spriteLightGlow;

        public Color[] statusColor;

        // Update is called once per frame
        private void Update()
        {
            spriteRenderer.color = statusColor[(int)interactable.Status];
            spriteLightGlow.enabled = interactable.IsNight;
        }
    }
}