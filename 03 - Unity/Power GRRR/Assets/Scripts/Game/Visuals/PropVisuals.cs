using UnityEngine;

namespace GGJ23.Game.Visuals
{
    public class PropVisuals : MonoBehaviour
    {
        [Header("Components")]
        public Prop prop;
        public SpriteRenderer[] spriteRenderers;
        public SpriteRenderer spriteRendererLightGlow;

        [Header("Sprites")]
        public bool useNightGlow = false;

        private void Awake()
        {

        }

        private void Update()
        {
            if(useNightGlow)
                spriteRendererLightGlow.enabled = prop.IsNight;

            for (int i = 0; i < spriteRenderers.Length; i++)
            {
                spriteRenderers[i].enabled = !prop.IsNight;
            }
        }
    }
}