using System.Runtime.InteropServices;
using TouchFall.Singletons;
using TouchFall.View.UI;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace TouchFall.Helper
{
    public sealed class Yandex : MonoBehaviour
    {
        #region Fields
        [SerializeField] private UIMainMenuDispatcher _mainMenu;
        #endregion

        #region Extern Methods
        [DllImport("__Internal")]
        private static extern void RateGame();

        [DllImport("__Internal")]
        private static extern void CanRateGame();

        [DllImport("__Internal")]
        private static extern void SaveExtern(string data);

        [DllImport("__Internal")]
        private static extern void LoadExtern();

        [DllImport("__Internal")]
        private static extern void SetToLeaderBoard(int value);

        [DllImport("__Internal")]
        private static extern string GetLang();

        [DllImport("__Internal")]
        private static extern void ShowAdv();

        [DllImport("__Internal")]
        private static extern void ShowAdvRewarded();
        #endregion

        #region Public Methods
        public void RateGameAction()
        {
#if UNITY_EDITOR
            return;
#endif
#if UNITY_WEBGL
            RateGame();
#endif
        }

        public void CanRate()
        {
#if UNITY_EDITOR
            return;
#endif
#if UNITY_WEBGL
            CanRateGame(); 
#endif
        }

        public void Save(string data)
        {
#if UNITY_EDITOR
            return;
#endif
#if UNITY_WEBGL
            SaveExtern(data);
#endif
        }

        public void Load()
        {
#if UNITY_EDITOR
            return;
#endif
#if UNITY_WEBGL
            LoadExtern();
#endif
        }

        public void SaveLeaderboard(int value)
        {
#if UNITY_EDITOR
            return;
#endif
#if UNITY_WEBGL
            SetToLeaderBoard(value);
#endif
        }

        public void ShowAdvirtisment()
        {
#if UNITY_EDITOR
            return;
#endif
#if UNITY_WEBGL
            ShowAdv();
#endif
        }

        public void ShowRewarded()
        {
#if UNITY_EDITOR
            return;
#endif
#if UNITY_WEBGL
            ShowAdvRewarded();
#endif
        }
        
        /// <summary>
        /// ���������� �� jslib
        /// </summary>
        public void LoadData(string data)
        {
            GameData.Instance.Load(data);

            if (GameData.Instance.SaveData.isChangeLang)
            {
                LocalizationSettings.InitializationOperation.Completed += e =>
                {
                    LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[(int)GameData.Instance.SaveData.language];
                };
            }
            else
            {
#if UNITY_WEBGL
                string language = GetLang();
                GameData.Instance.Localize(language);
#endif
            }

            _mainMenu.ShowBestPoints(GameData.Instance.SaveData.scores > 0);
            _mainMenu.UpdateBestBoint(GameData.Instance.SaveData.scores);
            _mainMenu.SetMusicImage();
            GameAudio.instance.EnableSounds();
            CanRate();
        }

        /// <summary>
        /// ���������� �� jslib
        /// </summary>
        public void CanRateGameCallback(int canRate)
        {
            _mainMenu.CanRateGame(canRate);
        }

        /// <summary>
        /// ���������� �� jslib
        /// </summary>
        public async void StartGame()
        {
            await _mainMenu.StartGameAfterAdvirtisment();
        }

        /// <summary>
        /// ���������� �� jslib
        /// </summary>
        public async void AfterRewarded()
        {
            await _mainMenu.ResumeGameAfterRewarded();
        }

        /// <summary>
        /// ���������� �� jslib
        /// </summary>
        public void UnLockBtns()
        {
            _mainMenu.UnLockBtns();
        }
        #endregion
    }
}
