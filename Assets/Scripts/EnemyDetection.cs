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
                _inRange = true;
                //StartCoroutine(DamageMech());
            }     
        }

        private void OnTriggerStay(Collider other)
        {
            if (_inRange == true)
            {
                _targetEnemy = mechs[0];
                _attackData.Target(_targetEnemy);
                _attackData.Attack(true);
            }       
        }

        private void OnTriggerExit(Collider other)
        {
            mechs.Remove(other.gameObject);
            _inRange = false;

            //if (_targetEnemy == null)
            //{
            _attackData.Attack(false);
            //}
        }

        private IEnumerator DamageMech()
        {
            // I only want this to be called on the targeted mech
            if (_inRange == true && _targetEnemy != null)
            {
                yield return new WaitForSeconds(.5f);
                if (onDamage != null)
                {
                    onDamage(_damageAmount);
                }
            }
        }
    }
}

