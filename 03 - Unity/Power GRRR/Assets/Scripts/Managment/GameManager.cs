using UnityEngine;
using UnityEngine.Events;

namespace GGJ23.UI
{
    public class GameManager : MonoBehaviour
    {
        public float Energy { get; private set; } = 100f;
        public int Score { get; private set; } = 0;

        public UnityEvent OnGameOver;

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

        public void SetEnergy(float energy)
        {
            Energy = energy;
        }

        public void SetScore(int score)
        {
            Score = score;
        }

        public void GameOver()
        {
            OnGameOver.Invoke();
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