using System.Threading;
using System;

namespace TouchFall.Singletons
{
    public sealed class GamePlay
    {
        #region Fields
        private static readonly Lazy<GamePlay> _instance =
            new(() => new GamePlay(), LazyThreadSafetyMode.ExecutionAndPublication);
        #endregion

        #region Events
        public event Action<int> EarnPoints;
        #endregion

        #region Constructor
        private GamePlay() { }
        #endregion

        #region Properties
        public static GamePlay Instance => _instance.Value;
        #endregion

        #region Public Methods
        public void ChargePoints(int points)
        {
            EarnPoints?.Invoke(points);
        }
        #endregion
    }
}
