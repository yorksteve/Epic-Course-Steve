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

        private GameObject _prefab;
        private int _towerID;
        private int _warFundsRequired;


        public override void Init()
        {
            base.Init();
        }

        public static event Action onPlacingTower;
        public static event Action onCancelTower;

        public delegate void BoughtTower(int id);
        public static BoughtTower onBoughtTower;

        public void Update()
        {
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(rayOrigin, out hitInfo))
            {
                _prefab.transform.position = hitInfo.point;                

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

            if (onBoughtTower != null)
            {
                onBoughtTower(_towerID);
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
    }
}

