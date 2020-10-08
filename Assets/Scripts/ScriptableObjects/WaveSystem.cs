using Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Wave.asset", menuName = "Window/Scriptables/New Wave")]
    public class WaveSystem : ScriptableObject
    {
        public int id;
        public List<GameObject> sequence;
        public int spawnDelay;
        public int waveDuration = 10;

        private WaitForSeconds _resetDelay;

        private void Awake()
        {
            _resetDelay = new WaitForSeconds(.5f);
        }

        public IEnumerator StartWaveSystem()
        {
            foreach (var mech in sequence)
            {
                PoolManager.Instance.GetMech(mech);
                yield return _resetDelay;
                EventManager.Fire("onNewWave");
                yield return new WaitForSeconds(spawnDelay);
            }
        }

        public void UpdateWave(int spawnDelayChange, int waveDurationChange, int mechCount)
        {
            spawnDelay = spawnDelayChange;
            waveDuration = waveDurationChange;
            sequence.Capacity = mechCount;
        }

        public void NewWave(int waveID, int spawnDelayChange, int waveDurationChange, List<GameObject> mechs)
        {
            // Create a new wave?
            id = waveID;
            spawnDelay = spawnDelayChange;
            waveDuration = waveDurationChange;
            sequence = mechs;
        }

        public void InsertWave(int waveID)
        {
            id = waveID + 1;
        }

        public void AddedMechs(List<GameObject> mechs)
        {
            sequence = mechs;
        }
    }
}

