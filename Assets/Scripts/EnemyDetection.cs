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
        private bool _inRange;

        public delegate void Damage(int damage);
        public static event Damage onDamage;


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
            if (other.gameObject.tag == "Enemy")
            {
                mechs.Add(other.gameObject);
                _targetEnemy = mechs[0];
                _inRange = true;
            }     
        }

        private void OnTriggerStay(Collider other)
        {
            if (_inRange == true)
            {
                _attackData.Target(_targetEnemy);
                _attackData.Attack(true);
            }       
        }

        private void OnTriggerExit(Collider other)
        {
            mechs.Remove(other.gameObject);
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

