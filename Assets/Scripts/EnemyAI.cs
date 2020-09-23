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
        [SerializeField] private ParticleSystem _explosion;
        [SerializeField] private GameObject _mechRotation;
        [SerializeField] private Collider _mechColl;
        private GameObject _mech;
        

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

            _mech = this.gameObject;

            if (_explosion != null)
            {
                _explosion = _mech.GetComponentInChildren<ParticleSystem>();
            }

            _rotationPoint = _mechRotation.GetComponent<Transform>();
        }
        
        private void OnEnable()
        {
            EventManager.Listen("onDamage", (Action<int, GameObject>)Health);
            EventManager.Listen("onNewWave", ResetMech);
            EventManager.Listen("onTargetTower", (Action<GameObject>)Target);
            EventManager.Listen("onCheckMech", (Action<GameObject>)CheckMech);
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

        IEnumerator DestroyMech()
        {
            _mechColl.enabled = false;
            _agent.isStopped = true;
            _health = 0;
            //_explosion.Play();
            _anim.SetBool("Die", true);
            EventManager.Fire("onDissolve", this.gameObject);
            yield return new WaitForSeconds(5f);

            EventManager.Fire("onMechDestroyed", _mechWarFund);
            _anim.WriteDefaultValues();
            _mechColl.enabled = true;

            EventManager.Fire("onRecycleMech", _mech);
            EventManager.Fire("onStopDissolve", this.gameObject);
        }

        // Mechs can attack soldiers placed in the field (to be added later...probably)
        public void Attack(bool attack)
        {
            if (attack == true)
            {
                _anim.SetTrigger("Fire");
                Damage();
            }
            else
            {
                _anim.ResetTrigger("Fire");
            }
        }

        public void Target(GameObject enemy)
        {
            if (_isChecked == true)
            {
                Vector3 direction = enemy.transform.position - _rotationPoint.position;
                transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
                if (enemy != null)
                {
                    Attack(true);
                }
                else
                {
                    Attack(false);
                }
            }
            // Use events to check enemies from OnTriggerExit, and reset position to forward facing
        }

        public int Damage()
        {
            // Fire event to damage towers
            EventManager.Fire("onDamageTowers", _damageAmount); // target tower needs to be passed in
            return _damageAmount;
        }

        public void Health(int damage, GameObject obj)
        {
            if (obj == this.gameObject)
            {
                _health -= damage;

                if (_health <= 0)
                {
                    EventManager.Fire("onTargetNew", this.gameObject);
                    StartCoroutine(DestroyMech());
                }
            }
        }

        private void CheckMech(GameObject mech)
        {
            if (mech == this.gameObject)
            {
                _isChecked = true;
            }
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onDamage", (Action<int, GameObject>)Health);
            EventManager.UnsubscribeEvent("onNewWave", ResetMech);
            EventManager.UnsubscribeEvent("onTargetTower", (Action<GameObject>)Target);
            EventManager.UnsubscribeEvent("onCheckMech", (Action<GameObject>)CheckMech);
        }
    }
}

