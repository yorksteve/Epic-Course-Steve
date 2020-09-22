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
        private bool _isCalled;

        public void Update()
        {
            if (transform.position.x < _destination)
            {
                transform.Translate(Time.deltaTime * _speed, 0, 0);
            }

            else
            {
                if (_isCalled == false)
                {
                    CompletedMovement();
                }
            }
            
        }

        void CompletedMovement()
        {
            EventManager.Fire("onDreadnaught");

            _isCalled = true;
        }
    }
}

