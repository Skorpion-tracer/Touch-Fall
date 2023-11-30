using TouchFall.Helper.Enums;
using UnityEngine;

namespace TouchFall.View
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class HeroModifyType : MonoBehaviour
    {
        #region Fields
        [SerializeField] private ModifyHero _modifyhero;

        [SerializeField] private Rigidbody2D _body;
        #endregion

        #region Properties
        public ModifyHero ModifyHero => _modifyhero;
        public Rigidbody2D Body => _body;
        #endregion

        #region Unity Methods
        private void OnValidate()
        {
            _body ??= GetComponent<Rigidbody2D>();
        }
        #endregion
    }
}
