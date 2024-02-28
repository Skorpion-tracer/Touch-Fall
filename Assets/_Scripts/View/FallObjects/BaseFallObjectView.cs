using TouchFall.View.Interfaces;
using UnityEngine;

namespace TouchFall.View
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public abstract class BaseFallObjectView : MonoBehaviour, IFallingObject, IPauseRigidbody2D
    {
        #region Fields
        [SerializeField] private Rigidbody2D _body;

        private Vector2 _lastVelocity;
        private float _lastAngularVelocity;
        private float _rangeVector = 30f;
        #endregion

        private void OnEnable()
        {
            _body.AddForce(new Vector2(Random.Range(-_rangeVector, _rangeVector), Random.Range(-_rangeVector, _rangeVector)), ForceMode2D.Impulse);
            Debug.Log("Активация");
        }

        #region Public Methods
        public abstract void ApplyMod();

        public abstract void DropObject();

        public void Pause(bool isPause)
        {
            if (isPause)
            {
                _lastVelocity = _body.velocity;
                _lastAngularVelocity = _body.angularVelocity;

                _body.velocity = Vector2.zero;
                _body.angularVelocity = 0f;

                _body.isKinematic = isPause;
            }
            else
            {
                _body.isKinematic = isPause;
                _body.velocity = _lastVelocity;
                _body.angularVelocity = _lastAngularVelocity;
            }
        }
        #endregion
    }
}
