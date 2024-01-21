using TouchFall.Singletons;
using TouchFall.View.Interfaces;
using UnityEngine;

namespace TouchFall.View
{
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class WindowView : MonoBehaviour
    {
        #region UnityMethods
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IFallingObject fallingObject))
            {
                fallingObject.ApplyMod();
            }
            if (collision.TryGetComponent(out IScore score))
            {
                GameLevel.Instance.ChargePoints(score.Score);
            }
        }
        #endregion
    }
}
