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
        private Camera _mainCamera;

        private ITower[] _towerData;
        private IAttack[] _attackData;

        private int _towerID;
        private int _warFundsRequired;
        private int _upgradeCost;
        private int _towerWorth;
        private bool _placingTower;
        private bool _gameStarted;


        public override void Init()
        {
            base.Init();
        }

        private void Awake()
        {
            _towerData = new ITower[_tower.Length];
            _attackData = new IAttack[_tower.Length];

            for (int i = 0; i < _tower.Length; i++)
            {
                _towerData[i] = _tower[i].GetComponent<ITower>();
                _attackData[i] = _tower[i].GetComponent<IAttack>();
            }

            _mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            EventManager.Listen("onUpgradeTower", (Action<ITower, Vector3>)CheckUpgrade);
            EventManager.Listen("onDamageTowers", (Action<float, GameObject>)TowerDamaged);
            EventManager.Listen("onStartingGame", OnGameStart);
        }

        private void OnGameStart()
        {
            _gameStarted = true;
        }

        public void Update()
        {
            if (_placingTower == false)
                return;

            Ray rayOrigin = _mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

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
            if (_warFundsRequired <= WarFundManager.Instance.RequestWarFunds())
            {
                var initial = Instantiate(_tower[_towerID], pos, Quaternion.identity);
                WarFundManager.Instance.BuyTower(_warFundsRequired);
                return initial.GetComponent<ITower>();
            }
            return null;
        }

        public void PlaceDecoyTower(int i)
        {
            if (_gameStarted == true)
            {
                _warFundsRequired = _towerData[i].WarFundsRequired;

                if (_warFundsRequired <= WarFundManager.Instance.RequestWarFunds())
                {
                    _placingTower = true;
                    _towerID = i;
                    _prefabDecoy = Instantiate(_decoyTower[i], Input.mousePosition, Quaternion.identity);

                    EventManager.Fire("onPlacingTower", true);
                }
                else
                {
                    Debug.Log("TowerManager::PlaceDecoyTower() : Not enough War Funds to buy this tower");
                }
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


        public void CheckUpgrade(ITower tower, Vector3 pos)
        {
            _towerUpgrade = tower.UpgradeModel;
            _currentTower = tower.CurrentModel;
            _towerPos = pos;

            if (_towerUpgrade != null)
            {
                _upgradeCost = _towerUpgrade.GetComponent<ITower>().WarFundsRequired;
                _towerWorth = tower.WarFundsRequired;
                UIManager.Instance.SellingTower(_towerWorth);

                if (_upgradeCost >= WarFundManager.Instance.RequestWarFunds())
                {
                    UIManager.Instance.TowerUpgradeAbility(false, _towerUpgrade);
                }
                else
                {
                    UIManager.Instance.TowerUpgradeAbility(true, _towerUpgrade);
                }
            }
            else
            {
                Debug.Log("Tower already upgraded");
                _towerWorth = tower.WarFundsRequired;
                UIManager.Instance.SellingTower(_towerWorth);
            }
        }

        public void SellTower()
        {
            Destroy(_currentTower);
            WarFundManager.Instance.SellTower(_towerWorth);
            EventManager.Fire("onTowerSold", _towerPos);
        }

        public void UpgradeTower()
        {
            Instantiate(_towerUpgrade, _towerPos, Quaternion.identity);
            WarFundManager.Instance.BuyTower(_upgradeCost);
            EventManager.Fire("onUpdateTower", _towerUpgrade, _towerPos);
            Destroy(_currentTower);
        }

        public void TowerDamaged(float damageAmount, GameObject tower)
        {            
            tower.GetComponent<ITower>().Health(damageAmount, tower);   
        }


        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onUpgradeTower", (Action<ITower, Vector3>)CheckUpgrade);
            EventManager.UnsubscribeEvent("onDamageTowers", (Action<float, GameObject>)TowerDamaged);
            EventManager.UnsubscribeEvent("onStartingGame", OnGameStart);
        }
    }
}

