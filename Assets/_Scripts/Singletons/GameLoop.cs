﻿using System;
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
            PauseBegin?.Invoke(true);
            GameState = GameState.Pause;
        }

        public void Resume()
        {
            PauseBegin?.Invoke(false);
            GameState = GameState.GamePlay;
        }

        public void GameOver()
        {
            GameState = GameState.GameOver;
        }

        public void NewGame()
        {
            GameState = GameState.GamePlay;
        }
        #endregion
    }
}
