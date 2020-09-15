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
        Queue<GameObject> mechs = new Queue<GameObject>();
        private int _damageAmount;
        private bool _inRange;

        public delegate void Damage(int damage);
        public static Damage onDamage;


        private void OnEnable()
        {
            _towerPos = transform.parent;
            _towerParent = _towerPos.gameObject;
            _towerData = _towerParent.GetComponent<ITower>();
            _attackData = _towerParent.GetComponent<IAttack>();
            _damageAmount = _attackData.Damage();
        }

        private void OnTriggerEnter(Collider other)
        {
            mechs.Enqueue(other.gameObject);
            _targetEnemy = mechs.Peek();
            _inRange = true;
        }

        private void OnTriggerStay(Collider other)
        {
            _attackData.Target(_targetEnemy);
            _attackData.Attack(true);
        }

        private void OnTriggerExit(Collider other)
        {
            mechs.Dequeue();
            _inRange = false;

            if (_targetEnemy == null)
            {
                _attackData.Attack(false);
            }
        }

        private IEnumerator DamageMech()
        {
            // I only want this to be called on the targeted mech
            while (_inRange == true)
            {
                yield return new WaitForSeconds(1f);
                if (onDamage != null)
                {
                    onDamage(_damageAmount);
                }
            }
        }
    }
}

