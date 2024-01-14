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

        [SerializeField] private float _durationEnterBtn = 0.3f;
        [SerializeField] private float _scaleBtn = 1.2f;

        private Vector2 _rotateBtn = new Vector3(0, 0, 150f);

        private RectTransform _gameInformationPanel;
        #endregion

        #region Unity Methods
        private void OnValidate()
        {
            _gameInformationPanel ??= GetComponent<RectTransform>();
        }

        private void Start()
        {

        }

        private void OnEnable()
        {
            GameLevel.Instance.Damage += OnDamage;
            GamePlay.Instance.EarnPoints += OnEarnPoints;
            _pauseBtn.onClick.AddListener(Pause);
        }

        private void OnDisable()
        {
            GameLevel.Instance.Damage -= OnDamage;
            GamePlay.Instance.EarnPoints -= OnEarnPoints;
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

        private async void Pause()
        {
            Debug.Log("Пауза");
        }
        #endregion

        #region Public Methods
        public void ButtonMouseEnter()
        {
            _pauseBtn.gameObject.transform.DOScale(_scaleBtn, _durationEnterBtn);
        }

        public void ButtonMouseExit()
        {
            _pauseBtn.gameObject.transform.DOScale(1f, _durationEnterBtn);
        }
        #endregion
    }
}
