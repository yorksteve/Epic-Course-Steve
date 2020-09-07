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
        public int waveDuration;

        public void StartWave()
        {
            foreach (var mech in sequence)
            {
                Instantiate(mech);
            }
        }
    }
}

