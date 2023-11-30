using System;
using System.Threading;
using TouchFall.Helper.Enums;

namespace TouchFall
{
    public sealed class SingleModifyPlayer
    {
        #region Fields
        private static readonly Lazy<SingleModifyPlayer> _instance =
            new(() => new SingleModifyPlayer(), LazyThreadSafetyMode.ExecutionAndPublication);
        #endregion

        #region Events
        public event Action<ModifyHero> Modify;
        #endregion

        #region Constructor
        private SingleModifyPlayer() { }
        #endregion

        #region Properties
        public static SingleModifyPlayer Instance => _instance.Value;
        #endregion

        #region Public Methods
        public void ApplyModify(ModifyHero modifyHero)
        {
            Modify?.Invoke(modifyHero);
        }
        #endregion
    }
}
