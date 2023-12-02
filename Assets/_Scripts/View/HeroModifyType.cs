using TouchFall.Helper.Enums;
using UnityEngine;

namespace TouchFall.View
{
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    public sealed class HeroModifyType : MonoBehaviour
    {
        #region Fields
        [SerializeField] private ModifyHero _modifyHero;
        [SerializeField] private Rigidbody2D _body;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        #endregion

        #region Properties
        public ModifyHero ModifyHero => _modifyHero;
        public Rigidbody2D Body => _body;
        public SpriteRenderer SpriteRenderer => _spriteRenderer;
        #endregion

        #region Unity Methods
        private void OnValidate()
        {
            _body ??= GetComponent<Rigidbody2D>();
            _spriteRenderer ??= GetComponent<SpriteRenderer>();
        }
        #endregion
    }
}
