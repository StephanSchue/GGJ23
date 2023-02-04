using UnityEngine;

namespace GGJ23.Game
{
    public class CharacterVisuals : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        public MovementController movementController;

        private void Update()
        {
            float dot = Vector3.Dot(Vector3.right, movementController.Direction);
            spriteRenderer.flipX = (dot < 0f);
        }
    }

}