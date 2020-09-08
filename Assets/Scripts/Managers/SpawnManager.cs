using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;

namespace Scripts.Managers
{
    public class SpawnManager : MonoSingleton<SpawnManager>
    {
        private int _waveCount = 1;
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _destination;

        public int mechsInWave;
        private int _amountOfMechs = 10;
        private int _successfulMechs;
        private int _destroyedMechs;

        public override void Init()
        {
            base.Init();
        }

        private void Start()
        {
            StartWave();
        }

        private void OnEnable()
        {
            EndZone.onEndZoneReached += SuccessfulMechs;
            EnemyAI.onMechDestroyed += DestroyedMechs;
        }

        private void OnDisable()
        {
            EndZone.onEndZoneReached -= SuccessfulMechs;
            EnemyAI.onMechDestroyed -= DestroyedMechs;
        }

        public void StartWave()
        {
            Debug.Log("SpawnManager :: StartWave() : Starting Wave");
            mechsInWave = _amountOfMechs * _waveCount;

            StartCoroutine(SpawnTime());
            
            _waveCount++;

            Debug.Log("SpawnManager :: StartWave() : End of StartWave()");
        }

        IEnumerator SpawnTime()
        {
            for (int i = 0; i <= mechsInWave; i++)
            {
                yield return new WaitForSeconds(5);
                PoolManager.Instance.GetMech();
            }

            Debug.Log("SpawnManager :: SpawnTime() : Spawning for current wave finished");
        }

        public void CheckWave()
        {
            if (mechsInWave == (_successfulMechs + _destroyedMechs))
            {
                StartCoroutine(NextWave());
            }

            Debug.Log("SpawnManager::CheckWave(): Completed Wave Check");
        }

        public void SuccessfulMechs()
        {
            _successfulMechs++;
            Debug.Log("SpawnManager::SuccessfulMechs()");

            CheckWave();
        }

        public void DestroyedMechs()
        {
            _destroyedMechs++;
            Debug.Log("SpawnManager::DestroyedMechs()");

            CheckWave();
        }

        IEnumerator NextWave()
        {
            Debug.Log("SpawnManager :: NextWave()");
            yield return new WaitForSeconds(10);
            StartWave();
        }

        public Transform RequestDestination()
        {
            return _destination;
        }

        public Transform RequestStartPoint()
        {
            return _startPoint;
        }

        public int RequestWave()
        {
            return _waveCount;
        }
    }


}

