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
        private float _probabilityEmpty = 1f;
        private float _probabilityHero = 2f;
        private float _probabilityBound = 0.1f;
        private float _probabilityEnemy = 1f;
        private float _probabilitySave = 1f;
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
                InstantiateObject();
                InstantiateModifyHeroObject();
                InstantiateModifyBoundObject();
                InstantiateEnemyObject();
                InstantiateNeedToSaveObject();
                _time = 0;
            }
        }
        #endregion

        #region Private Methods
        private void InstantiateObject()
        {
            float result = Random.Range(0, 5);
            if (result >= _probabilityEmpty)
            {
                FallObjectView fallObjectView = _pool.PoolEmptyObjects.GetPooledObject();

                if (fallObjectView != null)
                {
                    InitFallObject(fallObjectView.gameObject);
                }
            }  
        }

        private void InstantiateModifyHeroObject()
        {
            float result = Random.Range(0, 5);
            if (result >= _probabilityHero)
            {
                FallObjectModifyHeroView fallObjectModifyView = _pool.PoolModifyObjects.GetPooledObject();

                if (fallObjectModifyView != null)
                {
                    InitFallObject(fallObjectModifyView.gameObject);
                } 
            }
        }

        private void InstantiateModifyBoundObject()
        {
            float result = Random.Range(0, 10);
            if (result >= _probabilityBound)
            {
                FallObjectModifyBoundView fallObjectModifyBoundView = _pool.PoolModifyBoundObjects.GetPooledObject();

                if (fallObjectModifyBoundView != null)
                {
                    InitFallObject(fallObjectModifyBoundView.gameObject);
                }
            }
        }

        private void InstantiateEnemyObject()
        {
            float result = Random.Range(0, 5);
            if (result >= _probabilityEnemy)
            {
                FallObjectEnemyView fallObjectEnemyView = _pool.PoolEnemyObjects.GetPooledObject();

                if (fallObjectEnemyView != null)
                {
                    InitFallObject(fallObjectEnemyView.gameObject);
                }
            }
        }

        private void InstantiateNeedToSaveObject()
        {
            float result = Random.Range(0, 5);
            if (result >= _probabilitySave)
            {
                FallObjectNeedToSaveView fallObjectNeedToSaveView = _pool.PoolNeedToSaveObjects.GetPooledObject();

                if (fallObjectNeedToSaveView != null)
                {
                    InitFallObject(fallObjectNeedToSaveView.gameObject);
                }
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
