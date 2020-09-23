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
            EventManager.Listen("onPlacingTower", (Action<bool>)SpotAvailable);
            EventManager.Listen("onTowerDestroyed", (Action<Vector3>)TowerDestroyed);
            EventManager.Listen("onUpdateTower", (Action<GameObject, Vector3>)UpdateTower);
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
                _placingTower = false;
                _system.Stop();
            }
        }

        private void TowerDestroyed(Vector3 pos)
        {
            _isActive = false;
        }

        private void OnMouseEnter()
        {
            if (_isActive == false)
            {
                _materialRadius.color = Color.green;
                TowerManager.Instance.SnapToPosition(transform.position);
            }

        }

        private void OnMouseDown()
        {
            Debug.Log("AvailableSpots::OnMouseDown()");
            if (_isActive == false && _placingTower == true)
            {
                _towerPlaced = TowerManager.Instance.PlaceTower(transform.position);
                _isActive = true;
            }

            if (_isActive == true && _placingTower == false)
            {
                Debug.Log("AvailableSpots::OnMouseDown() : Inside if()");
                EventManager.Fire("onUpgradeTower", _towerPlaced, this.transform.position);
            }
        }

        private void OnMouseExit()
        {
            _materialRadius.color = Color.red;
            TowerManager.Instance.ReleaseSnap();
        }

        private void UpdateTower(GameObject tower, Vector3 pos)
        {
            if (pos == this.transform.position)
            {
                _towerPlaced.CurrentModel = tower;
            }
        }


        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onPlacingTower", (Action<bool>)SpotAvailable);
            EventManager.UnsubscribeEvent("onTowerDestroyed", (Action<Vector3>)TowerDestroyed);
            EventManager.UnsubscribeEvent("onUpdateTower", (Action<GameObject, Vector3>)UpdateTower);
        }
    }
}

