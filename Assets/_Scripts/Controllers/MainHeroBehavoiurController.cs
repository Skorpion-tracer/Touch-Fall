using TouchFall.Controller.Interfaces;
using TouchFall.Helper.Enums;
using TouchFall.Model;
using TouchFall.Singletons;
using TouchFall.View;

namespace TouchFall.Controller
{
    public sealed class MainHeroBehavoiurController : IFixedUpdater
    {
        #region Fields
        private MainHeroView _heroView;
        private MainHeroModel _heroModel;

        private StateBehaviourHero _stateBehaviourHero = StateBehaviourHero.None;

        private float _angle;
        private float _maxAngle = 365f;
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
                    _angle += _heroModel.SpeedRotation;
                    if (_angle >= _maxAngle) _angle = 0;
                    _heroView.Body.MoveRotation(_angle);
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
