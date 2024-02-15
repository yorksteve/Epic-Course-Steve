﻿using UnityEngine;

namespace Scripts
{
    public class HealthBarCube : MonoBehaviour
    {
        [SerializeField] private GameObject _currentObj;
        private GameObject _mainCamera;
        [SerializeField] private float _maxScale;// = 10f;
        [SerializeField] private float _maxHealth;
        private float _currentHealth;
        private Material _material;
        private Renderer _rend;
        private float _healthPercent;

        private void Awake()
        {
            _rend = GetComponent<Renderer>();
            _material = _rend.materials[0];
            _healthPercent = 1f;
            gameObject.transform.localScale = new Vector3(_maxScale, .5f, .05f);
            _mainCamera = Camera.main.gameObject;
        }

        void Update()
        {
            Vector3 direction = _mainCamera.transform.position - this.gameObject.transform.position;
            transform.LookAt(direction);
        }

        public void Reset()
        {
            ModifyHealth(_maxHealth);
        }

        public void ModifyHealth(float health)
        {
            _currentHealth = health;
            _healthPercent = _currentHealth / _maxHealth;
            _material.SetFloat("_healthPercent", _healthPercent);
            gameObject.transform.localScale = new Vector3(Mathf.Clamp(_healthPercent * _maxScale, 0, _maxScale), .5f, .05f);
        }
    }
}

