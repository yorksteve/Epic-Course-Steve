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
        private Animator _anim;
        private ParticleSystem _explosion;
        private GameObject _mech;
        

        public int health;
        [SerializeField] private int _mechWarFund;

        public delegate void MechDestroyed(int warFunds);
        public static MechDestroyed onMechDestroyed;

        public delegate void RecycleMech(GameObject mech);
        public static RecycleMech onRecycleMech;

        public event Action onTarget;

        private void OnEnable()
        {
            //EnemyDetection.onDamage += Health;
        }

        private void Start()
        {
            _destination = SpawnManager.Instance.RequestDestination();

            if (_anim != null)
            {
                _anim = GetComponent<Animator>();
            }

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
                _explosion = _mech.GetComponent<ParticleSystem>();
            }
        }

        IEnumerator DestroyMech()
        {
            _agent.isStopped = true;
            health = 0;
            _explosion.Play();
            _anim.SetBool("Die", true);
            yield return new WaitForSeconds(5f);

            if (onRecycleMech != null)
            {
                onRecycleMech(_mech);
            }
        }

        // Mechs can attack soldiers placed in the field (to be added later...probably)
        void IAttack.Attack(bool attack)
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

        float IHealth.Health(int damage)
        {
            //health -= damageAmount; (dependant upon the weapon)            

            if (health <= 0 && onMechDestroyed != null)
            {
                onMechDestroyed(_mechWarFund);
                StartCoroutine(DestroyMech());
            }

            return health;
        }
    }
}

