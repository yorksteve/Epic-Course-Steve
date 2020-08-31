using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoSingleton<SpawnManager>
{
    [SerializeField]
    private GameObject[] _mechs;

    private List<GameObject> _mechPool;

    [SerializeField]
    private GameObject _mechContainer;

    private int _waveCount;
    [SerializeField] private int _amountOfMechs;
    private Transform _startPoint;


    public override void Init()
    {
        base.Init();
    }

    private void Start()
    {
        _mechPool = GenerateMechs(_amountOfMechs * _waveCount);
    }

    List<GameObject> GenerateMechs(int amountOfMechs)
    {
        for (int i = 0; i < amountOfMechs; i++)
        {
            GameObject mech = Instantiate(_mechs[Random.Range(0, _mechs.Length)]);
            mech.transform.parent = _mechContainer.transform;
            mech.SetActive(false);
            _mechPool.Add(mech);
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

        return null;
    }

}
