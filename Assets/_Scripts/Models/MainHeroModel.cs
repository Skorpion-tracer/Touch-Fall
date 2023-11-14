using System;
using TouchFall.Helper.Enums;
using UnityEngine;

namespace TouchFall
{
    [Serializable]
    public sealed class MainHeroModel
    {
        [SerializeField] private float _speed = 3f;

        public InputState InputState { get; set; }
        public float Speed => _speed;
    }
}
