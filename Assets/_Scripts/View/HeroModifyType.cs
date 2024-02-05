using TouchFall.Helper.Enums;
using UnityEngine;

namespace TouchFall.View
{
    [RequireComponent(typeof(SpriteRenderer))]
    public sealed class HeroModifyType : MonoBehaviour
    {
        #region Fields
        [SerializeField] private ModifyHero _modifyHero;
        [SerializeField] private Rigidbody2D _body;
        #endregion

        #region Properties
        public ModifyHero ModifyHero => _modifyHero;
        public Rigidbody2D Body => _body;
        #endregion
    }
}
