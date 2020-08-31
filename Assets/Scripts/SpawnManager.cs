using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoSingleton<SpawnManager>
{
    private Transform _startPoint;
    [SerializeField] private GameObject[] _mechs;


    public override void Init()
    {
        base.Init();
    }


    public void SpawnMechs()
    {

    }
}
