using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Managers;
using Scripts.Interfaces;

namespace Scripts
{
    public class AvailableSpots : MonoBehaviour
    {
        [SerializeField] private bool _isActive;
        [SerializeField] private Material _materialRadius;

        private ParticleSystem _system;
        private GameObject _test;
        private ITower _towerPlaced;
        private bool _placingTower;

        private void OnEnable()
        {
            TowerManager.onPlacingTower += SpotAvailable;
        }

        private void Start()
        {
            _test = this.gameObject;

            _system = _test.GetComponent<ParticleSystem>();
        }

        void SpotAvailable(bool placingTower)
        {
            if (_isActive == false && placingTower == true)
            {
                _system.Play();
                _placingTower = true;
            }

            else
            {
                _system.Stop();
                _placingTower = false;
            }
        }

        private void OnMouseEnter()
        {
            if (_isActive == false)
            {
                _materialRadius.color = Color.green;
            }

            TowerManager.Instance.SnapToPosition(transform.position);
        }

        private void OnMouseDown()
        {
            if (_isActive == false && _placingTower == true)
            {
                _towerPlaced = TowerManager.Instance.PlaceTower(transform.position);
                _isActive = true;
            }
        }

        private void OnMouseExit()
        {
            _materialRadius.color = Color.red;
            TowerManager.Instance.ReleaseSnap();
        }


        private void OnDisable()
        {
            TowerManager.onPlacingTower -= SpotAvailable;
        }
    }
}

