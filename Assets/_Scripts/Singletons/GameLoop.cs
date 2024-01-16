using System;
using System.Threading;
using TouchFall.Helper.Enums;
using UnityEngine;

namespace TouchFall.Singletons
{
    public sealed class GameLoop
    {
        #region Fields
        private static readonly Lazy<GameLoop> _instance =
            new(() => new GameLoop(), LazyThreadSafetyMode.ExecutionAndPublication);
        #endregion

        #region Events
        public event Action<bool> PauseBegin;
        #endregion

        #region Constructor
        private GameLoop() { }
        #endregion

        #region Properties
        public static GameLoop Instance => _instance.Value;
        public GameState GameState { get; private set; } = GameState.MainMenu;
        #endregion

        #region Public Methods
        public void Pause()
        {
            GameState = GameState.Pause;
            NullTime(true);
        }

        public void Resume()
        {
            GameState = GameState.GamePlay;
            NullTime(false);
        }

        public void GameOver()
        {
            GameState = GameState.GameOver;
            Time.timeScale = 0;
        }

        public void NewGame()
        {
            GameState = GameState.GamePlay;
            Time.timeScale = 1;
        }
        #endregion

        private void NullTime(bool isPause)
        {
            Time.timeScale = Time.timeScale == 1 ? 0 : 1;
            PauseBegin?.Invoke(isPause);
        }
    }
}
