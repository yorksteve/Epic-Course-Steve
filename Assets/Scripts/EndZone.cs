using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Managers;
using System;


namespace Scripts
{
    public class EndZone : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Mech1") || other.CompareTag("Mech2"))
            {
                EventManager.Fire("onEndZoneReached", other.gameObject);
                EventManager.Fire("onSuccess");
            }
           
        }
    }
}

