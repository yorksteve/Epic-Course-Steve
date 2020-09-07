using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Scripts.Managers;
using System;


namespace Scripts
{
    public class EnemyAI : MonoBehaviour
    {
        private NavMeshAgent _agent;
        private Transform _destination;
        private Animator _anim;

        public int health;
        //[SerializeField] private int _money = 500;
        [SerializeField] private int _mechMoney;
        private int _destroyedMechs;

        public static event Action onMechDestroyed;

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

        void Health()
        {
            //health -= damageAmount; (dependant upon the weapon)

            if (health <= 0 && onMechDestroyed != null)
            {
                onMechDestroyed();
                health = 0;
                // Increase War Fund based on value of mech destroyed
            }
        }


        // Mechs can attack soldiers placed in the field (to be added later...probably)
        void Attack()
        {
            _anim.SetBool("Attack", true);
        }

        IEnumerator DestroyMech()
        {
            health = 0;
            _anim.SetBool("Die", true);
            yield return new WaitForSeconds(1.5f);
            this.gameObject.SetActive(false);
            _destroyedMechs++;
        }
    }
}

