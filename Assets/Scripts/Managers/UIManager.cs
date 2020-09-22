﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;
using UnityEngine.UI;

namespace Scripts.Managers
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField] private GameObject[] _towers;
        [SerializeField] private GameObject[] _upgradeDisplay;
        [SerializeField] private Text _warFunds;

        public override void Init()
        {
            base.Init();
        }

        public void TowerUpgradeAbility(bool enoughFunds, GameObject tower)
        {
            if (tower == _towers[0])
            {
                _upgradeDisplay[0].SetActive(true);
            }
            else
            {
                _upgradeDisplay[1].SetActive(true);
            }

            if (enoughFunds == false)
            {
                // Gray out and prevent upgrade
            }
            else
            {
                // Allow upgrade purchase
            }
        }

        public void SellTower(int id)
        {
            TowerManager.Instance.SellTower(id);
        }

        public void PurchaseUpgrade(int upgradeIndex)
        {
            TowerManager.Instance.UpgradeTower(upgradeIndex);
            _upgradeDisplay[upgradeIndex].SetActive(false);
        }

        public void CancelUpgrade(int i)
        {
            _upgradeDisplay[i].SetActive(false);
        }

        public void ChangeFunds(int amount)
        {
            _warFunds.text = amount.ToString();
        }
    }
}

