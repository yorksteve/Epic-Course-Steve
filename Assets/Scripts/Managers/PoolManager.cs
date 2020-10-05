using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;
using System;


namespace Scripts.Managers
{
    public class PoolManager : MonoSingleton<PoolManager>
    {
        [SerializeField] private GameObject[] _mechs;
        [SerializeField] private int _amountOfMechs = 10;
        [SerializeField] private GameObject _mechContainer;
        [SerializeField] private List<GameObject> _mechPool;

        private Transform _startPoint;
        private int _waveCount;


        public override void Init()
        {
            base.Init();
        }

        private void Awake()
        {
            _startPoint = SpawnManager.Instance.RequestStartPoint();
            _waveCount = SpawnManager.Instance.RequestWave();
        }

        private void Start()
        {
            _mechPool = GenerateMechs(_amountOfMechs * _waveCount);
        }

        private void OnEnable()
        {
            EventManager.Listen("onEndZoneReached", (Action<GameObject>)RecycleMech);
            EventManager.Listen("onRecycleMech", (Action<GameObject>)RecycleMech);
        }

        GameObject CreateMech()
        {
            GameObject mech = Instantiate(_mechs[UnityEngine.Random.Range(0, _mechs.Length)], _startPoint.position, Quaternion.identity);
            mech.transform.parent = _mechContainer.transform;
            mech.SetActive(false);
            _mechPool.Add(mech);

            return mech;
        }


        List<GameObject> GenerateMechs(int amountOfMechs)
        {
            for (int i = 0; i < amountOfMechs; i++)
            {
                GameObject mech = CreateMech();
            }

            return _mechPool;
        }


        public GameObject GetMech()
        {
            foreach (var mech in _mechPool)
            {
                if (mech.activeInHierarchy == false)
                {
                    mech.transform.position = _startPoint.position;
                    mech.SetActive(true);
                    return mech;
                }
            }

            return CreateMech();
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


