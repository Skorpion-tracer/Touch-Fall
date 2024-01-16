using System;
using System.Threading;

namespace TouchFall.Singletons
{
    public sealed class GameLevel
    {
        #region Fields
        private static readonly Lazy<GameLevel> _instance =
            new(() => new GameLevel(), LazyThreadSafetyMode.ExecutionAndPublication);

        private const int _maxLife = 3;
        private const int _minLife = 0;
        private int _countLife = 3;
        #endregion

        #region Events
        public event Action ChangeLevel;
        public event Action Damage;
        public event Action GameOver;
        public event Action ExitMenu;
        public event Action CreateGameSession;
        public event Action<int> EarnPoints;
        #endregion

        #region Constructor
        private GameLevel() { }
        #endregion

        #region Properties
        public static GameLevel Instance => _instance.Value;

        public int Lifes
        {
            get => _countLife;
            private set
            {
                if (value >= _maxLife)
                {
                    value = _maxLife;
                }
                else if (value < _minLife)
                {
                    value = _minLife;
                }
                _countLife = value;
            }
        }
        #endregion

        #region Public Methods
        public void NewGame()
        {
            Lifes = 3;
            CreateGameSession?.Invoke();
            GameLoop.Instance.NewGame();
        }

        public void IncrementLevel()
        {
            ChangeLevel?.Invoke();
        }

        public void ApplyDamage()
        {
            Lifes--;
            Damage?.Invoke();
            if (Lifes == 0)
            {
                GameOver?.Invoke();
                GameLoop.Instance.GameOver();
            }
        }

        public void ChargePoints(int points)
        {
            EarnPoints?.Invoke(points);
        }

        public void SetExtraLife()
        {
            Lifes = 1;
        }
        #endregion
    }
}
