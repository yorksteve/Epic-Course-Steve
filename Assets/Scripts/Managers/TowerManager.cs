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
        private GameObject _currentTower;
        private GameObject _towerUpgrade;
        private GameObject _prefabDecoy;
        private Vector3 _towerPos;

        private ITower[] _towerData;
        private IAttack[] _attackData;

        private int _towerID;
        private int _warFundsRequired;
        private int _upgradeCost;
        private bool _placingTower;


        public override void Init()
        {
            base.Init();
        }

        private void Start()
        {
            _towerData = new ITower[_tower.Length];
            _attackData = new IAttack[_tower.Length];

            for (int i = 0; i < _tower.Length; i++)
            {
                _towerData[i] = _tower[i].GetComponent<ITower>();
                _attackData[i] = _tower[i].GetComponent<IAttack>();
            }            
        }

        private void OnEnable()
        {
            EventManager.Listen("onUpgradeTower", (Action<ITower, Vector3>)CheckUpgrade);
            EventManager.Listen("onDamageTowers", (Action<int, GameObject>)TowerDamaged);
        }

        public void Update()
        {
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (_placingTower == false)
                return;

            if (Physics.Raycast(rayOrigin, out hitInfo))
            {
                if (_prefabDecoy != null)
                {
                    _prefabDecoy.transform.position = hitInfo.point;
                }

                if (Input.GetMouseButtonDown(1))
                {
                    EventManager.Fire("onPlacingTower", false);                    
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
                WarFundManager.Instance.BuyTower(_warFundsRequired);
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

            EventManager.Fire("onPlacingTower", true);
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


        public void CheckUpgrade(ITower tower, Vector3 pos)
        {
            Debug.Log("TowerManager::CheckUpgrade()");

            _towerUpgrade = tower.UpgradeModel;
            _currentTower = tower.CurrentModel;
            _upgradeCost = tower.WarFundsRequiredUpgrade;
            _towerPos = pos;

            if (tower.WarFundsRequiredUpgrade <= WarFundManager.Instance.RequestWarFunds())
            {
                UIManager.Instance.TowerUpgradeAbility(false, _towerUpgrade);
            }
            else
            {
                UIManager.Instance.TowerUpgradeAbility(true, _towerUpgrade);
            }
        }

        public void SellTower(int id)
        {
            Destroy(_tower[id]);
            WarFundManager.Instance.SellTower(_towerData[id].WarFundsRequired);
        }

        public void UpgradeTower()
        {
            Instantiate(_towerUpgrade, _towerPos, Quaternion.identity);
            WarFundManager.Instance.BuyTower(_upgradeCost);
            EventManager.Fire("onUpdateTower", _towerUpgrade, _towerPos);
            Destroy(_currentTower);
        }

        public void TowerDamaged(int damageAmount, GameObject tower)
        {
            if (tower != null && damageAmount != 0)
            {
                tower.GetComponent<IHealth>().Health(damageAmount, tower);  // WHY DOES THIS NOT WORK??
            }
        }


        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onUpgradeTower", (Action<ITower, Vector3>)CheckUpgrade);
            EventManager.UnsubscribeEvent("onDamageTowers", (Action<int, GameObject>)TowerDamaged);
        }
    }
}

