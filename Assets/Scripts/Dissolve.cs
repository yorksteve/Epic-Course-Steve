using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Managers;
using System;

public class Dissolve : MonoBehaviour
{
    [SerializeField] private Material _materialTop;
    [SerializeField] private Material _materialBottom;
    private bool _isDissolving;
    private float _dissolve = 0;
    private float _speed = .1f;

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
            _materialTop.SetFloat("_fillAmount", _dissolve);
            _materialBottom.SetFloat("_fillAmount", _dissolve);
        }
    }

    // Research all dissolving instead of specific ones

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
            _materialBottom.SetFloat("_fillAmount", 0);
            _materialTop.SetFloat("_fillAmount", 0);
        }
    }

    private void OnDisable()
    {
        EventManager.UnsubscribeEvent("onDissolve", (Action<GameObject>)StartDissolve);
        EventManager.UnsubscribeEvent("onStopDissolve", (Action<GameObject>)StopDissovle);
    }
}
