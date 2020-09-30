using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevHQ.FileBase.Missle_Launcher_Dual_Turret.Missle;
using Scripts.Interfaces;
using System;
using Scripts.Managers;

namespace GameDevHQ.FileBase.Missle_Launcher_Dual_Turret
{
    public class Missle_Launcher : MonoBehaviour, IAttack, ITower
    {
        [SerializeField]
        private GameObject _missilePrefab; //holds the missle gameobject to clone
        [SerializeField]
        private GameObject[] _misslePositionsLeft; //array to hold the rocket positions on the turret
        [SerializeField]
        private GameObject[] _misslePositionsRight; //array to hold the rocket positions on the turret
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

        [SerializeField] private GameObject _towerBase;
        private Transform _towerSource;
        private int _warFundsRequired = 750;

        public int WarFundsRequired { get => _warFundsRequired; set => _warFundsRequired = value; }
        public GameObject CurrentModel { get; set; }

        public GameObject UpgradeModel => throw new System.NotImplementedException();

        [SerializeField] private ParticleSystem _explosion;

        private void Start()
        {
            _towerSource = _towerBase.GetComponent<Transform>();
            CurrentModel = this.gameObject;
        }

        IEnumerator FireRocketsRoutine()
        {
            for (int i = 0; i < _misslePositionsLeft.Length; i++) //for loop to iterate through each missle position
            {
                GameObject rocketLeft = Instantiate(_missilePrefab) as GameObject; //instantiate a rocket
                GameObject rocketRight = Instantiate(_missilePrefab) as GameObject; //instantiate a rocket

                rocketLeft.transform.parent = _misslePositionsLeft[i].transform; //set the rockets parent to the missle launch position 
                rocketRight.transform.parent = _misslePositionsRight[i].transform; //set the rockets parent to the missle launch position 

                rocketLeft.transform.localPosition = Vector3.zero; //set the rocket position values to zero
                rocketRight.transform.localPosition = Vector3.zero; //set the rocket position values to zero

                rocketLeft.transform.localEulerAngles = new Vector3(0, 0, 0); //set the rotation values to be properly aligned with the rockets forward direction
                rocketRight.transform.localEulerAngles = new Vector3(0, 0, 0); //set the rotation values to be properly aligned with the rockets forward direction

                rocketLeft.transform.parent = null; //set the rocket parent to null
                rocketRight.transform.parent = null; //set the rocket parent to null

                rocketLeft.GetComponent<GameDevHQ.FileBase.Missle_Launcher_Dual_Turret.Missle.Missle>().AssignMissleRules(_launchSpeed, _power, _fuseDelay, _destroyTime); //assign missle properties 
                rocketRight.GetComponent<GameDevHQ.FileBase.Missle_Launcher_Dual_Turret.Missle.Missle>().AssignMissleRules(_launchSpeed, _power, _fuseDelay, _destroyTime); //assign missle properties 

                _misslePositionsLeft[i].SetActive(false); //turn off the rocket sitting in the turret to make it look like it fired
                _misslePositionsRight[i].SetActive(false); //turn off the rocket sitting in the turret to make it look like it fired

                Damage();

                yield return new WaitForSeconds(_fireDelay); //wait for the firedelay
            }

            for (int i = 0; i < _misslePositionsLeft.Length; i++) //itterate through missle positions
            {
                yield return new WaitForSeconds(_reloadTime); //wait for reload time
                _misslePositionsLeft[i].SetActive(true); //enable fake rocket to show ready to fire
                _misslePositionsRight[i].SetActive(true); //enable fake rocket to show ready to fire
            }

            _launched = false; //set launch bool to false
        }

        public void Attack(bool attack)
        {
            if (attack == true)
            {
                if (_launched == false)
                {
                    _launched = true; //set the launch bool to true
                    StartCoroutine(FireRocketsRoutine()); //start a coroutine that fires the rockets. 
                }
            }
        }

        public void Target(GameObject enemy)
        {
            Vector3 direction = enemy.transform.position - _towerSource.position;

            _towerSource.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        public float Damage()
        {
            float damageAmount = 8f;
            EventManager.Fire("onDamage", damageAmount);
            return damageAmount;
        }

        public void Health(float damage, GameObject obj)
        {
            if (obj == this.gameObject)
            {
                float health = 100f;
                health -= damage;
                EventManager.Fire("onHealthBar", health, obj);
                if (health <= 0f)
                {
                    _explosion.Play();
                    health = 0f;
                    Destroy(this.gameObject);
                    EventManager.Fire("onTowerDestroyed", this.transform.position);
                }
            }
        }
    }
}

