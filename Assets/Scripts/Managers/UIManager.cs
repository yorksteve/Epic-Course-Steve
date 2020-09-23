using System.Collections;
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
                if (enoughFunds == false)
                {
                    _upgradeDisplay[0].GetComponent<Image>().color = Color.gray;
                }
                else if (enoughFunds == true)
                {
                    _upgradeDisplay[0].GetComponent<Image>().color = Color.white;
                }
            }
            else
            {
                _upgradeDisplay[1].SetActive(true);
                if (enoughFunds == false)
                {
                    _upgradeDisplay[1].GetComponent<Image>().color = Color.gray;
                }
                else if (enoughFunds == true)
                {
                    _upgradeDisplay[1].GetComponent<Image>().color = Color.white;
                }
            }
        }

        public void SellTower(int id)
        {
            TowerManager.Instance.SellTower(id);
        }

        public void PurchaseUpgrade(int upgradeIndex)
        {
            TowerManager.Instance.UpgradeTower();
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

