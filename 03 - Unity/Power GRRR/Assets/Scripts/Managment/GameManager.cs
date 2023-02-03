using UnityEngine;

namespace GGJ23.UI
{
    public class GameManager : MonoBehaviour
    {
        private bool _paused = false;

        #region init

        private void Initialize()
        {

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