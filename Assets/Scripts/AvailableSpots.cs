using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Managers;


namespace Scripts
{
    public class AvailableSpots : MonoBehaviour
    {
        [SerializeField] private bool _isActive;
        [SerializeField] private Material _materialRadius;

        private ParticleSystem _system;
        private GameObject _test;
        private bool _fundsAvailable;

        private void OnEnable()
        {
            TowerManager.onPlacingTower += SpotAvailable;
            TowerManager.onCancelTower += CancelAvailablity;
            WarFundManager.onLackingFunds += FundsAvailableReceiver;
        }

        private void OnDisable()
        {
            TowerManager.onPlacingTower -= SpotAvailable;
            TowerManager.onCancelTower -= CancelAvailablity;
            WarFundManager.onLackingFunds -= FundsAvailableReceiver;
        }

        private void Start()
        {
            _test = this.gameObject;

            _system = _test.GetComponent<ParticleSystem>();
        }

        void SpotAvailable()
        {
            if (_isActive == false)
            {
                _system.Play();
            }
        }

        void CancelAvailablity()
        {
            _system.Stop();
        }

        void FundsAvailableReceiver()
        {
            _fundsAvailable = false;
        }

        private void OnMouseEnter()
        {
            if (_isActive == false)
            {
                _materialRadius.color = Color.green;
            }
        }

        private void OnMouseDown()
        {
            if (_isActive == false && _fundsAvailable == true)
            {
                TowerManager.Instance.PlaceTower(transform.position);
                _isActive = true;
            }
        }

        private void OnMouseExit()
        {
            _materialRadius.color = Color.red;
        }
    }
}

