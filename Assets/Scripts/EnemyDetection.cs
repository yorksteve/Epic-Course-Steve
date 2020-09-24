﻿using Scripts.Interfaces;
using Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scripts
{
    public class EnemyDetection : MonoBehaviour
    {
        private ITower _towerData;
        private IAttack _attackData;
        private Transform _towerPos;
        private GameObject _towerParent;
        private GameObject _targetEnemy;
        List<GameObject> mechs = new List<GameObject>();
        private int _damageAmount;


        private void OnEnable()
        {
            _towerPos = transform.parent;
            _towerParent = _towerPos.gameObject;
            _towerData = _towerParent.GetComponent<ITower>();
            _attackData = _towerParent.GetComponent<IAttack>();
            _damageAmount = _attackData.Damage();

            EventManager.Listen("onTargetNew", (Action<GameObject>)RemoveDestroyedMechs);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                mechs.Add(other.gameObject);
                _targetEnemy = mechs[0];
                EventManager.Fire("onTargetTower", this.gameObject);
                EventManager.Fire("onCheckMech", other.gameObject);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (_targetEnemy != null)
            {
                _attackData.Target(_targetEnemy);
                _attackData.Attack(true);
                StartCoroutine(DamageMech(_targetEnemy));
            }

            EventManager.Fire("onMechAttack", this.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                mechs.Remove(other.gameObject);
                if (mechs.Count > 0)
                {
                    _targetEnemy = mechs[0];
                }

                if (mechs.Count == 0)
                {
                    _attackData.Attack(false);
                }

                EventManager.Fire("onMechExit", other.gameObject);
            }
        }

        private void RemoveDestroyedMechs(GameObject mech)
        {
            mechs.Remove(mech);
            _attackData.Attack(false);
            if (mechs.Count > 0)
            {
                _targetEnemy = mechs[0];
                _attackData.Attack(true);
            }
        }

        private IEnumerator DamageMech(GameObject mech)
        {
            yield return new WaitForSeconds(2.5f);
            EventManager.Fire("onDamage", _damageAmount, _targetEnemy);
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onTargetNew", (Action<GameObject>)RemoveDestroyedMechs);
        }
    }
}

