using DG.Tweening;
using System.Threading.Tasks;
using TouchFall.Singletons;
using UnityEngine;
using UnityEngine.UI;

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
        [SerializeField] private RectTransform _menuExit;

        [Space(10f)]
        [SerializeField] private Button _btnAdvirtisment;
        [SerializeField] private RectTransform _panelPublicity;

        [Space(10f)]
        [SerializeField] private float _durationMovePanels = 0.4f;
        [SerializeField] private float _durationScaleBtns = 0.7f;
        [SerializeField] private float _scaleBtn = 1.2f;

        private Tween _tweenMouseEnter;
        private RectTransform _activePanel;

        private float _heighContainer;
        #endregion

        #region UnityMethods
        private void OnEnable()
        {
            GameLoop.Instance.PauseBegin += OnPauseBegin;
            GameLevel.Instance.GameOver += OnGameOver;
        }

        private void OnDisable()
        {
            GameLoop.Instance.PauseBegin -= OnPauseBegin;
            GameLevel.Instance.GameOver -= OnGameOver;
        }

        private async void Awake()
        {
            _heighContainer = _mainPanel.rect.yMin;

            await ResetPanels();

            _btnAdvirtisment.gameObject.transform.DOScale(_scaleBtn, _durationScaleBtns).SetLoops(-1, LoopType.Yoyo);
            _panelPublicity.gameObject.transform.DOScale(_scaleBtn, _durationScaleBtns).SetLoops(-1, LoopType.Yoyo);
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
            await _menuPause.DOAnchorPosY(_heighContainer + _menuPause.rect.yMin, _durationMovePanels).SetEase(Ease.OutBack).AsyncWaitForCompletion();
            _menuPause.gameObject.SetActive(false);

            GameLoop.Instance.Resume();
        }

        public async void ShowAdvirtisemnet()
        {
            // TODO Вызывать показ рекламы

            await _menuGameOver.DOScale(0f, _durationMovePanels).SetEase(Ease.OutBack).AsyncWaitForCompletion();
            _menuGameOver.gameObject.SetActive(false);

            GameLevel.Instance.SetExtraLife();
            GameLoop.Instance.ResumeAdvertisment();
        }

        public async void ExitToMainMenu()
        {
            GameLevel.Instance.ExitToMainMenu();

            await _activePanel.DOScale(0f, _durationMovePanels).SetEase(Ease.InBack).AsyncWaitForCompletion();
            _activePanel.gameObject.SetActive(false);

            await ResetPanels();
        }

        public async void QuiteGame()
        {
            await _mainMenu.DOScale(0f, _durationMovePanels).SetEase(Ease.InBack).AsyncWaitForCompletion();
            _mainMenu.gameObject.SetActive(false);

            _menuExit.gameObject.SetActive(true);
            await _menuExit.DOScale(1f, _durationMovePanels).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        }

        public async void CancelQuit()
        {
            await _menuExit.DOScale(0f, _durationMovePanels).SetEase(Ease.InBack).AsyncWaitForCompletion();
            _menuExit.gameObject.SetActive(false);

            _mainMenu.gameObject.SetActive(true);
            await _mainMenu.DOScale(1f, _durationMovePanels).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        }

        public async void RestartGame()
        {
            await _menuGameOver.DOScale(0f, _durationMovePanels).SetEase(Ease.InBack).AsyncWaitForCompletion();
            _menuGameOver.gameObject.SetActive(false);

            GameLevel.Instance.NewGame();
        }

        public void Exit()
        {
            Application.Quit();
        }
        #endregion

        #region Private Methods
        private void BtnMousEnter(Button btn)
        {
            _tweenMouseEnter = btn.gameObject.transform.DOScale(_scaleBtn, _durationScaleBtns).SetLoops(-1, LoopType.Yoyo);
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
                _menuPause.DOAnchorPosY(1f, _durationMovePanels).SetEase(Ease.OutBack);
                _activePanel = _menuPause;
            }
        }

        private async void OnGameOver()
        {
            if (!GameLevel.Instance.IsCanUseExtraLife)
            {
                _panelPublicity.gameObject.SetActive(false);
            }
            _activePanel = _menuGameOver;
            _menuGameOver.gameObject.SetActive(true);
            await _menuGameOver.DOScale(1f, _durationMovePanels).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        }

        private async Task ResetPanels()
        {
            _mainMenu.gameObject.SetActive(false);
            _mainMenu.anchoredPosition = new Vector2(_mainMenu.anchoredPosition.x, _heighContainer + _mainMenu.rect.yMin);
            _menuPause.localScale = Vector3.one;
            _menuPause.anchoredPosition = new Vector2(_menuPause.anchoredPosition.x, _heighContainer + _menuPause.rect.yMin);

            _menuExit.localScale = Vector3.zero;
            _menuExit.gameObject.SetActive(false);

            _menuGameOver.localScale = Vector2.zero;
            _menuGameOver.gameObject.SetActive(false);

            _panelPublicity.gameObject.SetActive(true);

            await Task.Delay(1000);
            _mainMenu.gameObject.SetActive(true);
            await _mainMenu.DOAnchorPosY(1f, _durationMovePanels).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        }
        #endregion
    }
}
