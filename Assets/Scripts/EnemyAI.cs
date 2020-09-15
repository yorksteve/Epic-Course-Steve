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
        

        public int health;
        [SerializeField] private int _mechWarFund;

        public static event Action onMechDestroyed;

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
        }

        IEnumerator DestroyMech()
        {
            health = 0;
            _anim.SetBool("Die", true);
            yield return new WaitForSeconds(1.5f);
            this.gameObject.SetActive(false);
        }

        // Mechs can attack soldiers placed in the field (to be added later...probably)
        void IAttack.Attack()
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
                onMechDestroyed();
                health = 0;
                _agent.isStopped = true;
                // Increase War Fund based on value of mech destroyed
            }

            return health;
        }
    }
}

