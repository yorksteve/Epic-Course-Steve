using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Managers;
using System;


namespace Scripts
{
    public class EndZone : MonoBehaviour
    {
        public static event Action onEndZoneReached;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("EndZone :: OnTriggerEnter : Destination Reached");

            if (other.tag == "Enemy")
            {
                if (onEndZoneReached != null)
                {
                    onEndZoneReached();
                }
            }
           
        }
    }
}

