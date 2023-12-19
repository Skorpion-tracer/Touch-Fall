using System;
using System.Threading;
using TouchFall.Helper.Enums;

namespace TouchFall.Singletons
{
    public sealed class ModifyBound
    {
        #region Fields
        private static readonly Lazy<ModifyBound> _instance =
            new(() => new ModifyBound(), LazyThreadSafetyMode.ExecutionAndPublication);
        #endregion

        #region Events
        public event Action<ModifyBounds> Modify;
        #endregion

        #region Constructor
        private ModifyBound() { }
        #endregion

        #region Properties
        public static ModifyBound Instance => _instance.Value;
        #endregion

        #region Public Methods
        public void ApplyModify(ModifyBounds modifyHero)
        {
            Modify?.Invoke(modifyHero);
        }
        #endregion
    }
}
