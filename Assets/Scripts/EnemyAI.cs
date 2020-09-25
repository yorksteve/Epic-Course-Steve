using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Scripts.Managers;
using System;
using Scripts.Interfaces;

namespace Scripts
{
    public class EnemyAI : MonoBehaviour, IAttack, IHealth
    {
        private NavMeshAgent _agent;
        private Transform _destination;
        [SerializeField] private Animator _anim;
        [SerializeField] private GameObject _mechRotation;
        [SerializeField] private Collider _mechColl;        

        [SerializeField] private int _health;
        [SerializeField] private int _mechWarFund;
        [SerializeField] private int _damageAmount;
        private Transform _rotationPoint;
        private bool _isChecked;


        private void Start()
        {
            _destination = SpawnManager.Instance.RequestDestination();

            if (_agent != null)
            {
                _agent.SetDestination(_destination.position);
            }
            else
            {
                _agent = GetComponent<NavMeshAgent>();
                _agent.SetDestination(_destination.position);
            }
            _rotationPoint = _mechRotation.GetComponent<Transform>();
            transform.rotation = Quaternion.identity;
        }

        private void OnEnable()
        {
            EventManager.Listen("onDamage", (Action<int, GameObject>)Health);
            EventManager.Listen("onNewWave", ResetMech);           
            EventManager.Listen("onTargetTower", (Action<GameObject>)Target);
            EventManager.Listen("onCheckMech", (Action<GameObject>)CheckMech);
            EventManager.Listen("onMechExit", (Action<GameObject>)MechExit);
            EventManager.Listen("onCleaningMech", (Action<GameObject>)CleanUpMech);
        }

        private void ResetMech()
        {
            if (_agent != null)
            {
                _agent.SetDestination(_destination.position);
            }
            else
            {
                _agent = GetComponent<NavMeshAgent>();
                _agent.SetDestination(_destination.position);
            }
        }

        void DestroyMech(GameObject mech)
        {
            _mechColl.enabled = false;
            _agent.isStopped = true;
            _health = 0;
            _anim.SetBool("Die", true);
            EventManager.Fire("onDissolve", mech);
        }

        void CleanUpMech(GameObject mech)
        {
            EventManager.Fire("onMechDestroyed", _mechWarFund);
            _anim.WriteDefaultValues();
            _mechColl.enabled = true;
            EventManager.Fire("onRecycleMech", mech);
            EventManager.Fire("onStopDissolve", mech);
        }

        // Mechs can attack soldiers placed in the field (to be added later...probably)
        public void Attack(bool attack)
        {
            if (attack == true)
            {
                Debug.Log("EnemyAI :: Attack()");
                _anim.SetBool("Fire", true);
                Damage();
            }
            else
            {
                _anim.SetBool("Fire", false);
            }
        }

        private void CheckMech(GameObject mech)
        {
            if (mech == this.gameObject)
            {
                _isChecked = true;
                Debug.Log("EnemyAI :: CheckMech()");
            }
        }

        private void MechExit(GameObject mech)
        {
            if (mech == this.gameObject)
            {
                _isChecked = false;
            }
        }

        public void Target(GameObject enemy)
        {
            if (_isChecked == true)
            {
                Debug.Log("EnemyAI :: Target()");
                Vector3 direction = enemy.transform.position - _rotationPoint.position;
                _rotationPoint.transform.rotation = Quaternion.LookRotation(direction);
                if (enemy != null)
                {
                    Attack(true);
                    AttackData(enemy);
                }
                else
                {
                    Attack(false);
                }
            }
            else
            {
                transform.rotation = Quaternion.identity;
            }
        }

        public int Damage()
        {
            return _damageAmount;
        }

        private void AttackData(GameObject tower)
        {
            int damage = Damage();
            EventManager.Fire("onDamageTowers", damage, tower); // Fire event to TowerManager
        }

        public void Health(int damage, GameObject obj)
        {
            if (obj == this.gameObject)
            {
                _health -= damage;

                if (_health <= 0)
                {
                    EventManager.Fire("onTargetNew", this.gameObject); // Fire event to EnemyDetection
                    DestroyMech(obj);
                }
            }
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onDamage", (Action<int, GameObject>)Health);
            EventManager.UnsubscribeEvent("onNewWave", ResetMech);
            EventManager.UnsubscribeEvent("onTargetTower", (Action<GameObject>)Target);
            EventManager.UnsubscribeEvent("onCheckMech", (Action<GameObject>)CheckMech);
            EventManager.UnsubscribeEvent("onMechExit", (Action<GameObject>)MechExit);
            EventManager.UnsubscribeEvent("onCleaningMech", (Action<GameObject>)CleanUpMech);
        }
    }
}

