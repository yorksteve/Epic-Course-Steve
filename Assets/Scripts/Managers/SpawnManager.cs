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

        public override void Init()
        {
            base.Init();
        }

        private void Start()
        {
            StartWave();
        }


        public void StartWave()
        {
            Debug.Log("Starting Wave");
            mechsInWave = _amountOfMechs * _waveCount;

            StartCoroutine(SpawnTime());
            
            _waveCount++;

            Debug.Log("End of StartWave()");
        }

        IEnumerator SpawnTime()
        {
            for (int i = 0; i <= mechsInWave; i++)
            {
                yield return new WaitForSeconds(5);
                PoolManager.Instance.GetMech();
            }

            Debug.Log("Spawning for current wave finished");
        }

        public void CheckWave()
        {


            if (mechsInWave == EndZone._mechsTriggered)
            {
                StartCoroutine(NextWave());
            }
        }

        IEnumerator NextWave()
        {
            Debug.Log("NextWave()");
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

