using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;

namespace Scripts.Managers
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField] private GameObject[] _towers;
        [SerializeField] private GameObject[] _upgradeDisplay;

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
        }
    }
}

