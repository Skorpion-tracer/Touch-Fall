using System;
using System.Threading;
using TouchFall.Helper.Enums;

namespace TouchFall.Singletons
{
    public sealed class ModifyPlayer
    {
        #region Fields
        private static readonly Lazy<ModifyPlayer> _instance =
            new(() => new ModifyPlayer(), LazyThreadSafetyMode.ExecutionAndPublication);
        #endregion

        #region Events
        public event Action<ModifyHero> Modify;
        #endregion

        #region Constructor
        private ModifyPlayer() { }
        #endregion

        #region Properties
        public static ModifyPlayer Instance => _instance.Value;
        #endregion

        #region Public Methods
        public void ApplyModify(ModifyHero modifyHero)
        {
            Modify?.Invoke(modifyHero);
        }
        #endregion
    }
}
