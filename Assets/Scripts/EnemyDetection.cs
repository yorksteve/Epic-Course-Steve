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
        private GameObject _towerParent;


        private void OnEnable()
        {
            _towerData = GetComponent<ITower>();
            _attackData = GetComponent<IAttack>();
            _towerParent = GetComponentInParent<GameObject>();
        }

        private void OnTriggerEnter(Collider other)
        {

        }

        private void OnTriggerStay(Collider other)
        {
            _attackData.Target(other.gameObject);
            _attackData.Attack();
        }

        private void OnTriggerExit(Collider other)
        {

        }
    }
}

