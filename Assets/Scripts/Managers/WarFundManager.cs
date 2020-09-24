using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;
using System;

namespace Scripts.Managers
{
    public class WarFundManager : MonoSingleton<WarFundManager>
    {
        [SerializeField] private int _warFunds;

        private void OnEnable()
        {
            EventManager.Listen("onMechDestroyed", (Action<int>)DestroyedMech);
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

        public void DestroyedMech(int mechWarFunds)
        {
            _warFunds += mechWarFunds;
            UpdateFunds(_warFunds);
        }

        public void BuyTower(int fundsRequired)
        {
            _warFunds -= fundsRequired;
            UpdateFunds(_warFunds);
        }

        public void SellTower(int funds)
        {
            _warFunds += funds;
            UpdateFunds(_warFunds);
        }

        public int UpdateFunds(int WarFunds)
        {
            WarFunds = _warFunds;
            UIManager.Instance.ChangeFunds(_warFunds);
            return _warFunds;
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onMechDestroyed", (Action<int>)DestroyedMech);
        }
    }
}

