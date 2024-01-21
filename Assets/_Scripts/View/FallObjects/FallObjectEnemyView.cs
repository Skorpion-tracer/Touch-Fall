using TouchFall.Singletons;
using UnityEngine;

namespace TouchFall.View
{
    public sealed class FallObjectEnemyView : BaseFallObjectView
    {
        #region Public Methods
        public override void ApplyMod()
        {
            GameLevel.Instance.ApplyDamage();
            gameObject.SetActive(false);
        }

        public override void DropObject()
        {
            gameObject.SetActive(false);
        }
        #endregion
    }
}
