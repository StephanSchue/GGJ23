using UnityEngine;

namespace GGJ23.Managment
{
    public class AudioManager : MonoBehaviour
    {
        [System.Serializable]
        public struct AudioLayerData
        {
            public string label;
            public AudioClip audioClip;
            public Vector2 fadeInOut;
            public float volume;
        }

        [System.Serializable]
        public struct SFXData
        {
            public string label;
            public AudioClip[] audioClips;
            public Vector2 fadeInOut;
            public float volume;
        }

        public GameObject audioSourceRoot;
        public AudioLayerData[] audioLayers;
        public SFXData[] sfxLayers;

        private AudioSource[] audioLayerSources;
        private AudioSource[] sfxLayerSources;

        public void Awake()
        {
            // Audio Layers
            audioLayerSources = new AudioSource[audioLayers.Length];

            for (int i = 0; i < audioLayerSources.Length; i++)
            {
                audioLayerSources[i] = audioSourceRoot.AddComponent<AudioSource>();
            }

            // SFX Layers
            sfxLayerSources = new AudioSource[sfxLayers.Length];

            for (int i = 0; i < sfxLayerSources.Length; i++)
            {
                sfxLayerSources[i] = audioSourceRoot.AddComponent<AudioSource>();
            }
        }

        public void PlaySFX(string label)
        {

        }

        public void PlayLayer(string label)
        {

        }
    
        public void RemoveLayer(string label)
        {

        }
    }
}