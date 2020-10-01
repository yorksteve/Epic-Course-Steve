using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Managers;

namespace Scripts.Extra
{
    public class SpaceShipController : MonoBehaviour
    {
        private float _speed = 3f;
        private float _destination = -5.09f;
        private bool _startingGame = false;
        private bool _eventFired = false;

        private void OnEnable()
        {
            EventManager.Listen("onStartingGame", FlyShip);
        }

        private void Update()
        {
            if (_eventFired == true)
                return;

            if (_startingGame == true)
            {
                if (transform.position.x < _destination)
                {
                    transform.Translate(Time.deltaTime * _speed, 0, 0);
                }

                else if (_eventFired == false)
                {
                    EventManager.Fire("onDreadnaught");
                    _eventFired = true;
                }
            }    
        }

        private void FlyShip()
        {
            _startingGame = true;
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onStartingGame", FlyShip);
        }
    }
}

