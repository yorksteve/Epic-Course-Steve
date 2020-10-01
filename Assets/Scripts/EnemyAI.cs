using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Scripts.Managers;
using System;
using Scripts.Interfaces;
using UnityEngine.Animations;

namespace Scripts
{
    public class EnemyAI : MonoBehaviour, IAttack, IHealth
    {
        [SerializeField] private NavMeshAgent _agent;
        private Transform _destination;
        [SerializeField] private Animator _anim;
        [SerializeField] private GameObject _mechRotation;
        [SerializeField] private Collider _mechColl;
        [SerializeField] private ParentConstraint _parentConstraint;
        private Renderer[] _rends;
        private GameObject _targetMech;

        [SerializeField] private float _health;
        [SerializeField] private float _maxHealth;
        [SerializeField] private int _mechWarFund;
        [SerializeField] private float _damageAmount;
        private float _dissolve = 0;
        private float _speed = .1f;
        private Transform _rotationPoint;
        [SerializeField] private bool _isChecked;
        private WaitForSeconds _destroyMechYield;


        private void Start()
        {
            _destination = SpawnManager.Instance.RequestDestination();
            _maxHealth = _health;

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
            
            _rends = GetComponentsInChildren<Renderer>();

            if (_parentConstraint != null)
            {
                _parentConstraint.enabled = true;
            }

            _destroyMechYield = new WaitForSeconds(3);
        }

        private void OnEnable()
        {
            EventManager.Listen("onDamage", (Action<float>)Health);
            EventManager.Listen("onNewWave", ResetMech);           
            EventManager.Listen("onTargetTower", (Action<GameObject>)Target);
            EventManager.Listen("onCheckMech", (Action<GameObject>)CheckMech);
            EventManager.Listen("onMechExit", (Action<GameObject>)MechExit);
            EventManager.Listen("onCleaningMech", (Action<GameObject>)CleanUpMech);
            EventManager.Listen("onTargetedMech", (Action<GameObject>)TargetedMech);
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

        IEnumerator DestroyMech(GameObject mech)
        {
            if (_parentConstraint != null)
            {
                _parentConstraint.enabled = false;
            }
            _mechColl.enabled = false;
            _agent.isStopped = true;
            _health = 0f;
            _anim.SetBool("Die", true);
            yield return _destroyMechYield;
      
            _dissolve = Mathf.Clamp01(_dissolve += (_speed * Time.deltaTime));
            while (_dissolve < 1f)
            {
                Debug.Log("EnemyAI :: DestroyMech() : dissolve while loop");
                _dissolve = Mathf.Clamp01(_dissolve += _speed * Time.deltaTime);
                foreach (var rend in _rends)
                {
                    rend.material.SetFloat("_fillAmount", _dissolve);
                }

                if (_dissolve >= 1f)
                {
                    EventManager.Fire("onCleaningMech", this.gameObject);
                }
            }

        }

        void CleanUpMech(GameObject mech)
        {
            Debug.Log("CleanUpMech()");
            EventManager.Fire("onMechDestroyed", _mechWarFund);
            EventManager.Fire("onMechDestroyedSpawn", 1);
            _anim.WriteDefaultValues();
            foreach (var rend in _rends)
            {
                rend.material.SetFloat("_fillAmount", 0f);
            }
            EventManager.Fire("onRecycleMech", mech);
            EventManager.Fire("onResetHealth", _maxHealth, mech);
            _mechColl.enabled = true;
            if (_parentConstraint != null)
            {
                _parentConstraint.enabled = true;
            }
        }

        // Mechs can attack soldiers placed in the field (to be added later...probably)
        public void Attack(bool attack)
        {
            if (attack == true)
            {
                _anim.SetBool("Fire", true);
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
                Vector3 direction = enemy.transform.position - _rotationPoint.position;
                _rotationPoint.rotation = Quaternion.LookRotation(direction, Vector3.up);
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
        }

        public float Damage()
        {
            return _damageAmount;
        }

        private void AttackData(GameObject tower)
        {
            float damage = Damage();
            EventManager.Fire("onDamageTowers", damage, tower); // Fire event to TowerManager
        }

        public void TargetedMech(GameObject mech)
        {
            _targetMech = mech;
        }

        public void Health(float damage)
        {
            if (_targetMech == this.gameObject)
            {
                _health -= damage;
                EventManager.Fire("onHealthBarCube", _health, this.gameObject);

                if (_health <= 0)
                {
                    EventManager.Fire("onTargetNew", this.gameObject); // Fire event to EnemyDetection
                    StartCoroutine(DestroyMech(this.gameObject));
                }
            }
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onDamage", (Action<float>)Health);
            EventManager.UnsubscribeEvent("onNewWave", ResetMech);
            EventManager.UnsubscribeEvent("onTargetTower", (Action<GameObject>)Target);
            EventManager.UnsubscribeEvent("onCheckMech", (Action<GameObject>)CheckMech);
            EventManager.UnsubscribeEvent("onMechExit", (Action<GameObject>)MechExit);
            EventManager.UnsubscribeEvent("onCleaningMech", (Action<GameObject>)CleanUpMech);
            EventManager.UnsubscribeEvent("onTargetedMech", (Action<GameObject>)TargetedMech);
        }
    }
}

