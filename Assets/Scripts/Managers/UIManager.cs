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
        [SerializeField] private GameObject _sellDisplay;
        [SerializeField] private Text _warFunds;
        [SerializeField] private Text _sellAmount;
        private Transform _purchaseButton;

        public override void Init()
        {
            base.Init();
        }

        public void TowerUpgradeAbility(bool enoughFunds, GameObject tower)
        {
            if (tower == _towers[0])
            {
                var colorChange = _upgradeDisplay[0].GetComponent<Image>().color;
                _purchaseButton = _upgradeDisplay[0].transform.Find("PurchaseButton");

                if (colorChange != null && _purchaseButton != null)
                {
                    if (enoughFunds == false)
                    {
                        _purchaseButton.gameObject.GetComponent<Button>().enabled = false;
                        colorChange.a = 1;
                        _upgradeDisplay[0].SetActive(true);
                        Debug.Log("Not enough WarFunds to upgrade tower");
                    }
                    else if (enoughFunds == true)
                    {
                        _purchaseButton.gameObject.GetComponent<Button>().enabled = true;
                        colorChange.a = 0;
                        _upgradeDisplay[0].SetActive(true);
                    }
                }
                
            }
            else
            {
                var colorChange = _upgradeDisplay[1].GetComponent<Image>().color;
                _purchaseButton = _upgradeDisplay[1].transform.Find("PurchaseButton");

                if (colorChange != null && _purchaseButton != null)
                {
                    if (enoughFunds == false)
                    {
                        _purchaseButton.gameObject.GetComponent<Button>().enabled = false;  // Disable the upgrade button
                        colorChange.a = 1; // Gray out the upgrade button
                        _upgradeDisplay[1].SetActive(true);
                        Debug.Log("Not enough WarFunds to upgrade tower");
                    }
                    else if (enoughFunds == true)
                    {
                        _purchaseButton.gameObject.GetComponent<Button>().enabled = true;
                        colorChange.a = 0;
                        _upgradeDisplay[1].SetActive(true);
                    }
                }
                
            }
        }

        public void SellingTower(int towerWorth)
        {
            _sellAmount.text = towerWorth.ToString();
            _sellDisplay.SetActive(true);
        }

        public void SellTower()
        {
            TowerManager.Instance.SellTower();
            _sellDisplay.SetActive(false);
        }

        public void PurchaseUpgrade(int upgradeIndex)
        {
            TowerManager.Instance.UpgradeTower();
            _upgradeDisplay[upgradeIndex].SetActive(false);
        }

        public void CancelUpgrade(int i)
        {
            _upgradeDisplay[i].SetActive(false);
            Debug.Log("UIManager :: CancelUpgrade()");
        }

        public void ChangeFunds(int amount)
        {
            _warFunds.text = amount.ToString();
        }
    }
}

