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
        [SerializeField] private float _downTime = 1.5f;
        #endregion

        #region Properties
        public StateMainHero StateMainHero { get; set; }
        public float Speed => _speed;
        public float DownTime => _downTime;
        #endregion
    }
}
