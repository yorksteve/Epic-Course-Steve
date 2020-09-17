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
        private GameObject _mech;
        

        [SerializeField] private int _health;
        [SerializeField] private int _mechWarFund;

        public delegate void MechDestroyed(int warFunds);
        public static MechDestroyed onMechDestroyed;

        public delegate void RecycleMech(GameObject mech);
        public static RecycleMech onRecycleMech;



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
        }
        
        private void OnEnable()
        {
            EnemyDetection.onDamage += Health;
            SpawnManager.onNewWave += ResetMech;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Health(5, _mech);
            }
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

            _anim.SetBool("Die", false);
        }

        IEnumerator DestroyMech()
        {
            _agent.isStopped = true;
            _health = 0;
            _explosion.Play();
            _anim.SetBool("Die", true);
            yield return new WaitForSeconds(5f);

            if (onRecycleMech != null)
            {
                onRecycleMech(_mech);
            }
        }

        // Mechs can attack soldiers placed in the field (to be added later...probably)
        public void Attack(bool attack)
        {
            _anim.SetBool("Attack", true);
        }

        public void Target(GameObject enemy)
        {
            throw new NotImplementedException();
        }

        public int Damage()
        {
            throw new NotImplementedException();
        }

        public void Health(int damage, GameObject mech)
        {
            mech = _mech;
            Debug.Log("EnemyAI::Health()");
            _health -= damage;

            Debug.Log(_health);

            if (_health <= 0 && onMechDestroyed != null)
            {
                onMechDestroyed(_mechWarFund);
                StartCoroutine(DestroyMech());
            }
        }

        private void OnDisable()
        {
            EnemyDetection.onDamage -= Health;
            SpawnManager.onNewWave -= ResetMech;
        }
    }
}

