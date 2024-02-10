﻿using System;
using System.Collections;
using Scripts.Interfaces;
using Scripts.Managers;
using UnityEngine;

/*
 *@author GameDevHQ 
 * For support, visit gamedevhq.com
 */

namespace GameDevHQ.FileBase.Missile_Launcher
{
    public class Missile_Launcher : MonoBehaviour, ITower, IAttack
    {
        public enum MissileType
        {
            Normal,
            Homing
        }


        [SerializeField]
        private GameObject _missilePrefab; //holds the missle gameobject to clone
        [SerializeField]
        private MissileType _missileType; //type of missle to be launched
        [SerializeField]
        private GameObject[] _misslePositions; //array to hold the rocket positions on the turret
        [SerializeField]
        private float _fireDelay; //fire delay between rockets
        [SerializeField]
        private float _launchSpeed; //initial launch speed of the rocket
        [SerializeField]
        private float _power; //power to apply to the force of the rocket
        [SerializeField]
        private float _fuseDelay; //fuse delay before the rocket launches
        [SerializeField]
        private float _reloadTime; //time in between reloading the rockets
        [SerializeField]
        private float _destroyTime = 10.0f; //how long till the rockets get cleaned up
        private bool _launched; //bool to check if we launched the rockets
        [SerializeField]
        private Transform _target; //Who should the rocket fire at?

        [SerializeField] private ParticleSystem _explosion;

        [SerializeField] private int _warFundsRequired = 500;
        [SerializeField] private GameObject _upgradeModel;
        public int WarFundsRequired { get => _warFundsRequired; set => _warFundsRequired = value; }
        public GameObject CurrentModel { get; set; }
        public GameObject UpgradeModel { get => _upgradeModel; }

        [SerializeField] private GameObject _towerBase;
        private Transform _towerSource;

        private float _maxHealth = 50f;
        private float _damageAmount = 2f;


        private void Start()
        {
            CurrentModel = this.gameObject;

            _towerSource = _towerBase.GetComponent<Transform>();
        }

        IEnumerator FireRocketsRoutine()
        {
            for (int i = 0; i < _misslePositions.Length; i++) //for loop to iterate through each missle position
            {
                GameObject rocket = Instantiate(_missilePrefab) as GameObject; //instantiate a rocket

                rocket.transform.parent = _misslePositions[i].transform; //set the rockets parent to the missle launch position 
                rocket.transform.localPosition = Vector3.zero; //set the rocket position values to zero
                rocket.transform.localEulerAngles = new Vector3(-90, 0, 0); //set the rotation values to be properly aligned with the rockets forward direction
                rocket.transform.parent = null; //set the rocket parent to null

                rocket.GetComponent<GameDevHQ.FileBase.Missile_Launcher.Missile.Missile>().AssignMissleRules(_missileType, _target, _launchSpeed, _power, _fuseDelay, _destroyTime); //assign missle properties 

                _misslePositions[i].SetActive(false); //turn off the rocket sitting in the turret to make it look like it fired

                Damage();

                yield return new WaitForSeconds(_fireDelay); //wait for the firedelay
            }

            for (int i = 0; i < _misslePositions.Length; i++) //itterate through missle positions
            {
                yield return new WaitForSeconds(_reloadTime); //wait for reload time
                _misslePositions[i].SetActive(true); //enable fake rocket to show ready to fire
            }

            _launched = false; //set launch bool to false
        }

        public void Attack(bool attack)
        {
            if (_launched == false && attack == true)
            {
                _launched = true; //set the launch bool to true
                StartCoroutine(FireRocketsRoutine()); //start a coroutine that fires the rockets. 
            }
        }

        public float Damage()
        {
            EventManager.Fire("onDamage", _damageAmount);
            return _damageAmount;
        }

        public void Target(GameObject enemy)
        {
            _target = enemy.transform;
            Vector3 direction = enemy.transform.position - _towerSource.position;

            _towerSource.rotation = Quaternion.LookRotation(direction, Vector3.up);
            EventManager.Fire("onTargetedMech", enemy);
        }

        private void OnEnable()
        {
            EventManager.Listen("onDamageTowers", (Action<float, GameObject>)Health);
        }

        public void Health(float damage, GameObject obj)
        {
            if (obj == this.gameObject)
            {
                float health = _maxHealth;
                health -= damage;
                EventManager.Fire("onHealthBarCube", health, obj);
                if (health <= 0f)
                {
                    _explosion.Play();
                    health = 0f;
                    Destroy(this.gameObject);
                    EventManager.Fire("onTowerDestroyed", this.transform.position);
                }
            }
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onDamageTowers", (Action<float, GameObject>)Health);
        }

        public float EditorGetHealth()
        {
            return _maxHealth;
        }

        public float EditorGetDamage()
        {
            return _damageAmount;
        }

        public float EditorSetHealth(float health)
        {
            _maxHealth = health;
            return _maxHealth;
        }

        public float EditorSetDamage(float damage)
        {
            _damageAmount = damage;
            return _damageAmount;
        }
    }
}

