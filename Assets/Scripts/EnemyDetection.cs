using Scripts.Interfaces;
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

        public delegate void Damage(int damage, GameObject mech);
        public static event Damage onDamage;


        private void OnEnable()
        {
            _towerPos = transform.parent;
            _towerParent = _towerPos.gameObject;
            _towerData = _towerParent.GetComponent<ITower>();
            _attackData = _towerParent.GetComponent<IAttack>();
            _damageAmount = _attackData.Damage();

            EnemyAI.onTargetNew += RemoveDestroyedMechs;
            //EventManager.Listen("onTargetNew", RemoveDestroyedMechs(GameObject mech));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                mechs.Add(other.gameObject);
                _targetEnemy = mechs[0];
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
            }
        }

        private void RemoveDestroyedMechs(GameObject mech)
        {
            mechs.Remove(mech);
            if (mechs.Count > 0)
            {
                _targetEnemy = mechs[0];
            }
            else
            {
                _attackData.Attack(false);
            }
        }

        private IEnumerator DamageMech(GameObject mech)
        {
            yield return new WaitForSeconds(2f);
            //EventManager.Fire("onDamage", _damageAmount, _targetEnemy);

            if (onDamage != null)
            {
                onDamage(_damageAmount, mech);
            }
        }

        private void OnDisable()
        {
            //EventManager.UnsubscribeEvent("onTargetNew", RemoveDestroyedMechs);
        }
    }
}

