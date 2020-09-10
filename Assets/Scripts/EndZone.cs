using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Managers;
using System;


namespace Scripts
{
    public class EndZone : MonoBehaviour
    {
        public delegate void EndZoneReached(GameObject mech);
        public static event EndZoneReached onEndZoneReached;

        public static event Action onSuccess;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("EndZone :: OnTriggerEnter : Destination Reached");

            if (other.tag == "Enemy")
            {
                if (onEndZoneReached != null)
                {
                    onEndZoneReached(other.gameObject);
                }

                if (onSuccess != null)
                {
                    onSuccess();
                }
            }
           
        }
    }
}

