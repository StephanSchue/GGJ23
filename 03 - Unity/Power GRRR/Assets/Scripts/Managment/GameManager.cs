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
            Energy = 1f;
            Score = 0;
            gameLogic.ResetGame();
            Pause(false);
        }

        public void ContinueGame()
        {
            Pause(false);
        }

        public void RestartGame()
        {
            Energy = 1f;
            Score = 0;
            gameLogic.ResetGame();
            Pause(false);
        }

        public void StopGame()
        {
            Score = 0;
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

        private void GameOver()
        {
            Pause(true);
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