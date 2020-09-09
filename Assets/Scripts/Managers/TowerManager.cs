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
                _decoyTower[0/* Of whatever tower is active*/].transform.position = hitInfo.point;

                //if valid spot call PlaceTower();
            }

            if (Input.GetMouseButtonDown(0))
            {
                PlaceTower();
            }
        }


        public void PlaceTower()
        {
            //Instantiate(_tower, hitInfo.point, Quaternion.identity);
            onTowerPlaced();
        }

        public void PlaceDecoyTower()
        {
            Instantiate(_decoyTower[0], Input.mousePosition, Quaternion.identity);
        }

        public void ValidSpot()
        {
            _canPlaceTower = true;
        }
    }
}

