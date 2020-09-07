using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Managers;


namespace Scripts
{
    public class EndZone : MonoBehaviour
    {
        public static int _mechsTriggered;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Destination Reached");
            _mechsTriggered++;
            other.gameObject.SetActive(false);
            // Reduce player's life count

            SpawnManager.Instance.CheckWave();
        }
    }
}

