using DG.Tweening;
using System.Collections.Generic;
using TouchFall.Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace TouchFall.View.UI
{
    public sealed class UIGamePlayDispatcher : MonoBehaviour
    {
        #region Fields
        [SerializeField] private List<Image> _lifes;
        [SerializeField] private Button _pauseBtn;
        [SerializeField] private Button _resumeBtn;

        [SerializeField] private float _durationEnterBtn = 0.3f;
        [SerializeField] private float _scaleBtn = 1.2f;

        [SerializeField] private float _durationHidePanel = 0.4f; 

        private RectTransform _gameInformationPanel;
        private Tween _tweenMouseEnter;

        private float _heightPanel;
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

        #region Private Methods
        private void OnDamage()
        {

        }

        private void OnEarnPoints(int points)
        {

        }

        private void OnCreateGameSession()
        {
            ShowPanel();
        }

        private void Pause()
        {
            GameLoop.Instance.Pause();
            //TODO заблюрить главный экран
            //TODO отобразить меню паузы
            HidePanel();
        }

        private void Resume()
        {
            //TODO скрыть меню паузы
            //TODO разблюрить экран
            ShowPanel();
        }

        private void OnPauseBegin(bool pause)
        {
            if (pause) return;

            ShowPanel();
        }

        private void OnGameOver()
        {
            HidePanel();
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

        private void HidePanel()
        {
            _gameInformationPanel.DOAnchorPosY(_heightPanel * -1, _durationHidePanel)
                .SetUpdate(UpdateType.Normal, true).SetEase(Ease.InBack);
        }

        private void ShowPanel()
        {
            _gameInformationPanel.DOAnchorPosY(0, _durationHidePanel).SetEase(Ease.OutBack);
        }
        #endregion
    }
}
