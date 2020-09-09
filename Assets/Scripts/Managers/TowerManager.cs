using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;

namespace Scripts.Managers
{
    public class TowerManager : MonoSingleton<TowerManager>
    {
        [SerializeField] private GameObject _decoyTower;
        [SerializeField] private GameObject _tower;

        RaycastHit hitInfo;


        public override void Init()
        {
            base.Init();
        }

        public void PlaceTower()
        {
            Instantiate(_tower, hitInfo.point, Quaternion.identity);
        }

        public void PlaceDecoyTower()
        {
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(rayOrigin, out hitInfo))
            {
                _decoyTower.transform.position = hitInfo.point;

                //if valid spot call PlaceTower();
            }
        }
    }
}

