using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Scripts.Managers;
using UnityEngine.UI;

namespace Scripts
{
    public class HealthBarCube : MonoBehaviour
    {
        [SerializeField] private GameObject _currentObj;
        private float _maxScale = 10f;
        [SerializeField] private float _maxHealth;
        private float _currentHealth;
        private Material _material;
        private Renderer _rend;
        private float _healthPercent;

        private void OnEnable()
        {
            EventManager.Listen("onHealthBarCube", (Action<float, GameObject>)ModifyHealth);
        }

        private void Awake()
        {
            _rend = GetComponent<Renderer>();
            _material = _rend.materials[0];
            _healthPercent = 1f;
            gameObject.transform.localScale = new Vector3(_maxScale, .5f, .05f);
        }

        void Update()
        {
            Vector3 direction = Camera.main.transform.position - this.gameObject.transform.position;
            transform.LookAt(direction);
        }

        private void ModifyHealth(float health, GameObject obj)
        {
            if (obj == _currentObj)
            {
                Debug.Log("HealthBarCube :: ModifyHealth()");
                _currentHealth = health;
                _healthPercent = _currentHealth / _maxHealth;
                _material.SetFloat("_healthPercent", _healthPercent);
                gameObject.transform.localScale = new Vector3(Mathf.Clamp(_healthPercent * _maxScale, 0, _maxScale), .5f, .05f);
            }
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onHealthBarCube", (Action<float, GameObject>)ModifyHealth);
        }
    }
}

