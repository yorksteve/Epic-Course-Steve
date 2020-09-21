using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;

namespace Scripts.Managers
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public override void Init()
        {
            base.Init();
        }

        public void TowerUpgradeAbility(bool enoughFunds, GameObject towerUpgrade)
        {
            // Gray out unrelated tower

            if (enoughFunds == false)
            {
                // Gray out and prevent upgrade
            }
            else
            {
                // Allow upgrade purchase
            }
        }

        public void SellTower()
        {
            TowerManager.Instance.SellTower();
        }

        public void PurchaseUpgrade(int upgradeIndex)
        {
            TowerManager.Instance.UpgradeTower(upgradeIndex);
        }
    }
}

