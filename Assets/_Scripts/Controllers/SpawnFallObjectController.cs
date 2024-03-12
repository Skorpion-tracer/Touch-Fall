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

        private float _timeToSpawn;
        private float _minPositionX;
        private float _maxPositionX;
        private float _posX;
        private float _posY;
        private float _maxPosY;
        private float _minPosY;
        private float _incrementOffset = 1.5f;
        #endregion

        #region Constructor
        public SpawnFallObjectController(SpawnFallObjectModel spawnFallObjectModel, PoolContainer poolContainer, Vector2 screenBounds)
        {
            _model = spawnFallObjectModel;
            _pool = poolContainer;

            _minPositionX = -(screenBounds.x - 2f);
            _maxPositionX = screenBounds.x - 2f;
            _posX = _minPositionX;
            _maxPosY = screenBounds.y + _model.OffsetVerticalPositionSpawn;
            _minPosY = screenBounds.y + 0.8f;
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
            _timeToSpawn += Time.deltaTime;
            if (_timeToSpawn >= _model.TimeSpawn)
            {
                _posX = Random.Range(_minPositionX, _maxPositionX);
                _posY = Random.Range(_minPosY, _maxPosY);

                InstantiateObject();
                InstantiateModifyHeroObject();
                InstantiateEnemyObject();
                InstantiateNeedToSaveObject();
                InstantiateModifyBoundObject();

                _timeToSpawn = 0;
            }
        }
        #endregion

        #region Private Methods
        private void OnCreateGameSession()
        {
            _timeToSpawn = 0;
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
            _posX += _incrementOffset;
            float posX = _posX;
            if (posX > _maxPositionX)
            {
                posX = _minPositionX;
                _posX = _minPositionX;
            }

            _posY -= _incrementOffset;
            float posY = _posY;
            if (posY < _minPosY)
            {
                posY = _maxPosY;
                _posY = _maxPosY;
            }

            fallObject.transform.position = new Vector2(posX, posY);
            fallObject.SetActive(true);
        }
        #endregion
    }
}
