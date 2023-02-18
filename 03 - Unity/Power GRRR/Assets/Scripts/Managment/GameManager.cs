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

        public void StartGame()
        {
            _gameOver = false;
            Energy = 1f;
            Pause(false);
        }

        public void ContinueGame()
        {
            Pause(false);
        }

        public void RestartGame()
        {
            _gameOver = false;
            Energy = 1f;
            Pause(false);
        }

        public void StopGame()
        {
            Pause(true);
        }

        public void Pause(bool pause)
        {
            _paused = pause;
            gameLogic.InteractionController.enabled = !pause;
            gameLogic.MovementController.enabled = !pause;

            if(pause) { gameLogic.StopGame(); }
            else { gameLogic.RunGame(); }
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