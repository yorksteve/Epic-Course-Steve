﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;

namespace Scripts.Manager
{
    public class SpawnManager : MonoSingleton<SpawnManager>
    {
        [SerializeField] private GameObject[] _mechs;
        [SerializeField] private int _amountOfMechs = 10;
        [SerializeField] private GameObject _mechContainer;

        [SerializeField] private List<GameObject> _mechPool;
        private int _waveCount = 1;
        private Transform _startPoint;
        [SerializeField] private Transform _destination;

        public override void Init()
        {
            base.Init();
        }

        private void Start()
        {
            //StartCoroutine("StartRound");
            _mechPool = GenerateMechs(_amountOfMechs * _waveCount);
            StartWave();
        }

        GameObject CreateMech()
        {
            GameObject mech = Instantiate(_mechs[Random.Range(0, _mechs.Length)]);
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
                    mech.SetActive(true);
                    return mech;
                }
            }

            return CreateMech();
        }

        public void StartWave()
        {
            StartCoroutine(SpawnTime());
            
            _waveCount++;
        }

        IEnumerator SpawnTime()
        {
            for (int i = 0; i <= (_amountOfMechs * _waveCount); i++)
            {
                yield return new WaitForSeconds(5);
                GetMech();
            }

            Debug.Log("Spawning for current wave finished");

        }

        public Transform RequestDestination()
        {
            return _destination;
        }
    }


}

