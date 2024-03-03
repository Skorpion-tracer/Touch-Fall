using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using TouchFall.Singletons;
using UnityEngine;
using UnityEngine.UI;

namespace TouchFall.View.UI
{
    public sealed class UIGamePlayDispatcher : MonoBehaviour
    {
        #region Fields
        [SerializeField] private RectTransform _gameInformationPanel;

        [Space(10f)]
        [SerializeField] private List<RectTransform> _lifes;
        [SerializeField] private Button _pauseBtn;

        [SerializeField] private float _durationEnterBtn = 0.3f;
        [SerializeField] private float _scaleBtn = 1.2f;

        [SerializeField] private float _durationHidePanel = 0.4f;

        [Space(10f)]
        [SerializeField] private float _durationShakeLife = 1.2f;
        [SerializeField] private Vector3 _punchShakeLife = Vector3.zero;

        [Space(10f)]
        [SerializeField] private TextMeshProUGUI _pointsText;
        
        private Tween _tweenMouseEnter;
        private Tween _tweenPause;
        private List<Tween> _tweensLifes = new(3);

        private float _heightPanel;
        private int _indexLife = 0;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            _pointsText.text = "0";

            _heightPanel = _gameInformationPanel.rect.yMin;

            _gameInformationPanel.anchoredPosition = new Vector2(_gameInformationPanel.anchoredPosition.x, _heightPanel * -1);

            _pointsText.gameObject.transform.DOScale(_punchShakeLife, _durationShakeLife).SetEase(Ease.InOutFlash).SetLoops(-1, LoopType.Yoyo);
        }

        private void OnEnable()
        {
            GameLevel.Instance.Damage += OnDamage;
            GameLevel.Instance.EarnPoints += OnEarnPoints;
            GameLevel.Instance.CreateGameSession += OnCreateGameSession;
            GameLevel.Instance.GameOver += OnGameOver;
            GameLoop.Instance.PauseBegin += OnPauseBegin;
            GameLevel.Instance.SetNewLife += OnNewLife;
            GameLevel.Instance.ExtraLife += OnExtraLife;
            _pauseBtn.onClick.AddListener(Pause);
        }

        private void OnDisable()
        {
            GameLevel.Instance.Damage -= OnDamage;
            GameLevel.Instance.EarnPoints -= OnEarnPoints;
            GameLevel.Instance.CreateGameSession -= OnCreateGameSession;
            GameLevel.Instance.GameOver -= OnGameOver;
            GameLoop.Instance.PauseBegin -= OnPauseBegin;
            GameLevel.Instance.SetNewLife -= OnNewLife;
            GameLevel.Instance.ExtraLife -= OnExtraLife;
            _pauseBtn.onClick.RemoveListener(Pause);
        }
        #endregion

        #region Public Methods
        public void ButtonMouseEnter()
        {
            _tweenMouseEnter = _pauseBtn.gameObject.transform.DOScale(_scaleBtn, _durationEnterBtn).SetLoops(-1, LoopType.Yoyo);
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
            _pauseBtn.interactable = false;
            _tweenPause = _gameInformationPanel.DOAnchorPosY(_heightPanel * -1, _durationHidePanel).SetEase(Ease.InBack).OnComplete(() =>
            {
                _gameInformationPanel.gameObject.SetActive(false);
                _pauseBtn.interactable = true;
            });
        }

        private void ShowPanel()
        {
            _tweenPause?.Kill();
            _gameInformationPanel.gameObject.SetActive(true);
            _gameInformationPanel.DOAnchorPosY(0, _durationHidePanel).SetEase(Ease.OutBack);
        }

        private async void OnNewLife()
        {
            await AddLife(1);
        }

        private async void OnExtraLife()
        {
            await AddLife(2);
        }

        private async void OnDamage()
        {
            if (_tweensLifes.Count == 0) return;
            _tweensLifes[0].Kill();
            _tweensLifes.RemoveAt(0);
            await _lifes[_indexLife].DOScale(0f, _durationShakeLife).SetEase(Ease.InBack).AsyncWaitForCompletion();
            _lifes[_indexLife].gameObject.SetActive(false);
            _indexLife++;
            if (_indexLife > _lifes.Count) _indexLife = _lifes.Count;
        }

        private void OnEarnPoints(int points)
        {
            _pointsText.text = points.ToString();
        }

        private void OnCreateGameSession()
        {
            _pointsText.text = "0";
            ShowLifes();
            ShowPanel();
        }

        private void Pause()
        {
            GameLoop.Instance.Pause();
        }

        private void OnPauseBegin(bool pause)
        {
            if (pause)
            {
                HidePanel();
            }
            else
            {
                ShowPanel();
            }
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

        private async Task AddLife(int index)
        {
            _lifes[index].gameObject.SetActive(true);
            await _lifes[index].DOScale(1f, _durationShakeLife).SetEase(Ease.OutBack).AsyncWaitForCompletion();
            Tween tween = _lifes[index].DOScale(_punchShakeLife, _durationShakeLife).SetEase(Ease.InOutFlash).SetLoops(-1, LoopType.Yoyo);
            _tweensLifes.Add(tween);
            _indexLife = index;
        }
        #endregion
    }
}
