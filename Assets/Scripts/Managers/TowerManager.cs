using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;

namespace Scripts.Managers
{
    public class TowerManager : MonoSingleton<TowerManager>
    {
        [SerializeField] private GameObject[] _decoyTower;
        [SerializeField] private GameObject[] _tower;
        [SerializeField] private Material _materialRadius;

        private GameObject _prefab;

        private bool _canPlaceTower;
        private int _towerID;


        public override void Init()
        {
            base.Init();
        }

        public static event Action onTowerPlaced;
        public static event Action onPlacingTower;

        private void OnEnable()
        {
            AvailableSpots.onFoundAvailableSpot += ValidSpot;
        }

        private void OnDisable()
        {
            AvailableSpots.onFoundAvailableSpot -= ValidSpot;
        }

        public void Update()
        {
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(rayOrigin, out hitInfo))
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    _prefab = Instantiate(_decoyTower[0], hitInfo.point, Quaternion.identity);
                    _towerID = 0;

                    if (onPlacingTower != null)
                    {
                        onPlacingTower();
                    }
                }

                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    _prefab = Instantiate(_decoyTower[1], hitInfo.point, Quaternion.identity);
                    _towerID = 1;

                    if (onPlacingTower != null)
                    {
                        onPlacingTower();
                    }
                }

                _prefab.transform.position = hitInfo.point;

                if (_prefab.transform.position == hitInfo.point)
                {
                    _materialRadius.color = Color.green;

                    if (Input.GetMouseButtonDown(0) && _canPlaceTower == true)
                    {
                        PlaceTower(hitInfo.point);
                    }
                }

                else
                {
                    _materialRadius.color = Color.red;
                }
            }
        }


        public void PlaceTower(Vector3 pos)
        {
         
            Instantiate(_tower[_towerID], pos, Quaternion.identity);
            _canPlaceTower = false;
            if (onTowerPlaced != null)
            {
                onTowerPlaced();
            }
        }

        public void PlaceDecoyTower(int i)
        {
            _towerID = i;
            Instantiate(_decoyTower[i], Input.mousePosition, Quaternion.identity);     

            if (onPlacingTower != null)
            {
                onPlacingTower();
            }
        }

        public void ValidSpot(Vector3 pos)
        {
            if (pos == Input.mousePosition)
            {
                _canPlaceTower = true;
            }

            else
            {
                _canPlaceTower = false;
            }
        }
    }
}

