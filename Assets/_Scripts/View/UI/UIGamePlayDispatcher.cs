using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using TouchFall.Helper.Enums;
using TouchFall.Input;
using TouchFall.Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace TouchFall.View.UI
{
    public sealed class UIGamePlayDispatcher : MonoBehaviour
    {
        #region Fields
        [SerializeField] private List<RectTransform> _lifes;
        [SerializeField] private Button _pauseBtn;
        [SerializeField] private Button _resumeBtn;

        [SerializeField] private float _durationEnterBtn = 0.3f;
        [SerializeField] private float _scaleBtn = 1.2f;

        [SerializeField] private float _durationHidePanel = 0.4f;

        [Space(10f)]
        [SerializeField] private float _durationShakeLife = 1.2f; 
        [SerializeField] private float _strengthShakeLife = 1f; 
        [SerializeField] private int _vibratoShakeLife = 5; 
        [SerializeField] private float _randomnessShakeLife = 90f; 
        [SerializeField] private bool _fadeShakeLife = true; 
        [SerializeField] private Vector3 _punchShakeLife = Vector3.zero; 

        private RectTransform _gameInformationPanel;
        private Tween _tweenMouseEnter;
        private List<Tween> _tweensLifes = new(3);

        private float _heightPanel;
        private int _indexLife = 0;
        #endregion

        #region Unity Methods
        private void OnValidate()
        {
            _gameInformationPanel ??= GetComponent<RectTransform>();
        }

        private void Start()
        {
            _heightPanel = _gameInformationPanel.rect.yMin;

            _gameInformationPanel.anchoredPosition = new Vector2(_gameInformationPanel.anchoredPosition.x, _heightPanel * -1);
        }

        private void OnEnable()
        {
            GameLevel.Instance.Damage += OnDamage;
            GameLevel.Instance.EarnPoints += OnEarnPoints;
            GameLevel.Instance.CreateGameSession += OnCreateGameSession;
            GameLevel.Instance.GameOver += OnGameOver;
            GameLoop.Instance.PauseBegin += OnPauseBegin;
            _pauseBtn.onClick.AddListener(Pause);
        }

        private void OnDisable()
        {
            GameLevel.Instance.Damage -= OnDamage;
            GameLevel.Instance.EarnPoints -= OnEarnPoints;
            GameLevel.Instance.CreateGameSession -= OnCreateGameSession;
            GameLevel.Instance.GameOver -= OnGameOver;
            GameLoop.Instance.PauseBegin -= OnPauseBegin;
            _pauseBtn.onClick.RemoveListener(Pause);
        }
        #endregion

        #region Public Methods
        public void ButtonMouseEnter()
        {
            _tweenMouseEnter = _pauseBtn.gameObject.transform.DOScale(_scaleBtn, _durationEnterBtn)
                .SetUpdate(UpdateType.Normal, true).SetLoops(-1, LoopType.Yoyo);
        }

        public void ButtonMouseExit()
        {
            _tweenMouseEnter.Kill();
            _pauseBtn.gameObject.transform.DOScale(1f, _durationEnterBtn);
        }
        #endregion

        #region Private Methods
        private void HidePanel()
        {
            _gameInformationPanel.DOAnchorPosY(_heightPanel * -1, _durationHidePanel)
                .SetUpdate(UpdateType.Normal, true).SetEase(Ease.InBack);
        }

        private void ShowPanel()
        {
            _gameInformationPanel.DOAnchorPosY(0, _durationHidePanel).SetEase(Ease.OutBack);
        }

        private void OnDamage()
        {
            _tweensLifes[0].Kill();
            _tweensLifes.RemoveAt(0);
            _lifes[_indexLife].gameObject.SetActive(false);
            _indexLife++;
        }

        private void OnEarnPoints(int points)
        {

        }

        private void OnCreateGameSession()
        {
            ShowLifes();
            ShowPanel();            
        }

        private void Pause()
        {
            GameLoop.Instance.Pause();
            HidePanel();
        }

        private void OnPauseBegin(bool pause)
        {
            if (pause) return;

            ShowPanel();
        }

        private void OnGameOver()
        {
            for (int i = 0; i < _tweensLifes.Count; i++)
            {
                _tweensLifes[i].Kill();
            }
            for (int i = 0; i < _lifes.Count; i++)
            {
                _lifes[i].gameObject.SetActive(false);
            }
            HidePanel();
        }

        private void ShowLifes()
        {
            _tweensLifes.Clear();
            for (int i = 0; i < _lifes.Count; i++)
            {
                Tween tween = _lifes[i].DOScale(_punchShakeLife, _durationShakeLife).SetEase(Ease.InOutFlash).SetLoops(-1, LoopType.Yoyo);
                _tweensLifes.Add(tween);
            }
            for (int i = 0; i < _lifes.Count; i++)
            {
                _lifes[i].localScale = Vector3.one;
                _lifes[i].gameObject.SetActive(true);
            }
            _indexLife = 0;
        }
        #endregion
    }
}
