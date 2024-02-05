using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField] private TextMeshPro _textScore;
        [SerializeField] private Light2D _light;
        [SerializeField] private Transform _parentText;
        [SerializeField] private Color _startColor;
        [SerializeField] private Color[] _colors = new[] { Color.yellow, Color.magenta, Color.red };
        [SerializeField, Range(0.1f, 1.5f)] private float _durationLerpColor = 0.1f;
        [SerializeField] private float _durationFadeScore = 1f;
        [SerializeField] private float _durationShowScore = 1f;
        [SerializeField] private float _durationMoveScore = 1f;
        [SerializeField] private float _minXScore = 0.8f;
        [SerializeField] private float _maxXScore = 1f;
        [SerializeField] private float _minYScore = -0.8f;
        [SerializeField] private float _maxYScore = 3f;
        [SerializeField] private Ease _easing = Ease.OutSine;

        private Stack<TextMeshPro> _texts = new(15);

        private int _countColors = 3;
        private int _currentColor;
        private float _coeffStopColorLerp = 3f;
        #endregion

        #region UnityMethods
        private void OnEnable()
        {
            GameLevel.Instance.ChangeLevel += OnChangeLevel;
            GameLevel.Instance.GameOver += OnGameOver;
        }

        private void OnDisable()
        {
            GameLevel.Instance.ChangeLevel -= OnChangeLevel;
            GameLevel.Instance.GameOver -= OnGameOver;
        }

        private void Awake()
        {
            GameLevel.Instance.CreateGameSession += OnCreateGameSession;
            InitTextMesseges();
            OnCreateGameSession();
        }

        private void OnDestroy()
        {
            GameLevel.Instance.CreateGameSession -= OnCreateGameSession;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out IFallingObject fallingObject))
            {
                fallingObject.ApplyMod();
            }
            if (collision.TryGetComponent(out IScore score))
            {
                if (score.Score == 0) return;
                GameLevel.Instance.ChargePoints(score.Score);
                ShowScore(score.Score);
            }
        }
        #endregion

        #region Private Methods
        private void OnCreateGameSession()
        {
            _light.color = _startColor;
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

        private void ShowScore(int score)
        {
            if (_texts.Count <= 0) return;
            TextMeshPro text = _texts.Pop();

            //text.gameObject.transform.localScale = Vector3.one;
            text.gameObject.transform.position = new Vector2(transform.position.x, transform.position.y);
            text.text = score.ToString();
            text.gameObject.SetActive(true);
            text.gameObject.transform.DOMove(
                new Vector2(Random.Range(_minXScore, _maxXScore), 
                Random.Range(_minYScore, _maxYScore)), _durationMoveScore).SetEase(_easing);
            text.DOFade(0, _durationFadeScore).OnComplete(() =>
            {
                text.gameObject.SetActive(false);
                text.gameObject.transform.position = _parentText.position;
                text.color = new Color(text.color.r, text.color.g, text.color.b, 255f);
                _texts.Push(text);
            });
        }

        private void InitTextMesseges()
        {
            for (int i = 0; i < 15; i++)
            {
                _texts.Push(Instantiate(_textScore, _parentText));
            }
            AllTextsHide();
        }

        private void AllTextsHide()
        {
            foreach (TextMeshPro text in _texts)
            {
                text.gameObject.SetActive(false);
                text.gameObject.transform.position = _parentText.position;
                text.color = new Color(text.color.r, text.color.g, text.color.b, 255f);
            }
        }

        private void OnGameOver()
        {
            AllTextsHide();
        }
        #endregion
    }
}
