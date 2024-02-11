using TouchFall.Controller.Interfaces;
using TouchFall.Helper.Enums;
using TouchFall.Model;
using TouchFall.Singletons;
using TouchFall.View;
using UnityEngine;

namespace TouchFall.Controller
{
    public sealed class MainHeroBehavoiurController : IFixedUpdater
    {
        #region Fields
        private readonly MainHeroView _heroView;
        private readonly MainHeroModel _heroModel;

        private StateBehaviourHero _stateBehaviourHero = StateBehaviourHero.None;      
        #endregion

        #region Constructor
        public MainHeroBehavoiurController(MainHeroView mainHeroView, MainHeroModel mainHeroModel)
        {
            _heroView = mainHeroView;
            _heroModel = mainHeroModel;

            ModifyPlayer.Instance.Modify += OnModify;
        }

        ~MainHeroBehavoiurController()
        {
            ModifyPlayer.Instance.Modify -= OnModify;
        }
        #endregion

        #region Public Methods
        public void FixedUpdate()
        {
            switch (_stateBehaviourHero)
            {
                case StateBehaviourHero.None:
                    return;
                case StateBehaviourHero.Rotate:
                    _heroView.Body.MoveRotation(_heroView.Body.rotation + _heroModel.SpeedRotation * Time.fixedDeltaTime);
                    break;
            }
        }
        #endregion

        #region Private Methods
        private void OnModify(ModifyHero modifyHero)
        {
            if (modifyHero == ModifyHero.Spinning) _stateBehaviourHero = StateBehaviourHero.Rotate;
            else _stateBehaviourHero = StateBehaviourHero.None;
        }
        #endregion
    }
}
