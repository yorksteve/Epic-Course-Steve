using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Extra
{
    public class SpaceShipController : MonoBehaviour
    {
        private int _speed = 3;
        private float _destination = -5.09f;
        private bool _isCalled;

        public static event Action onDreadnaught;

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
            if (onDreadnaught != null)
            { 
                onDreadnaught();
            }

            _isCalled = true;
        }
    }
}

