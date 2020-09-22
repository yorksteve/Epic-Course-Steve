﻿using System.Collections;
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
            Debug.Log("EndZone :: OnTriggerEnter : Destination Reached");

            if (other.tag == "Enemy")
            {
                EventManager.Fire("onEndZoneReached", other.gameObject);
                EventManager.Fire("onSuccess");
            }
           
        }
    }
}

