using UnityEngine;

namespace GGJ23.Game.Visuals
{
    public class PickupVisuals : MonoBehaviour
    {
        [Header("Components")]
        public Pickup pickup;
        public SpriteRenderer spriteRenderer;

        private void Awake()
        {

        }

        private void Update()
        {
            spriteRenderer.enabled = !pickup.IsNight && pickup.Status == PickupStatus.On;
        }
    }
}