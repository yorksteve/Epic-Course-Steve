using Scripts.Interfaces;
using Scripts.Managers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GameDevHQ.FileBase.Dual_Gatling_Gun
{
    /// <summary>
    /// This script will allow you to view the presentation of the Turret and use it within your project.
    /// Please feel free to extend this script however you'd like. To access this script from another script
    /// (Script Communication using GetComponent) -- You must include the namespace (using statements) at the top. 
    /// "using GameDevHQ.FileBase.Dual_Gatling_Gun" without the quotes. 
    /// 
    /// For more, visit GameDevHQ.com
    /// 
    /// @authors
    /// Al Heck
    /// Jonathan Weinberger
    /// </summary>

    [RequireComponent(typeof(AudioSource))] //Require Audio Source component
    public class Dual_Gatling_Gun : MonoBehaviour, IAttack, ITower, IHealth
    {
        [SerializeField]
        private Transform[] _gunBarrel; //Reference to hold the gun barrel
        [SerializeField]
        private GameObject[] _muzzleFlash; //reference to the muzzle flash effect to play when firing
        [SerializeField]
        private ParticleSystem[] _bulletCasings; //reference to the bullet casing effect to play when firing
        [SerializeField]
        private AudioClip _fireSound; //Reference to the audio clip

        private AudioSource _audioSource; //reference to the audio source component
        private bool _startWeaponNoise = true;

        [SerializeField] private int _warFundsRequired = 500;
        [SerializeField] private GameObject _towerBase;
        private Transform _towerSource;

        [SerializeField] private ParticleSystem _explosion;


        public int WarFundsRequired { get => _warFundsRequired; set => _warFundsRequired = value; }
        public GameObject CurrentModel { get; set; }

        public GameObject UpgradeModel => throw new System.NotImplementedException();

        // Use this for initialization
        void Start()
        {
            CurrentModel = this.gameObject;

            _muzzleFlash[0].SetActive(false); //setting the initial state of the muzzle flash effect to off
            _muzzleFlash[1].SetActive(false); //setting the initial state of the muzzle flash effect to off
            _audioSource = GetComponent<AudioSource>(); //ssign the Audio Source to the reference variable
            _audioSource.playOnAwake = false; //disabling play on awake
            _audioSource.loop = true; //making sure our sound effect loops
            _audioSource.clip = _fireSound; //assign the clip to play

            _towerSource = _towerBase.GetComponent<Transform>();
        }

        // Method to rotate gun barrel 
        void RotateBarrel() 
        {
            _gunBarrel[0].transform.Rotate(Vector3.forward * Time.deltaTime * -500.0f); //rotate the gun barrel along the "forward" (z) axis at 500 meters per second
            _gunBarrel[1].transform.Rotate(Vector3.forward * Time.deltaTime * -500.0f); //rotate the gun barrel along the "forward" (z) axis at 500 meters per second
        }

        public void Attack(bool attack)
        {
            if (attack == true)
            {
                RotateBarrel(); //Call the rotation function responsible for rotating our gun barrel

                //for loop to iterate through all muzzle flash objects
                for (int i = 0; i < _muzzleFlash.Length; i++)
                {
                    _muzzleFlash[i].SetActive(true); //enable muzzle effect particle effect
                    _bulletCasings[i].Emit(1); //Emit the bullet casing particle effect   
                }

                if (_startWeaponNoise == true) //checking if we need to start the gun sound
                {
                    _audioSource.Play(); //play audio clip attached to audio source
                    _startWeaponNoise = false; //set the start weapon noise value to false to prevent calling it again
                }
            }

            else
            {
                //for loop to iterate through all muzzle flash objects
                for (int i = 0; i < _muzzleFlash.Length; i++)
                {
                    _muzzleFlash[i].SetActive(false); //enable muzzle effect particle effect
                }
                _audioSource.Stop(); //stop the sound effect from playing
                _startWeaponNoise = true; //set the start weapon noise value to true
            }
        }

        public void Target(GameObject enemy)
        {
            Vector3 direction = enemy.transform.position - _towerSource.position;

            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }

        public int Damage()
        {
            int damageAmount = 5;
            return damageAmount;
        }

        public void Health(int damage, GameObject obj)
        {
            if (obj == this.gameObject)
            {
                int health = 100;
                health -= damage;
                if (health <= 0)
                {
                    _explosion.Play();
                    health = 0;
                    Destroy(this.gameObject);
                    EventManager.Fire("onTowerDestroyed", this.transform.position);
                }
            }
        }
    }

}
