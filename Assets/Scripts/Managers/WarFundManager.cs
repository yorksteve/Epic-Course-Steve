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
        [SerializeField] private int _warFunds;

        private int[] _warFundsRequired;

        public static event Action onLackingFunds;

        private void OnEnable()
        {
            TowerManager.onPlacingTower += CheckWarFunds;
            TowerManager.onBoughtTower += BuyTower;
        }

        private void OnDisable()
        {
            TowerManager.onPlacingTower -= CheckWarFunds;
            TowerManager.onBoughtTower -= BuyTower;
        }

        public override void Init()
        {
            base.Init();
        }

        public void DestroyedMech()
        {
            // Increase warFunds
        }

        public void BuyTower(int id)
        {
            //_warFunds -= _warFundsRequired[id];
        }

        public void CheckWarFunds()
        {
            //if (_warFunds < _warFundsRequired[id])
            //{
            //    Debug.Log("Not enought WarFunds");

            //    if (onLackingFunds != null)
            //    {
            //        onLackingFunds();
            //    }
            //}

            //else
            //{
            //    Debug.Log("You're good to buy tower!");
            //}
        }
    }
}

