using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Managers;

namespace Scripts.Extra
{
    public class SpaceShipController : MonoBehaviour
    {
        private int _speed = 3;
        private float _destination = -5.09f;

        private void OnEnable()
        {
            EventManager.Listen("onStartingGame", FlyShip);
        }

        private void FlyShip()
        {
            while (transform.position.x < _destination)
            {
                transform.Translate(Time.deltaTime * _speed, 0, 0);

                if (transform.position.x == _destination)
                {
                    EventManager.Fire("onDreadnaught");
                }
            }
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onStartingGame", FlyShip);
        }
    }
}

