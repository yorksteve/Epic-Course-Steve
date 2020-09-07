using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Managers;
using System;


namespace Scripts
{
    public class EndZone : MonoBehaviour
    {
        public static int _mechsTriggered;

        public static event Action onEndZoneReached;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("EndZone :: OnTriggerEnter : Destination Reached");
            _mechsTriggered++;
            other.gameObject.SetActive(false);
            // Reduce player's life count

            SpawnManager.Instance.CheckWave();

            if (onEndZoneReached != null)
            {
                onEndZoneReached();
            }
        }
    }
}

