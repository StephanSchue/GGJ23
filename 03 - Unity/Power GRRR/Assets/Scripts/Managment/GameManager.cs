using UnityEngine;
using UnityEngine.Events;

namespace GGJ23.UI
{
    public class GameManager : MonoBehaviour
    {
        public Game.GameLogic gameLogic;

        public float Energy { get; private set; } = 100f;
        public float Score { get; private set; } = 0;

        [Header("Events")]
        public UnityEvent OnGameOver;

        private bool _paused = false;
        private bool _gameOver = false;

        #region init

        private void Initialize()
        {
            DontDestroyOnLoad(this);
        }

        #endregion

        #region loop

        private void Update()
        {
            Energy = 1f - gameLogic.LossPercentage;
            Score = gameLogic.Score;

            if(!_gameOver && Energy < 0f)
            {
                GameOver();
            }
        }

        #endregion

        #region public methods

        public void Restart()
        {
            _gameOver = false;
            Pause(false);
        }

        public void Pause(bool pause)
        {
            _paused = pause;
            Time.timeScale = _paused ? 0f : 1f;
        }

        public void GameOver()
        {
            Pause(true);

            _gameOver = true;
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