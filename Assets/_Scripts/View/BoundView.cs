using System.Linq;
using UnityEngine;

namespace TouchFall.View
{
    public sealed class BoundView : MonoBehaviour
    {
        #region Fields
        private (Transform topBound, WindowView window, Transform bottomBound) _bounds;
        #endregion

        #region Properties
        public (Transform topBound, WindowView window, Transform bottomBound) Bounds => _bounds;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            Transform[] transforms = GetComponentsInChildren<Transform>().Where(e => e.TryGetComponent<WindowView>(out _) == false).ToArray();
            if (transforms != null && transforms.Length > 0)
            {
                _bounds.topBound = transforms[0];
                _bounds.bottomBound = transforms[1];
            }
            _bounds.window = GetComponentInChildren<WindowView>();
        }
        #endregion
    }
}
