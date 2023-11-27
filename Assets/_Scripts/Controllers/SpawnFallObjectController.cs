using TouchFall.Controller.Interfaces;
using TouchFall.Helper;
using TouchFall.Model;
using TouchFall.View;
using UnityEngine;

namespace TouchFall.Controller
{
    public sealed class SpawnFallObjectController : IUpdater
    {
        #region Fields
        private SpawnFallObjectModel _model;
        private ObjectPool _pool;

        private float _time;
        private float _minPositionX;
        private float _maxPositionX;
        private float _posY;
        #endregion

        #region Constructor
        public SpawnFallObjectController(SpawnFallObjectModel spawnFallObjectModel, ObjectPool objectPool, Vector2 screenBounds)
        {
            _model = spawnFallObjectModel;
            _pool = objectPool;

            _minPositionX = -screenBounds.x;
            _maxPositionX = screenBounds.x;
            _posY = screenBounds.y + _model.OffsetVerticalPositionSpawn;
        }
        #endregion

        #region Public Methods
        public void Update()
        {
            _time += Time.deltaTime;
            if (_time >= _model.TimeSpawn)
            {
                InstantiateObject();
                _time = 0;
            }
        }
        #endregion

        #region Private Methods
        private void InstantiateObject()
        {
            FallObjectView fallObjectView = _pool.GetPooledObject();

            if (fallObjectView != null)
            {
                fallObjectView.gameObject.transform.position = new Vector2(Random.Range(_minPositionX, _maxPositionX), _posY);
                fallObjectView.gameObject.SetActive(true);
            }
        }
        #endregion
    }
}
