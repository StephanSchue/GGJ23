using UnityEngine;

namespace GGJ23.Game
{
    public class PropVisuals : MonoBehaviour
    {
        [Header("Components")]
        public Prop prop;
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer spriteRendererLightGlow;

        [Header("Sprites")]
        public Sprite daySprite;
        public Sprite nightSprite;
        public bool useDayNightSprite = true;
        public bool useNightGlow = false;

        private void Update()
        {
            if(useDayNightSprite)
                spriteRenderer.sprite = prop.IsNight ? nightSprite : daySprite;
            
            if(useNightGlow)
                spriteRendererLightGlow.enabled = prop.IsNight;
        }
    }
}