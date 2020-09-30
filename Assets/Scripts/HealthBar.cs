﻿using Scripts.Managers;
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

        private void OnEnable()
        {
            EventManager.Listen("onHealthBar", (Action<float, GameObject>)ModifyHealth);
            EventManager.Listen("onResetHealth", (Action<float, GameObject>)SetMaxHealth);
        }

        void Update()
        {
            Vector3 direction = Camera.main.transform.position - this.gameObject.transform.position;
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
                Debug.Log("HealthBar :: ModifyHealth()");
                _healthSlider.value = health;
            }
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onHealthBar", (Action<float, GameObject>)ModifyHealth);
            EventManager.UnsubscribeEvent("onResetHealth", (Action<float, GameObject>)SetMaxHealth);
        }
    }
}

