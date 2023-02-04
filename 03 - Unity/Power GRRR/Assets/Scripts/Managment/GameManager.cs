using UnityEngine;

namespace GGJ23.UI
{
    public class GameManager : MonoBehaviour
    {
        public float Energy { get; private set; } = 100f;

        private bool _paused = false;

        #region init

        private void Initialize()
        {
            DontDestroyOnLoad(this);
        }

        #endregion

        #region loop

        private void Update()
        {
            
        }

        #endregion

        #region public methods

        public void Restart()
        {

        }

        public void Pause(bool pause)
        {
            _paused = pause;
            Time.timeScale = _paused ? 0f : 1f;
        }

        public void Exit()
        {
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #elif UNITY_WEBPLAYER
                Application.OpenURL("http://google.com");
            #else
                Application.Quit();
            #endif
        }

        #endregion
    }
}