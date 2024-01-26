using TouchFall.Controller.Interfaces;
using TouchFall.Helper.PoolObject;
using TouchFall.Model;
using TouchFall.Singletons;
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
        #endregion

        #region Constructor
        public SpawnFallObjectController(SpawnFallObjectModel spawnFallObjectModel, PoolContainer poolContainer, Vector2 screenBounds)
        {
            _model = spawnFallObjectModel;
            _pool = poolContainer;

            _minPositionX = -screenBounds.x;
            _maxPositionX = screenBounds.x;
            _posY = screenBounds.y + _model.OffsetVerticalPositionSpawn;
            GameLevel.Instance.CreateGameSession += OnCreateGameSession;
        }

        ~SpawnFallObjectController()
        {
            GameLevel.Instance.CreateGameSession -= OnCreateGameSession;
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
        private void OnCreateGameSession()
        {
            _time = 0;
        }

        private void InstantiateObject()
        {
            int result = Random.Range(1, _model.MaxRangeDropEmpty);
            if (result >= _model.ProbabilityEmpty)
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
            int result = Random.Range(1, _model.MaxRangeDropHero);
            if (result >= _model.ProbabilityHero)
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
            int result = Random.Range(1, _model.MaxRangeDropBound);
            if (result >= _model.ProbabilityBound)
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
            int result = Random.Range(1, _model.MaxRangeDropEnemy);
            if (result >= _model.ProbabilityEnemy)
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
            int result = Random.Range(1, _model.MaxRangeDropSave);
            if (result >= _model.ProbabilitySave)
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
