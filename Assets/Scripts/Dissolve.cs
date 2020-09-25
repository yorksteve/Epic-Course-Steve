using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Managers;
using System;

public class Dissolve : MonoBehaviour
{
    [SerializeField] private Material _upperBody;
    [SerializeField] private Material _lowerBody;
    private MaterialPropertyBlock block;
    private bool _isDissolving;
    private float _dissolve = 0;
    private float _speed = .1f;



    private void Start()
    {
        block = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        EventManager.Listen("onDissolve", (Action<GameObject>)StartDissolve);
        EventManager.Listen("onStopDissolve", (Action<GameObject>)StopDissovle);
    }

    void Update()
    {
        if (_isDissolving == true)
        {
            _dissolve = Mathf.Clamp01(_dissolve += _speed * Time.deltaTime);
            block.SetFloat("_fillAmount", _dissolve);
            //SetPropertyBlock(block);
            
            if (_dissolve == 1f)
            {
                EventManager.Fire("onCleaningMech", this.gameObject);
            }
        }
    }

    void StartDissolve(GameObject mech)
    {
        if (mech == this.gameObject)
        {
            _isDissolving = true;
        }
    }

    void StopDissovle(GameObject mech)
    {
        if (mech == this.gameObject)
        {
            _isDissolving = false;
        }
    }

    private void OnDisable()
    {
        EventManager.UnsubscribeEvent("onDissolve", (Action<GameObject>)StartDissolve);
        EventManager.UnsubscribeEvent("onStopDissolve", (Action<GameObject>)StopDissovle);
    }
}
