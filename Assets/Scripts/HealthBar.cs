using Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider _healthSlider;
        [SerializeField] private GameObject _currentObj;
        private Camera _mainCamera;

        private void OnEnable()
        {
            EventManager.Listen("onHealthBar", (Action<float, GameObject>)ModifyHealth);
            EventManager.Listen("onResetHealthTower", (Action<float, GameObject>)SetMaxHealth);
        }

        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        void Update()
        {
            Vector3 direction = _mainCamera.transform.position - this.gameObject.transform.position;
            transform.LookAt(direction);
        }

        private void SetMaxHealth(float health, GameObject obj)
        {
            if (_currentObj == obj)
            {
                _healthSlider.maxValue = health;
                _healthSlider.value = health;
            }
        }

        private void ModifyHealth(float health, GameObject obj)
        {
            if (_currentObj == obj)
            {
                _healthSlider.value = health;
            }
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onHealthBar", (Action<float, GameObject>)ModifyHealth);
            EventManager.UnsubscribeEvent("onResetHealthTower", (Action<float, GameObject>)SetMaxHealth);
        }
    }
}

