using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;
using System;
using Scripts.ScriptableObjects;

namespace Scripts.Managers
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        [SerializeField] private GameObject[] _mechs;
        [SerializeField] private GameObject _mechContainer;
        [SerializeField] private List<GameObject> _mech1Pool;
        [SerializeField] private List<GameObject> _mech2Pool;

        private Transform _startPoint;
        private int _waveCount;
        private int _amountOfMechs = 10;


        public override void Init()
        {
            base.Init();
        }

        private void Start()
        {
            _mech1Pool = GenerateMech1(_amountOfMechs);
            _mech2Pool = GenerateMech2(_amountOfMechs);
            _startPoint = SpawnManager.Instance.RequestStartPoint();
            //_waveCount = SpawnManager.Instance.RequestWave();
        }

        private void OnEnable()
        {
            EventManager.Listen("onEndZoneReached", (Action<GameObject>)RecycleMech);
            EventManager.Listen("onRecycleMech", (Action<GameObject>)RecycleMech);
        }

        GameObject CreateMech(int i)
        {
            GameObject mech = Instantiate(_mechs[i], _startPoint.position, Quaternion.identity);
            mech.transform.parent = _mechContainer.transform;
            mech.SetActive(false);
            if (i == 0)
            {
                _mech1Pool.Add(mech);
            }
            else
            {
                _mech2Pool.Add(mech);
            }

            return mech;
        }


        List<GameObject> GenerateMech1(int amountOfMechs)
        {
            for (int i = 0; i < amountOfMechs; i++)
            {
                GameObject mech1 = CreateMech(0);
            }

            return _mech1Pool;
        }

        List<GameObject> GenerateMech2(int amountOfMechs)
        {
            for (int i = 0; i < amountOfMechs; i++)
            {
                GameObject mech2 = CreateMech(1);
            }

            return _mech2Pool;
        }


        public GameObject GetMech1()
        {
            foreach (var mech in _mech1Pool)
            {
                if (mech.activeInHierarchy == false)
                {
                    mech.transform.position = _startPoint.position;
                    mech.SetActive(true);
                    return mech;
                }
            }

            return CreateMech(0);
        }

        public GameObject GetMech2()
        {
            foreach (var mech in _mech2Pool)
            {
                if (mech.activeInHierarchy == false)
                {
                    mech.transform.position = _startPoint.position;
                    mech.SetActive(true);
                    return mech;
                }
            }

            return CreateMech(1);
        }

        public void RecycleMech(GameObject mech)
        {
            mech.SetActive(false);
        }


        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onEndZoneReached", (Action<GameObject>)RecycleMech);
            EventManager.UnsubscribeEvent("onRecycleMech", (Action<GameObject>)RecycleMech);
        }
    }
}


