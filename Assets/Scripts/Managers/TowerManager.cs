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

        public delegate void TowerPlaced(Vector3 pos);
        public static event TowerPlaced onTowerPlaced;

        public static event Action onPlacingTower;
        public static event Action onCancelTower;

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
                _prefab.transform.position = hitInfo.point;

                if (_prefab.transform.position == hitInfo.point)
                {
                    if (_canPlaceTower == true)
                    {
                        _materialRadius.color = Color.green;

                        if (Input.GetMouseButtonDown(0))
                        {
                            PlaceTower(hitInfo.point);
                        }
                    }

                    else
                    {
                        _materialRadius.color = Color.red;
                    }
                }

                if (Input.GetMouseButtonDown(1))
                {
                    Destroy(_prefab);
                    if (onCancelTower != null)
                    {
                        onCancelTower();
                    }
                }
            }
        }

        public void PlaceTower(Vector3 pos)
        {
         
            Instantiate(_tower[_towerID], pos, Quaternion.identity);
            _canPlaceTower = false;
            if (onTowerPlaced != null)
            {
                onTowerPlaced(pos);
            }
        }

        public void PlaceDecoyTower(int i)
        {
            _towerID = i;
            _prefab = Instantiate(_decoyTower[i], Input.mousePosition, Quaternion.identity);     

            if (onPlacingTower != null)
            {
                onPlacingTower();
            }
        }

        public void ValidSpot(Vector3 pos)
        {
            if (pos == _prefab.transform.position)  // This doesn't work
            {
                _canPlaceTower = true;
            }
        }
    }
}

