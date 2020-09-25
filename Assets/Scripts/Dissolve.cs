using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Managers;
using System;

public class Dissolve : MonoBehaviour
{
    private Renderer[] _rends;
    private bool _isDissolving;
    private float _dissolve = 0;
    private float _speed = .1f;



    private void Start()
    {
        if (_rends != null)
        {
            _rends = GetComponentsInChildren<MeshRenderer>();
        }
        else
        {
            Debug.Log("Failed to get renderers");
        }
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
            foreach (var rend in _rends)
            {
                rend.material.SetFloat("_fillAmount", _dissolve);
            }    

            if (_dissolve >= 1f)
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
            foreach (var rend in _rends)
            {
                rend.material.SetFloat("_fillAmount", 0);
            }
        }
    }

    private void OnDisable()
    {
        EventManager.UnsubscribeEvent("onDissolve", (Action<GameObject>)StartDissolve);
        EventManager.UnsubscribeEvent("onStopDissolve", (Action<GameObject>)StopDissovle);
    }
}
