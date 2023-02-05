using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace GGJ23.Managment
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

        private void Start()
        {
            gameLogic.GameOver.AddListener(GameOver);
        }

        #region loop

        private void Update()
        {
            Energy = 1f - gameLogic.LossPercentage;
            Score = gameLogic.Score;
        }

        #endregion

        #region public methods

        public void Restart()
        {
            _gameOver = false;
            Energy = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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