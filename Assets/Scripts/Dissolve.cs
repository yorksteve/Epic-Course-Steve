using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Managers;

public class Dissolve : MonoBehaviour
{
    [SerializeField] private Material _materialTop;
    [SerializeField] private Material _materialBottom;
    private bool _isDissolving;
    private float _dissolve = 0;
    private float _speed = .1f;

    private void OnEnable()
    {
        EventManager.Listen("onDissolve", StartDissolve);
        EventManager.Listen("onStopDissolve", StopDissovle);
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

    void StartDissolve()
    {
        _isDissolving = true;
    }

    void StopDissovle()
    {
        _isDissolving = false;
    }

    private void OnDisable()
    {
        EventManager.UnsubscribeEvent("onDissolve", StartDissolve);
        EventManager.UnsubscribeEvent("onStopDissolve", StopDissovle);
    }
}
