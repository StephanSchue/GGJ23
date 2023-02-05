using UnityEngine;
using DG.Tweening;

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
                audioLayerSources[i].clip = audioLayers[i].audioClip;
                audioLayerSources[i].volume = 0f;
                audioLayerSources[i].loop = true;
                audioLayerSources[i].Play();
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
            for (int i = 0; i < audioLayers.Length; i++)
            {
                if (audioLayers[i].label == label)
                {
                    audioLayerSources[i].DOFade(audioLayers[i].volume, audioLayers[i].fadeInOut.x);
                    break;
                }
            }
        }
    
        public void RemoveLayer(string label)
        {
            for (int i = 0; i < audioLayers.Length; i++)
            {
                if (audioLayers[i].label == label)
                {
                    audioLayerSources[i].DOFade(0f, audioLayers[i].fadeInOut.y);
                    break;
                }
            }
        }
    }
}