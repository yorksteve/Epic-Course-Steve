using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;

namespace Scripts.Manager
{
    public class SpawnManager : MonoSingleton<SpawnManager>
    {
        [SerializeField]
        private GameObject[] _mechs;

        private List<GameObject> _mechPool;

        [SerializeField]
        private GameObject _mechContainer;

        private int _waveCount = 1;
        [SerializeField] private int _amountOfMechs = 10;

        private Transform _startPoint;


        public override void Init()
        {
            base.Init();
        }

        private void Start()
        {
            StartCoroutine("StartRound");
        }

        GameObject CreateMech()
        {
            GameObject mech = Instantiate(_mechs[Random.Range(0, _mechs.Length)]);
            mech.transform.parent = _mechContainer.transform;
            //mech.SetActive(false);
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
            while (true) // number of mechs released <= total mechs for round
            {
                yield return new WaitForSeconds(2);
                GetMech();
            }

        }

        IEnumerator StartRound()
        {
            while (true)
            {
                _mechPool = GenerateMechs(_amountOfMechs * _waveCount);
                yield return new WaitForSeconds(60);
                StartWave();
            }
            
        }

    }


}

