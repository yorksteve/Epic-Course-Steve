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
            StartCoroutine(SpawnTime());
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
        }

        public void RestartGame()
        {
            _waveCount = 1;
            StartWave();
        }

        public void LoadLevel(int wave)
        {
            _waveCount = wave;
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

        public List<GameObject> RequestSequence(int number)
        {
            var currentWave = _wave[number];
            return currentWave.sequence;
        }

        public int RequestWaveID(int number)
        {
            var currentWave = _wave[number];
            return currentWave.id;
        }

        public int RequestSpawnDelay(int number)
        {
            var currentWave = _wave[number];
            return currentWave.spawnDelay;
        }

        public int RequestWaveDuration(int number)
        {
            var currentWave = _wave[number];
            return currentWave.waveDuration;
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onDreadnaught", StartWave);
            EventManager.UnsubscribeEvent("onNextWave", StartWave);
        }
    }


}

