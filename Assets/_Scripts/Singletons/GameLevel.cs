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
        private const int _countLevelIncrement = 2;
        private int _countLife = 3;
        private int _countStartLife = 3;
        private int _points;
        private int _level;
        private int _countExtraLife;
        #endregion

        #region Events
        public event Action ChangeLevel;
        public event Action Damage;
        public event Action GameOver;
        public event Action ExitMenu;
        public event Action CreateGameSession;
        public event Action SetNewLife;
        public event Action ExtraLife;
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

        public bool IsCanUseExtraLife => _countExtraLife <= 2;
        public int Points => _points;
        #endregion

        #region Public Methods
        public void NewGame()
        {
            Lifes = _countStartLife;
            _countExtraLife = 0;
            CreateGameSession?.Invoke();
            GameLoop.Instance.NewGame();
        }

        public void IncrementLevel()
        {
            ChangeLevel?.Invoke();
            _level++;
            if (_level >= _countLevelIncrement && Lifes < 2)
            {
                _level = 0;
                Lifes++;
                SetNewLife?.Invoke();
            }
        }

        public void ApplyDamage()
        {
            Lifes--;
            Damage?.Invoke();
            if (Lifes <= 0)
            {
                GameOver?.Invoke();
                GameLoop.Instance.GameOver();
            }
        }

        public void ChargePoints(int points)
        {
            _points += points;
            EarnPoints?.Invoke(_points);
        }

        public void Reset()
        {
            _points = 0;
        }

        public void SetExtraLife()
        {
            Lifes = 1;
            ExtraLife?.Invoke();
            _countExtraLife++;
        }

        public void ExitToMainMenu()
        {
            ExitMenu?.Invoke();
        }
        #endregion
    }
}
