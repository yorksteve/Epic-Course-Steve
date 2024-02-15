using System;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;

namespace Scripts.Managers
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        [SerializeField] private GameObject[] _mechs;
        [SerializeField] private GameObject _mechContainer;
        [SerializeField] private List<GameObject> _mechPool;

        private Transform _startPoint;
        private int _waveCount;
        private int _amountOfMechs = 10;


        public override void Init()
        {
            base.Init();
        }

        private void Start()
        {
            _startPoint = SpawnManager.Instance.RequestStartPoint();
            GenerateMech1();
            GenerateMech2();
        }

        private void OnEnable()
        {
            EventManager.Listen("onRecycleMech", (Action<GameObject>)RecycleMech);
        }

        GameObject CreateMech(int id)
        {
            GameObject mech = Instantiate(_mechs[id], _startPoint.position, Quaternion.identity);
            mech.transform.parent = _mechContainer.transform;
            mech.SetActive(false);
            _mechPool.Add(mech);

            return mech;
        }

        List<GameObject> GenerateMech1()
        {
            for (int i = 0; i < _amountOfMechs; i++)
            {
                GameObject mech1 = CreateMech(0);
            }

            return _mechPool;
        }

        List<GameObject> GenerateMech2()
        {
            for (int i = 0; i < _amountOfMechs; i++)
            {
                GameObject mech2 = CreateMech(1);
            }

            return _mechPool;
        }


        public GameObject GetMech(int id)
        {
            foreach (var mech in _mechPool)
            {
                if (mech.activeInHierarchy == false && mech.CompareTag(_mechs[id].tag))
                {
                    mech.transform.position = _startPoint.position;
                    mech.SetActive(true);
                    return mech;
                }
            }
            return CreateMech(id);
        }

        public void RecycleMech(GameObject mech)
        {
            mech.SetActive(false);
        }


        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onRecycleMech", (Action<GameObject>)RecycleMech);
        }
    }
}


