using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Threading.Tasks;
using TouchFall.Singletons;
using System;

namespace TouchFall.View.UI
{
    public sealed class UIMainMenuDispatcher : MonoBehaviour
    {
        #region Fields
        [SerializeField] private RectTransform _mainPanel;

        [Space(10f)]
        [SerializeField] private RectTransform _mainMenu;
        [SerializeField] private RectTransform _menuPause;
        [SerializeField] private RectTransform _menuGameOver;
        [SerializeField] private RectTransform _menuSettings;
        [SerializeField] private RectTransform _menuExit;

        [Space(10f)]
        [SerializeField] private Button _startGame;
        [SerializeField] private Button _settings;
        [SerializeField] private Button _exit;
        [SerializeField] private Button _exitToMainMenu;
        [SerializeField] private Button _exitConfirm;

        [Space(10f)]
        [SerializeField] private float _durationMovePanels = 0.4f;
        [SerializeField] private float _durationScaleBtns = 0.7f;
        [SerializeField] private float _scaleBtn = 1.2f;

        private Tween _tweenMouseEnter;

        private float _widthContainer;
        private float _heighContainer;
        #endregion

        #region UnityMethods
        private void OnEnable()
        {
            GameLoop.Instance.PauseBegin += OnPauseBegin;
            Debug.Log("Подписка");
        }

        private void OnDisable()
        {
            GameLoop.Instance.PauseBegin -= OnPauseBegin;
        }

        private async void Start()
        {
            _widthContainer = _mainPanel.rect.xMin;
            _heighContainer = _mainPanel.rect.yMin;

            _mainMenu.gameObject.SetActive(false);
            _mainMenu.anchoredPosition = new Vector2(_mainMenu.anchoredPosition.x, _heighContainer + _mainMenu.rect.yMin);
            _menuPause.anchoredPosition = new Vector2(_menuPause.anchoredPosition.x, _heighContainer + _menuPause.rect.yMin);

            await Task.Delay(1000);
            _mainMenu.gameObject.SetActive(true);
            _mainMenu.DOAnchorPosY(1f, _durationMovePanels).SetEase(Ease.OutBack);
        }
        #endregion

        #region Public Methods
        public async void StartGame()
        {
            await _mainMenu.DOAnchorPosY(_heighContainer + _mainMenu.rect.yMin, _durationMovePanels).SetEase(Ease.InBack).AsyncWaitForCompletion();
            GameLevel.Instance.NewGame();
        }

        public void BtnMouseEnter(Button btn)
        {
            BtnMousEnter(btn);
        }

        public void BtnMouseExit(Button btn)
        {
            BtnMousExit(btn);
        }

        public async void ResumeGame()
        {
            await _menuPause.DOAnchorPosY(_heighContainer + _menuPause.rect.yMin, _durationMovePanels)
                    .SetUpdate(UpdateType.Normal, true).SetEase(Ease.OutBack).AsyncWaitForCompletion();
            GameLoop.Instance.Resume();
            _menuPause.gameObject.SetActive(false);
        }
        #endregion

        #region Private Methods
        private void BtnMousEnter(Button btn)
        {
            _tweenMouseEnter = btn.gameObject.transform.DOScale(_scaleBtn, _durationScaleBtns)
                .SetUpdate(UpdateType.Normal, true).SetLoops(-1, LoopType.Yoyo);
        }

        private void BtnMousExit(Button btn)
        {
            _tweenMouseEnter.Kill();
            btn.gameObject.transform.DOScale(1f, _durationScaleBtns).SetUpdate(UpdateType.Normal, true);
        }

        private void OnPauseBegin(bool pause)
        {
            if (pause)
            {
                _menuPause.gameObject.SetActive(true);
                _menuPause.DOAnchorPosY(1f, _durationMovePanels)
                    .SetUpdate(UpdateType.Normal, true).SetEase(Ease.OutBack);
                Debug.Log("Пауза");
            }
        }
        #endregion
    }
}
