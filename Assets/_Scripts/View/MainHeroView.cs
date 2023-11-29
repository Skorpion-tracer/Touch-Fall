using System.Collections;
using UnityEngine;

namespace TouchFall.View
{
    [RequireComponent(typeof(Rigidbody2D))]
    public sealed class MainHeroView : MonoBehaviour
    {
        #region Fields
        private Rigidbody2D _body;

        private Coroutine _corutine;
        #endregion

        #region Properties
        public Rigidbody2D Body => _body;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
        }

        private void OnEnable()
        {
            SingleModifyPlayer.Instance.Modify += OnModify;
        }

        private void OnDisable()
        {
            SingleModifyPlayer.Instance.Modify -= OnModify;
        }
        #endregion

        #region Private Methods
        private void OnModify()
        {
            //if (_corutine == null)
            //    _corutine = StartCoroutine(nameof(CorutineRotation));
        }

        private IEnumerator CorutineRotation()
        {
            while (true)
            {
                Debug.Log("Вращение");
                _body.MoveRotation(15);
                yield return null;
            }
        }
        #endregion
    }
}

