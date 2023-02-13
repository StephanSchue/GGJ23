using UnityEngine;

namespace GGJ23.Game.Visuals
{
    public class PropVisuals : MonoBehaviour
    {
        [Header("Components")]
        public Prop prop;
        public SpriteRenderer spriteRenderer;
        public SpriteRenderer spriteRendererLightGlow;

        [Header("Sprites")]
        public Sprite[] daySprites;
        public Sprite[] nightSprites;
        public bool useDayNightSprite = true;
        public bool useNightGlow = false;

        private int index = 0;

        private void Awake()
        {
            index = 0;
            Ramdomize();
        }

        public void Ramdomize()
        {
            index = Random.Range(0, daySprites.Length);
        }

        private void Update()
        {
            if(useDayNightSprite)
                spriteRenderer.sprite = prop.IsNight ? nightSprites[index] : daySprites[index];
            
            if(useNightGlow)
                spriteRendererLightGlow.enabled = prop.IsNight;
        }
    }
}