using Scripts.Interfaces;
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
        private ITower[] _towerData;
        private IAttack[] _attackData;

        private GameObject _prefabDecoy;
        private int _towerID;
        private int _warFundsRequired;
        private bool _placingTower;


        public override void Init()
        {
            base.Init();
        }

        public static event Action<bool> onPlacingTower;

        public delegate void BoughtTower(int id);
        public static BoughtTower onBoughtTower;

        private void Start()
        {
            _towerData = new ITower[_tower.Length];
            _attackData = new IAttack[_tower.Length];

            for (int i = 0; i < _tower.Length; i++)
            {
                _towerData[i] = _tower[i].GetComponent<ITower>();
                _attackData[i] = _tower[i].GetComponent<IAttack>();
                Debug.Log(_towerData[i].WarFundsRequired);
            }            
        }

        public void Update()
        {
            if (_placingTower == false)
                return;

            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(rayOrigin, out hitInfo))
            {
                if (_prefabDecoy != null)
                {
                    _prefabDecoy.transform.position = hitInfo.point;
                }

                if (Input.GetMouseButtonDown(1))
                {
                    if (onPlacingTower != null)
                    {
                        onPlacingTower(false);
                    }
                    Destroy(_prefabDecoy);
                }
            }
        }

        public ITower PlaceTower(Vector3 pos)
        {
            Debug.Log("TowerManager::PlaceTower()");

            _warFundsRequired = _towerData[_towerID].WarFundsRequired;

            if (_warFundsRequired <= WarFundManager.Instance.RequestWarFunds())
            {
                var initial = Instantiate(_tower[_towerID], pos, Quaternion.identity);

                if (onBoughtTower != null)
                {
                    onBoughtTower(_warFundsRequired);
                }

                return initial.GetComponent<ITower>();
            }

            else
            {
                Debug.Log("TowerManager::PlaceTower() : Not enough War Funds to buy this tower");
            }


            return null;
        }

        public void PlaceDecoyTower(int i)
        {
            _placingTower = true;
            _towerID = i;
            _prefabDecoy = Instantiate(_decoyTower[i], Input.mousePosition, Quaternion.identity);

            if (onPlacingTower != null)
            {
                onPlacingTower(true);
            }
        }

        public void SnapToPosition(Vector3 pos)
        {
            _placingTower = false;
            if (_prefabDecoy != null)
            {
                _prefabDecoy.transform.position = pos;
            }
        }

        public void ReleaseSnap()
        {
            _placingTower = true;
        }

        public int RequestDamage(int i)
        {
            int damage = _attackData[i].Damage();
            return damage;
        }
    }
}

