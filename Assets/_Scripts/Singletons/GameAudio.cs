using UnityEngine;

namespace TouchFall.Singletons
{
    public class GameAudio : MonoBehaviour
    {
        #region Singleton
        public static GameAudio instance;
        #endregion

        #region SerializeFields
        [SerializeField] private AudioSource _music;
        [SerializeField] private AudioSource _soundBounds;
        [SerializeField] private AudioSource _soundHero;
        [SerializeField] private AudioSource _soundOther;

        [Space(5f), Header("Musics")]
        [SerializeField] private AudioClip[] _musicsMenu;
        [SerializeField] private AudioClip[] _musicsGamePlay;

        [SerializeField] private AudioClip _gameOver;
        [SerializeField] private AudioClip _damage;
        [SerializeField] private AudioClip _extraLife;
        [SerializeField] private AudioClip _emptyBonus;
        #endregion

        #region Unity Methods
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            PlayMusicMenu();
        }
        #endregion

        #region Public Methods
        public void EnableSounds()
        {
            bool onOff = !GameData.Instance.SaveData.isOnMusic;
            _music.mute = onOff;
            _soundBounds.mute = onOff;
            _soundHero.mute = onOff;
            _soundOther.mute = onOff;
        }

        public void DisableSounds()
        {
            _music.mute = true;
            _soundBounds.mute = true;
            _soundHero.mute = true;
            _soundOther.mute = true;
        }

        public void PlaySound(AudioClip clip)
        {
            _soundOther.clip = clip;
            _soundOther.Play();
        }

        public void PlaySoundBounds(AudioClip clip)
        {
            _soundBounds.clip = clip;
            _soundBounds.Play();
        }

        public void PlaySoundHero(AudioClip clip)
        {
            _soundHero.clip = clip;
            _soundHero.Play();
        }

        public void Pause(bool isPause)
        {
            if (isPause) _music.Pause();
            else _music.UnPause();
        }

        public void PlayMusicMenu()
        {
            _music.clip = _musicsMenu[Random.Range(0, _musicsMenu.Length - 1)];
            _music.Play();
        }

        public void PlayMusicGamePlay(int level)
        {
            if (level > _musicsGamePlay.Length) return;
            _music.clip = _musicsGamePlay[level - 1];
            _music.Play();
        }

        public void PlayGameOver()
        {
            PlaySound(_gameOver);
        }

        public void PlayDamge()
        {
            PlaySound(_damage);
        }

        public void PlayExtraLife()
        {
            PlaySound(_extraLife);
        }

        public void PlayEmptyBonus()
        {
            PlaySound(_emptyBonus);
        }
        #endregion
    }
}