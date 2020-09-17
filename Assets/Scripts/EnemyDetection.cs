using Scripts.Interfaces;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
                //StartCoroutine(DamageMech());
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

                if (_targetEnemy == null)
                {
                    _attackData.Attack(false);
                }
            }
        }

        private void RemoveDestroyedMechs(Collider mechCollider)
        {
            mechs.Remove(mechCollider.gameObject);
            if (mechs.Count > 0)
            {
                _targetEnemy = mechs[0];
            }
        }

        private IEnumerator DamageMech()
        {
            if (_targetEnemy != null)
            {
                yield return new WaitForSeconds(1f);
                if (onDamage != null)
                {
                    onDamage(_damageAmount, _targetEnemy);
                }
            }
        }
    }
}

