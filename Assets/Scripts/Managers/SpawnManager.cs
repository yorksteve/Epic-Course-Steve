﻿using Scripts.Extra;
using System;
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
        private int _successfulMechs = 0;
        private int _destroyedMechs = 0;

        private WaitForSeconds _spawnTimeYield;
        private WaitForSeconds _resetMechYield;

        public override void Init()
        {
            base.Init();
        }


        private void OnEnable()
        {
            EventManager.Listen("onSuccess", SuccessfulMechs);
            EventManager.Listen("onMechDestroyedSpawn", DestroyedMechs);
            EventManager.Listen("onDreadnaught", StartWave);
            EventManager.Listen("onNextWave", StartWave);
        }

        private void Start()
        {
            _spawnTimeYield = new WaitForSeconds(5);
            _resetMechYield = new WaitForSeconds(.5f);
        }

        public void StartWave()
        {
            EventManager.Fire("onWaveCount", _waveCount);
            if (_waveCount <= 10)
            {
                mechsInWave = _amountOfMechs * _waveCount;

                StartCoroutine(SpawnTime());

                _waveCount++;
            }
        }

        IEnumerator SpawnTime()
        {
            for (int i = 0; i <= mechsInWave; i++)
            {
                yield return _spawnTimeYield;
                PoolManager.Instance.GetMech();
                yield return _resetMechYield;
                EventManager.Fire("onNewWave");
            }
        }

        public void CheckWave()
        {
            if (mechsInWave == (_successfulMechs + _destroyedMechs))
            {
                StartCoroutine(UIManager.Instance.NextWave());
            }
        }

        public void SuccessfulMechs()
        {
            _successfulMechs++;
            CheckWave();
        }

        public void DestroyedMechs()
        {
            _destroyedMechs++;
            CheckWave();
        }

        public void RestartGame()
        {
            _waveCount = 1;
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

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onSuccess", SuccessfulMechs);
            EventManager.UnsubscribeEvent("onMechDestroyed", DestroyedMechs);
            EventManager.UnsubscribeEvent("onDreadnaught", StartWave);
        }
    }


}

