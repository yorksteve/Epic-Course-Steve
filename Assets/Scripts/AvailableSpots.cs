using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Managers;


namespace Scripts
{
    public class AvailableSpots : MonoBehaviour
    {
        private bool _isActive;

        private GameObject _towerType;
        private ParticleSystem _system;

        public delegate void FoundAvailableSpot(Vector3 pos);
        public static event FoundAvailableSpot onFoundAvailableSpot;

        private void OnEnable()
        {
            TowerManager.onTowerPlaced += SpotTaken;
        }

        private void OnDisable()
        {
            TowerManager.onTowerPlaced -= SpotTaken;
        }

        private void Start()
        {
            if (_system != null)
            {
                _system = GetComponent<ParticleSystem>();
            }
        }

        void SpotAvailable()
        {
            if (_isActive == false)
            {
                _system.Play();
                if (onFoundAvailableSpot != null)
                {
                    onFoundAvailableSpot(transform.position);
                }
            }
        }

        void SpotTaken()
        {
            _isActive = true;
            //_towerType = the tower placed in this collider
        }
    }
}

