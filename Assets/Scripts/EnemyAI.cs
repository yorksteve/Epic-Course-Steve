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
        private Collider _mechColl;
        

        [SerializeField] private int _health;
        [SerializeField] private int _mechWarFund;
        [SerializeField] private int _damageAmount;
        [SerializeField] private GameObject _mechRotation;
        private Transform _rotationPoint;

        public delegate void MechDestroyed(int warFunds);
        public static MechDestroyed onMechDestroyed;

        public delegate void RecycleMech(GameObject mech);
        public static RecycleMech onRecycleMech;

        public delegate void TargetNew(GameObject mech);
        public static TargetNew onTargetNew;



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

            if (_mechColl != null)
            {
                _mechColl = this.gameObject.GetComponent<Collider>();
            }

            _rotationPoint = _mechRotation.GetComponent<Transform>();
        }
        
        private void OnEnable()
        {
            //EventManager.Listen("onDamage", Health(int, GameObject));
            EnemyDetection.onDamage += Health;

            //EventManager.Listen("onNewWave", ResetMech);
            SpawnManager.onNewWave += ResetMech;
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

            //EventManager.Fire("onRecycleMech", _mech);
            onRecycleMech?.Invoke(_mech);           
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
            return _damageAmount;
        }

        public void Health(int damage, GameObject mech)
        {
            if (mech == this.gameObject)
            {
                Debug.Log("EnemyAI::Health()");
                _health -= damage;

                if (_health <= 0)
                {
                    //EventManager.Fire("onMechDestroyed", _mechWarFund);
                    onMechDestroyed(_mechWarFund);
                    //EventManager.Fire("onTargetNew", mech);
                    onTargetNew?.Invoke(mech);
                    _mechColl.enabled = false;
                    StartCoroutine(DestroyMech());
                }
            }
        }

        private void OnDisable()
        {
            //EventManager.UnsubscribeEvent("onDamage", Health(damage, mech));
            EnemyDetection.onDamage -= Health;

            //EventManager.UnsubscribeEvent("onNewWave", ResetMech);
            SpawnManager.onNewWave -= ResetMech;
        }
    }
}

