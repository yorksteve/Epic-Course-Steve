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

        private bool _canPlaceTower;
        private int _towerID;


        public override void Init()
        {
            base.Init();
        }

        public static event Action onTowerPlaced;

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
                _decoyTower[_towerID].transform.position = hitInfo.point;

                //check spot 
                if (Input.GetMouseButtonDown(0) && _canPlaceTower == true)
                {
                    PlaceTower(_towerID, hitInfo.point);
                }
            }
        }


        public void PlaceTower(int i, Vector3 pos)
        {
         
            Instantiate(_tower[i], pos, Quaternion.identity);
            _canPlaceTower = false;
            if (onTowerPlaced != null)
            {
                onTowerPlaced();
            }
        }

        public void PlaceDecoyTower()
        {
            Instantiate(_decoyTower[_towerID], Input.mousePosition, Quaternion.identity);
        }

        public void ValidSpot()
        {
            _canPlaceTower = true;
        }
    }
}

