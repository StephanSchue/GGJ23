using UnityEngine;
using TMPro;

namespace GGJ23.Game.Visuals
{
    public class PickupVisuals : MonoBehaviour
    {
        [Header("Components")]
        public Pickup pickup;
        public SpriteRenderer spriteRenderer;
        public Animator animator;

        private void Awake()
        {

        }

        private void Update()
        {
            bool visible = !pickup.IsNight && pickup.Status == PickupStatus.On;
            spriteRenderer.enabled = visible;
        }
    }
}