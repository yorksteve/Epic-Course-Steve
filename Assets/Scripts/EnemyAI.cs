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
            _agent.isStopped = true;
            _health = 0;
            _explosion.Play();
            _anim.SetBool("Die", true);
            yield return new WaitForSeconds(5f);

            _anim.WriteDefaultValues();
            _mechColl.enabled = true;

            EventManager.Fire("onRecycleMech", _mech);
        }

        // Mechs can attack soldiers placed in the field (to be added later...probably)
        public void Attack(bool attack)
        {
            _anim.SetTrigger("Fire");
            Damage();
        }

        public void Target(GameObject enemy)
        {
            Vector3 direction = enemy.transform.position - _rotationPoint.position;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
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
                Debug.Log("EnemyAI::Health()");
                _health -= damage;

                if (_health <= 0)
                {
                    EventManager.Fire("onMechDestroyed", _mechWarFund);
                    EventManager.Fire("onTargetNew", obj);
                    _mechColl.enabled = false;
                    StartCoroutine(DestroyMech());
                }
            }
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onDamage", (Action<int, GameObject>)Health);
            EventManager.UnsubscribeEvent("onNewWave", ResetMech);
        }
    }
}

