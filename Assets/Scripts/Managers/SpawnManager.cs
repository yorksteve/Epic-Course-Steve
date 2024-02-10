using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Scripts.ScriptableObjects;
using UnityEngine;
using YorkSDK.Util;

namespace Scripts.Managers
{
    public class SpawnManager : MonoSingleton<SpawnManager>
    {
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Transform _destination;
        [SerializeField] private List<WaveSystem> _wave;

        private List<GameObject> _currentWave;
        private WaveSystem _waveTracker;

        private int _mechCounter;


        public override void Init()
        {
            base.Init();
        }


        private void OnEnable()
        {
            EventManager.Listen("onDreadnaught", StartWave);
            EventManager.Listen("onSuccess", MechCounter);
            EventManager.Listen("onMechKilled", MechCounter);
        }

        public void StartWave()
        {
            StartCoroutine(SpawnTime(1));
        }

        IEnumerator SpawnTime(int id)
        {
            //foreach (var wave in _wave)
            //{
            //    EventManager.Fire("onWaveCount", wave.id);
            //    yield return wave.StartWaveSystem();
            //    yield return new WaitForSeconds(wave.waveDuration);
            //}

            for (int i = 0; i < _wave.Count; i++)
            {
                EventManager.Fire("onWaveCount", id);
                _waveTracker = _wave[i];
                yield return _wave[i].StartWaveSystem();
                yield return new WaitForSeconds(_wave[i].waveDuration);
                yield return WaveDelayRoutine();
            }
        }

        IEnumerator WaveDelayRoutine()
        {
            yield return UIManager.Instance.NextWave();
        }

        public void MechCounter()
        {
            _mechCounter++;
            if (_mechCounter == _waveTracker.sequence.Count)
            {
                EventManager.Fire("onStartNextWave", true);
            }
        }

        public void RestartGame()
        {
            StartCoroutine(SpawnTime(1));
        }

        public void LoadLevel(int waveID)
        {
            StartCoroutine(SpawnTime(waveID));
        }

        public Transform RequestDestination()
        {
            return _destination;
        }

        public Transform RequestStartPoint()
        {
            return _startPoint;
        }

        public int RequestAmountOfMechs(int number)
        {
            if (_wave[number - 1] != null)
            {
                var currentWave = _wave[number - 1].sequence.Count;
                return currentWave;
            }

            return 0;
        }

        public int RequestSpawnDelay(int number)
        {
            if (_wave[number - 1] != null)
            {
                var currentWave = _wave[number - 1];
                return currentWave.spawnDelay;
            }

            return 0;
        }

        public int RequestWaveDuration(int number)
        {
            if (_wave[number - 1] != null)
            {
                var currentWave = _wave[number - 1];
                return currentWave.waveDuration;
            }

            return 0;
        }

        public void RequestSequence(int number)
        {
            if (_wave[number - 1] != null)
            {
                _currentWave = _wave[number - 1].sequence;
            }
        }

        public GameObject[] RequestMechs(int number)
        {
            if (_wave[number - 1] != null)
            {
                GameObject[] mechs = _wave[number - 1].sequence.Distinct().ToArray();
                return mechs;
            }
            else
            {
                return null;
            }
        }

        public void UpdateWaveSystem(int waveNumber, int spawnDelay, int waveDuration, int mechCount)
        {
            _wave[waveNumber - 1].UpdateWave(spawnDelay, waveDuration, mechCount);
        }

        public void NewWave(int waveNumber, int spawnDelay, int waveDuration, List<GameObject> mechs)
        {
            _wave[waveNumber - 1].NewWave(waveNumber, spawnDelay, waveDuration, mechs);
        }

        public void InsertWave(int waveNumber)
        {
            for (int i = waveNumber - 1; i < _wave.Count; i++)
            {
                _wave[waveNumber - 1].InsertWave(i);
            }
        }

        public void AddedMechs(int waveNumber, List<GameObject> mechs)
        {
            _wave[waveNumber - 1].AddedMechs(mechs);
        }

        public List<GameObject> SetListCustomization()
        {
            return _currentWave;
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onDreadnaught", StartWave);
            EventManager.UnsubscribeEvent("onSuccess", MechCounter);
            EventManager.UnsubscribeEvent("onMechKilled", MechCounter);
        }
    }


}

