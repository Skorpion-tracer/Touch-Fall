using System;
using TouchFall.Helper.Enums;
using UnityEngine;

namespace TouchFall.Model
{
    [Serializable]
    public sealed class MainHeroModel
    {
        #region Поля
        [SerializeField] private float _speed = 3f;
        [SerializeField] private float _speedRotation = 7f;
        [SerializeField] private float _downTime = 1.5f;
        [SerializeField] private float _clampMove = 2f;
        #endregion

        #region Properties
        public StateMoveMainHero StateMainHero { get; set; }
        public float Speed => _speed;
        public float DownTime => _downTime;
        public float SpeedRotation => _speedRotation;
        public float ClampMove => _clampMove;
        #endregion
    }
}
