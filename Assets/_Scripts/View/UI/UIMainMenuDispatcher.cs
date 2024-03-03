using DG.Tweening;
using System;
using System.Threading.Tasks;
using TMPro;
using TouchFall.Helper;
using TouchFall.Helper.Enums;
using TouchFall.Singletons;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

namespace TouchFall.View.UI
{
    public sealed class UIMainMenuDispatcher : MonoBehaviour
    {
        #region Fields
        [SerializeField] private Yandex _yandex;

        [Space(10f)]
        [SerializeField] private RectTransform _mainPanel;

        [Space(10f)]
        [SerializeField] private RectTransform _mainMenu;
        [SerializeField] private RectTransform _menuPause;
        [SerializeField] private RectTransform _menuGameOver;
        [SerializeField] private RectTransform _menuExit;
        [SerializeField] private RectTransform _menuTutorial;
        
        [Space(10f)]
        [SerializeField] private Image _imageMusicStartmenu;
        [SerializeField] private Image _imageMusicPause;
        [SerializeField] private Sprite _musicOn;
        [SerializeField] private Sprite _musicOff;
        [SerializeField] private AudioClip _soundTap;

        [Space(10f)]
        [SerializeField] private Button _btnAdvirtisment;
        [SerializeField] private RectTransform _panelPublicity;
        [SerializeField] private Button _btnRateGame;
        [SerializeField] private Button _btnRestartGameOver;
        [SerializeField] private Button _btnExitMenuGameOver;

        [Space(10f)]
        [SerializeField] private RectTransform _panelBestPoints;
        [SerializeField] private TextMeshProUGUI _textCountPoints;

        [Space(10f)]
        [SerializeField] private float _durationMovePanels = 0.4f;
        [SerializeField] private float _durationScaleBtns = 0.7f;
        [SerializeField] private float _scaleBtn = 1.2f;

        private Tween _tweenMouseEnter;
        private RectTransform _activePanel;
        private Button _buttonStartGame;

        private float _heighContainer;

        private int _countShowAdv = 0;
        private int _maxShowAdv = 2;

        private bool _isStart;
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
            _btnRateGame.gameObject.transform.DOScale(_scaleBtn, _durationScaleBtns).SetLoops(-1, LoopType.Yoyo);
        }
        #endregion

        #region Public Methods

        public async void StartGame(Button button)
        {
            _buttonStartGame = button;
            _buttonStartGame.interactable = false;

            GameAudio.instance.PlaySound(_soundTap);

            _isStart = true;

            if (IsShowAdvertisment()) return;

            await StartGameAfterAdvirtisment();
        }

        public async Task StartGameAfterAdvirtisment()
        {
            if (_isStart)
            {
                await _mainMenu.DOAnchorPosY(_heighContainer + _mainMenu.rect.yMin, _durationMovePanels).SetEase(Ease.InBack).AsyncWaitForCompletion();
            }
            else
            {
                await _menuGameOver.DOScale(0f, _durationMovePanels).SetEase(Ease.InBack).AsyncWaitForCompletion();
                _menuGameOver.gameObject.SetActive(false);
            }

            _buttonStartGame.interactable = true;

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

        public async void ResumeGame(Button button)
        {
            button.interactable = false;

            GameAudio.instance.PlaySound(_soundTap);
            await _menuPause.DOAnchorPosY(_heighContainer + _menuPause.rect.yMin, _durationMovePanels).SetEase(Ease.InBack).AsyncWaitForCompletion();
            _menuPause.gameObject.SetActive(false);

            GameLoop.Instance.Resume();

            button.interactable = true;
        }

        public async void ShowAdvirtisemnet()
        {
            _btnAdvirtisment.interactable = false;
            _btnRestartGameOver.interactable = false;
            _btnExitMenuGameOver.interactable = false;

            GameAudio.instance.PlaySound(_soundTap);

            if (GameLevel.Instance.IsCanUseExtraLife)
            {
                _yandex.ShowRewarded();
                return;
            }

            await ResumeGameAfterRewarded();
        }

        public async Task ResumeGameAfterRewarded()
        {
            await _menuGameOver.DOScale(0f, _durationMovePanels).SetEase(Ease.OutBack).AsyncWaitForCompletion();
            _menuGameOver.gameObject.SetActive(false);

            GameLevel.Instance.SetExtraLife();
            GameLoop.Instance.ResumeAdvertisment();

            UnLockBtns();
        }

        public void UnLockBtns()
        {
            _btnAdvirtisment.interactable = true;
            _btnRestartGameOver.interactable = true;
            _btnExitMenuGameOver.interactable = true;
        }

        public async void ExitToMainMenu(Button button)
        {
            button.interactable = false;

            GameAudio.instance.PlaySound(_soundTap);
            GameLevel.Instance.ExitToMainMenu();

            await _activePanel.DOScale(0f, _durationMovePanels).SetEase(Ease.InBack).AsyncWaitForCompletion();
            _activePanel.gameObject.SetActive(false);

            await ResetPanels();

            button.interactable = true;
        }

        public void SetShowTotorial(bool isShow)
        {
            _menuTutorial.gameObject.SetActive(isShow);
            _tweenMouseEnter?.Restart();
            _tweenMouseEnter?.Complete();
            _tweenMouseEnter?.Kill();
        }

        public async void QuiteGame()
        {
            GameAudio.instance.PlaySound(_soundTap);

            await _mainMenu.DOScale(0f, _durationMovePanels).SetEase(Ease.InBack).AsyncWaitForCompletion();
            _mainMenu.gameObject.SetActive(false);

            _menuExit.gameObject.SetActive(true);
            await _menuExit.DOScale(1f, _durationMovePanels).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        }

        public async void CancelQuit()
        {
            GameAudio.instance.PlaySound(_soundTap);

            await _menuExit.DOScale(0f, _durationMovePanels).SetEase(Ease.InBack).AsyncWaitForCompletion();
            _menuExit.gameObject.SetActive(false);

            _mainMenu.gameObject.SetActive(true);
            await _mainMenu.DOScale(1f, _durationMovePanels).SetEase(Ease.OutBack).AsyncWaitForCompletion();
        }

        public async void RestartGame(Button button)
        {
            _buttonStartGame = button;
            _buttonStartGame.interactable = false;

            GameAudio.instance.PlaySound(_soundTap);

            _isStart = false;

            if (IsShowAdvertisment()) return;

            await StartGameAfterAdvirtisment();
        }

        public void MusicOnOff()
        {
            GameData.Instance.Save(!GameData.Instance.SaveData.isOnMusic);
            GameAudio.instance.EnableSounds();
            SetMusicImage();
        }

        public void CanRateGame(int canRate)
        {
            bool result = Convert.ToBoolean(canRate);
            _btnRateGame.gameObject.SetActive(result);
        }

        public void RateGame()
        {
            _btnRateGame.interactable = false;
            _yandex.RateGameAction();
            _btnRateGame.interactable = true;
        }

        public void ShowBestPoints(bool isShow)
        {
            _panelBestPoints.gameObject.SetActive(isShow);
        }

        public void UpdateBestBoint(int score)
        {
            _textCountPoints.text = score.ToString();
        }

        public void ChangeLang()
        {
            Language currentLang = GameData.Instance.SaveData.language;
            GameData.Instance.SaveData.language = currentLang == Language.English ? Language.Russian : Language.English;
            GameData.Instance.Save(GameData.Instance.SaveData.language);

            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)GameData.Instance.SaveData.language];
        }

        public void SetMusicImage()
        {
            if (GameData.Instance.SaveData.isOnMusic)
            {
                _imageMusicStartmenu.sprite = _musicOn;
                _imageMusicPause.sprite = _musicOn;
            }
            else
            {
                _imageMusicStartmenu.sprite = _musicOff;
                _imageMusicPause.sprite = _musicOff;
            }
        }
        #endregion

        #region Private Methods
        private bool IsShowAdvertisment()
        {
            _countShowAdv++;

            if (_countShowAdv >= _maxShowAdv && GameLevel.Instance.TimerGame >= 60f)
            {
                _yandex.ShowAdvirtisment();
                _countShowAdv = 0;
                GameLevel.Instance.TimerGame = 0f;
                return true;
            }

            return false;
        }

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
            _panelPublicity.gameObject.SetActive(GameLevel.Instance.IsCanUseExtraLife);
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

            _mainMenu.gameObject.SetActive(true);
            await _mainMenu.DOAnchorPosY(1f, _durationMovePanels).SetEase(Ease.OutBack).SetDelay(1.3f).AsyncWaitForCompletion();
        }
        #endregion
    }
}
