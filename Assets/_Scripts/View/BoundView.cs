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
            Collider2D[] transforms = GetComponentsInChildren<Collider2D>().Where(e => e.usedByEffector == false).ToArray();

            foreach (var t in transforms)
            {
                WindowView windowView = t.GetComponentInChildren<WindowView>();
                if (windowView != null)
                {
                    _bounds.window = windowView;
                    break;
                }
            }

            if (transforms != null && transforms.Length == 3)
            {
                _bounds.topBound = transforms[0].GetComponentInChildren<Transform>();
                _bounds.bottomBound = transforms[1].GetComponentInChildren<Transform>();
            }            
        }
        #endregion
    }
}
