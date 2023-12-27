using System;
using System.Threading;

namespace TouchFall.Singletons
{
    public sealed class GameLevel
    {
        #region Fields
        private static readonly Lazy<GameLevel> _instance =
            new(() => new GameLevel(), LazyThreadSafetyMode.ExecutionAndPublication);
        #endregion

        #region Events
        public event Action ChangeLevel;
        public event Action Damage;
        #endregion

        #region Constructor
        private GameLevel() { }
        #endregion

        #region Properties
        public static GameLevel Instance => _instance.Value;
        #endregion

        #region Public Methods
        public void IncrementLevel()
        {
            ChangeLevel?.Invoke();
        }

        public void ApplyDamage()
        {
            Damage?.Invoke();
        }
        #endregion
    }
}
