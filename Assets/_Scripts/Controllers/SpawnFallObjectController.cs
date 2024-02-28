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

        private enum SpawnType : byte
        {
            None,
            Empty,
            ModifyHero,
            Enemy,
            NeedToSave,
            ModifyBound
        }

        private SpawnFallObjectModel _model;
        private PoolContainer _pool;

        private SpawnType _spawnType;
        private float _timeToSpawn;
        private float _timeSpawnObject;
        private float _minPositionX;
        private float _maxPositionX;
        private float _posX;
        private float _posY;
        private float _maxPosY;
        private float _minPosY;
        private float _incrementOffset = 1.5f;
        private float _timeSpawn = 0.5f;
        #endregion

        #region Constructor
        public SpawnFallObjectController(SpawnFallObjectModel spawnFallObjectModel, PoolContainer poolContainer, Vector2 screenBounds)
        {
            _model = spawnFallObjectModel;
            _pool = poolContainer;

            _minPositionX = -(screenBounds.x - 0.9f);
            _maxPositionX = screenBounds.x - 0.9f;
            _posX = _minPositionX;
            _maxPosY = screenBounds.y + _model.OffsetVerticalPositionSpawn;
            _minPosY = screenBounds.y + 0.4f;
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
            //switch (_spawnType)
            //{
            //    case SpawnType.None:
            //        _timeToSpawn += Time.deltaTime;
            //        if (_timeToSpawn >= _model.TimeSpawn)
            //        {
            //            _posX = Random.Range(_minPositionX, _maxPositionX);
            //            _posY = Random.Range(_minPosY, _maxPosY);
            //            _spawnType = SpawnType.Empty;
            //            return;
            //        }                        
            //        break;
            //    case SpawnType.Empty:
            //        InstantiateObject();
            //        _spawnType = SpawnType.ModifyHero;
            //        return;
            //    case SpawnType.ModifyHero:
            //        _timeSpawnObject += Time.deltaTime;
            //        if (_timeSpawnObject >= _timeSpawn)
            //        {
            //            InstantiateModifyHeroObject();
            //            _spawnType = SpawnType.Enemy;
            //            _timeSpawnObject = 0;
            //            return;
            //        }
            //        break;
            //    case SpawnType.Enemy:
            //        _timeSpawnObject += Time.deltaTime;
            //        if (_timeSpawnObject >= _timeSpawn)
            //        {
            //            InstantiateEnemyObject();
            //            _spawnType = SpawnType.NeedToSave;
            //            _timeSpawnObject = 0;
            //            return;
            //        }
            //        break;
            //    case SpawnType.NeedToSave:
            //        _timeSpawnObject += Time.deltaTime;
            //        if (_timeSpawnObject >= _timeSpawn)
            //        {
            //            InstantiateNeedToSaveObject();
            //            _spawnType = SpawnType.ModifyBound;
            //            _timeSpawnObject = 0;
            //            return;
            //        }
            //        break;
            //    case SpawnType.ModifyBound:
            //        _timeSpawnObject += Time.deltaTime;
            //        if (_timeSpawnObject >= _timeSpawn)
            //        {
            //            InstantiateModifyBoundObject();
            //            _spawnType = SpawnType.None;
            //            _timeSpawnObject = 0;
            //            return;
            //        }
            //        break;
            //}
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
            _timeSpawnObject = 0;
            _spawnType = SpawnType.None;
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
            //fallObject.transform.position = new Vector2(Random.Range(_minPositionX, _maxPositionX), Random.Range(_minPosY, _maxPosY));
            fallObject.SetActive(true);
        }
        #endregion
    }
}
