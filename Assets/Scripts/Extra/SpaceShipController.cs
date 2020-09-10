using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Extra
{
    public class SpaceShipController : MonoBehaviour
    {
        [SerializeField] private Transform _destination;
        private int _speed = 3;

        public static event Action onDreadnaught;

        public void Update()
        {
            if (transform.position.x < -5.09f)
            {
                transform.Translate(Time.deltaTime * _speed, 0, 0);
            }

            if (transform.position.x == -5.09 && onDreadnaught != null)
            {
                onDreadnaught();
            }
        }
    }
}

