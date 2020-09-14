using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;
using GameDevHQ.FileBase.Missile_Launcher;
using GameDevHQ.FileBase.Gatling_Gun;
using Scripts.Interfaces;
using System;

namespace Scripts.Managers
{
    public class WarFundManager : MonoSingleton<WarFundManager>
    {
        [SerializeField] private int _warFunds = 500;

        public static event Action onLackingFunds;

        private void OnEnable()
        {
            TowerManager.onBoughtTower += BuyTower;
            EnemyAI.onMechDestroyed += DestroyedMech;
        }

        public override void Init()
        {
            base.Init();
        }

        public int RequestWarFunds()
        {
            Debug.Log("WarFundManager::RequestWarFunds() : " + _warFunds);
            return _warFunds;
        }

        public void DestroyedMech()
        {
            // Increase warFunds
        }

        public void BuyTower(int fundsRequired)
        {
            _warFunds -= fundsRequired;
            UpdateFunds(_warFunds);
        }

        public int UpdateFunds(int WarFunds)
        {
            return WarFunds;
        }

        private void OnDisable()
        {
            TowerManager.onBoughtTower -= BuyTower;
            EnemyAI.onMechDestroyed -= DestroyedMech;
        }
    }
}

