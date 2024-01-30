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
        [SerializeField] private AudioSource _sounds;
        //[SerializeField] private AudioMix

        [Space(5f), Header("Musics")]
        [SerializeField] private AudioClip[] _musicsMenu;
        [SerializeField] private AudioClip[] _musicsGamePlay;

        [SerializeField] private AudioClip _gameOver;
        [SerializeField] private AudioClip _damage;
        [SerializeField] private AudioClip _extraLife;
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
        }
        #endregion

        #region Public Methods
        public void EnableSounds(bool isEnable)
        {

        }

        public void PlaySound(AudioClip clip)
        {
            _sounds.clip = clip;
            _sounds.Play();
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
        #endregion
    }
}