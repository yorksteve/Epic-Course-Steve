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

        public IEnumerator StartWaveSystem()
        {
            foreach (var mech in sequence)
            {
                PoolManager.Instance.GetMech();
                EventManager.Fire("onNewWave");
                yield return new WaitForSeconds(spawnDelay);
            }
        }
    }
}

