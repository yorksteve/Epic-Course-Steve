using Scripts.Extra;
using Scripts.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;

namespace Scripts.Managers
{
    public class SpawnManager : MonoSingleton<SpawnManager>
    {
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _destination;
        [SerializeField] private List<WaveSystem> _wave;

        private int _currentWave;
        private int _waveCount = 1;

        //public int mechsInWave;
        //private int _successfulMechs = 0;
        //private int _destroyedMechs = 0;

        //private WaitForSeconds _spawnTimeYield;
        //private WaitForSeconds _resetMechYield;

        public override void Init()
        {
            base.Init();
        }


        private void OnEnable()
        {
            //EventManager.Listen("onSuccess", SuccessfulMechs);
            //EventManager.Listen("onMechDestroyedSpawn", DestroyedMechs);
            EventManager.Listen("onDreadnaught", StartWave);
            EventManager.Listen("onNextWave", StartWave);
        }

        private void Start()
        {
            //_spawnTimeYield = new WaitForSeconds(_wave[_currentWave].spawnDelay);
            //_resetMechYield = new WaitForSeconds(.5f);
        }

        public void StartWave()
        {
            //var currentWave = _wave[_currentWave];

            //mechsInWave = currentWave.sequence.Count;
            //_waveCount = currentWave.id;


            StartCoroutine(SpawnTime());

            //if (_waveCount <= 10)
            //{
            //    mechsInWave =

            //    StartCoroutine(SpawnTime());

            //    _waveCount++;
            //}
        }

        IEnumerator SpawnTime()
        {
            foreach (var wave in _wave)
            {
                EventManager.Fire("onWaveCount", _waveCount);
                wave.StartWaveSystem();
                yield return new WaitForSeconds(wave.waveDuration);
                _waveCount++;
                //yield return _resetMechYield;
            }

            //for (int i = 0; i <= mechsInWave; i++)
            //{
            //yield return _spawnTimeYield;
            //PoolManager.Instance.GetMech();
            
            //}
        }

        //public void CheckWave()
        //{
        //    if (mechsInWave == (_successfulMechs + _destroyedMechs))
        //    {
        //        StartCoroutine(UIManager.Instance.NextWave());
        //    }
        //}

        //public void SuccessfulMechs()
        //{
        //    _successfulMechs++;
        //    CheckWave();
        //}

        //public void DestroyedMechs()
        //{
        //    _destroyedMechs++;
        //    CheckWave();
        //}

        //public void RestartGame()
        //{
        //    _waveCount = 1;
        //    StartWave();
        //}

        //public void LoadLevel(int wave)
        //{
        //    _waveCount = wave;
        //    StartWave();
        //}

        public Transform RequestDestination()
        {
            return _destination;
        }

        public Transform RequestStartPoint()
        {
            return _startPoint;
        }

        //public int RequestWave()
        //{
        //    return _waveCount;
        //}

        private void OnDisable()
        {
            //EventManager.UnsubscribeEvent("onSuccess", SuccessfulMechs);
            //EventManager.UnsubscribeEvent("onMechDestroyed", DestroyedMechs);
            EventManager.UnsubscribeEvent("onDreadnaught", StartWave);
            EventManager.UnsubscribeEvent("onNextWave", StartWave);
        }
    }


}

