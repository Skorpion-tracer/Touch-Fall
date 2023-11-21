using TouchFall.Model;
using TouchFall.View.Interfaces;
using UnityEngine;

namespace TouchFall.View
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BottomTriggerView : MonoBehaviour
    {
        #region Fields
        private BoundModel _boundModel;
        private Vector2 _screenBounds;
        #endregion

        #region PublicMethods
        public void Initialized(BoundModel boundModel, Vector2 screenBounds)
        {
            _boundModel = boundModel;
            _screenBounds = screenBounds;
        }
        #endregion

        #region Unity Methods
        private void Start()
        {
            transform.localScale = new Vector2(_screenBounds.x * _boundModel.BottomTriggerWidth, transform.localScale.y);
            transform.position = new Vector2(_screenBounds.x, -_screenBounds.y - _boundModel.BottomTriggerPositionOffset);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IFallingObject fallingObject))
            {
                fallingObject.DropObject();
            }
        }
        #endregion
    }
}
