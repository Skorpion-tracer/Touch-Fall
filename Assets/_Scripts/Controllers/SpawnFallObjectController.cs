using TouchFall.Controller.Interfaces;
using TouchFall.Helper.PoolObject;
using TouchFall.Model;
using TouchFall.View;
using UnityEngine;

namespace TouchFall.Controller
{
    public sealed class SpawnFallObjectController : IUpdater
    {
        #region Fields
        private SpawnFallObjectModel _model;
        private PoolContainer _pool;

        private float _time;
        private float _minPositionX;
        private float _maxPositionX;
        private float _posY;
        private float _probability = 3;
        #endregion

        #region Constructor
        public SpawnFallObjectController(SpawnFallObjectModel spawnFallObjectModel, PoolContainer poolContainer, Vector2 screenBounds)
        {
            _model = spawnFallObjectModel;
            _pool = poolContainer;

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
                float result = Random.Range(0, 5);
                if (result >= _probability)
                    InstantiateObject();
                else
                    InstantiateModifyObject();
                _time = 0;
            }
        }
        #endregion

        #region Private Methods
        private void InstantiateObject()
        {
            FallObjectView fallObjectView = _pool.PoolEmptyObjects.GetPooledObject();

            if (fallObjectView != null)
            {
                InitFallObject(fallObjectView.gameObject);
            }
        }

        private void InstantiateModifyObject()
        {
            FallObjectModifyView fallObjectModifyView = _pool.PoolModifyObjects.GetPooledObject();

            if (fallObjectModifyView != null)
            {
                InitFallObject(fallObjectModifyView.gameObject);
            }
        }

        private void InitFallObject(GameObject fallObject)
        {
            fallObject.transform.position = new Vector2(Random.Range(_minPositionX, _maxPositionX), _posY);
            fallObject.SetActive(true);
        }
        #endregion
    }
}
