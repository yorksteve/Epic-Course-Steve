using Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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
        [SerializeField] private GameObject[] _mechs;

        private void Awake()
        {
            _resetDelay = new WaitForSeconds(.5f);
            //var mech1 = (GameObject)AssetDatabase.LoadMainAssetAtPath("Assets/GameDevHQ/FileBase/Projects/Tutorials/Starter_Files/Epic_Tower_Defense/3D/Characters/Robots/Mech1/Prefab/Mech1.prefab");
            //var mech2 = (GameObject)AssetDatabase.LoadMainAssetAtPath("Assets/GameDevHQ/FileBase/3D/Characters/Robots/Mech_02/Prefab/Mech2.prefab");
            //_mechs = new GameObject[] { mech1, mech2 };
        }

        public IEnumerator StartWaveSystem()
        {
            foreach (var mech in sequence)
            {
                if (mech == _mechs[0])
                    PoolManager.Instance.GetMech(0);
                else
                    PoolManager.Instance.GetMech(1);
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

