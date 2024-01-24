using System.Collections;
using TouchFall.Singletons;
using TouchFall.View.Interfaces;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace TouchFall.View
{
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class WindowView : MonoBehaviour
    {
        #region SirializedFields
        [SerializeField] private Light2D _light;
        [SerializeField] private Color[] _colors = new[] { Color.yellow, Color.magenta, Color.red};
        [SerializeField, Range(0.1f, 1.5f)] private float _durationLerpColor = 0.1f;

        private int _countColors = 3;
        private int _currentColor;
        private float _coeffStopColorLerp = 3f;
        private Color _startColor;
        #endregion

        #region UnityMethods
        private void OnEnable()
        {
            GameLevel.Instance.CreateGameSession += OnCreateGameSession;
            GameLevel.Instance.ChangeLevel += OnChangeLevel;
        }

        private void OnDisable()
        {
            GameLevel.Instance.CreateGameSession -= OnCreateGameSession;
            GameLevel.Instance.ChangeLevel -= OnChangeLevel;
        }

        private void Awake()
        {
            _startColor = _light.color;
            OnCreateGameSession();
        }

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

        #region Private Methods
        private void OnCreateGameSession()
        {
            _light.color = _startColor;
            Debug.Log(_light.color);
            _currentColor = 0;
        }

        private void OnChangeLevel()
        {
            if (_currentColor == _countColors) return;
            StartCoroutine(SwapColor());
        }

        private IEnumerator SwapColor()
        {
            float i = 0f;
            while (true)
            {
                _light.color = Color.Lerp(_light.color, _colors[_currentColor], _durationLerpColor * Time.deltaTime);
                i += Time.deltaTime;
                if (i >= (_durationLerpColor * _coeffStopColorLerp)) break;
                yield return null;
            }
            _currentColor++;
        }
        #endregion
    }
}
