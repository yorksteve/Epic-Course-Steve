﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Interfaces;


namespace GameDevHQ.FileBase.Gatling_Gun
{
    /// <summary>
    /// This script will allow you to view the presentation of the Turret and use it within your project.
    /// Please feel free to extend this script however you'd like. To access this script from another script
    /// (Script Communication using GetComponent) -- You must include the namespace (using statements) at the top. 
    /// "using GameDevHQ.FileBase.Gatling_Gun" without the quotes. 
    /// 
    /// For more, visit GameDevHQ.com
    /// 
    /// @authors
    /// Al Heck
    /// Jonathan Weinberger
    /// </summary>

    [RequireComponent(typeof(AudioSource))] //Require Audio Source component
    public class Gatling_Gun : MonoBehaviour, ITower, IAttack
    {
        private Transform _gunBarrel; //Reference to hold the gun barrel
        public GameObject Muzzle_Flash; //reference to the muzzle flash effect to play when firing
        public ParticleSystem bulletCasings; //reference to the bullet casing effect to play when firing
        public AudioClip fireSound; //Reference to the audio clip

        private AudioSource _audioSource; //reference to the audio source component
        private bool _startWeaponNoise = true;

        [SerializeField] private int _warFundsRequired;
        [SerializeField] private GameObject _upgradeModel;
        public int WarFundsRequired { get => _warFundsRequired; set => _warFundsRequired = value; }
        public GameObject CurrentModel { get; set; }
        public GameObject UpgradeModel { get => _upgradeModel; }

        [SerializeField] private GameObject _towerBase;
        private Transform _towerSource;

        // Use this for initialization
        void Start()
        {
            CurrentModel = this.gameObject;
            _gunBarrel = GameObject.Find("Barrel_to_Spin").GetComponent<Transform>(); //assigning the transform of the gun barrel to the variable
            Muzzle_Flash.SetActive(false); //setting the initial state of the muzzle flash effect to off
            _audioSource = GetComponent<AudioSource>(); //ssign the Audio Source to the reference variable
            _audioSource.playOnAwake = false; //disabling play on awake
            _audioSource.loop = true; //making sure our sound effect loops
            _audioSource.clip = fireSound; //assign the clip to play

            _towerSource = _towerBase.GetComponent<Transform>();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnTriggerEnter(Collider other)
        {
            // Add to the queue
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Enemy"))
            {
                Target(other.gameObject);
                Attack();
            }
            
        }

        private void OnTriggerExit(Collider other)
        {
            Muzzle_Flash.SetActive(false); //turn off muzzle flash particle effect
            _audioSource.Stop(); //stop the sound effect from playing
            _startWeaponNoise = true; //set the start weapon noise value to true

            // Remove from queue
        }

        // Method to rotate gun barrel 
        void RotateBarrel() 
        {
            _gunBarrel.transform.Rotate(Vector3.forward * Time.deltaTime * -500.0f); //rotate the gun barrel along the "forward" (z) axis at 500 meters per second
        }

        public void Attack()
        {
            RotateBarrel(); //Call the rotation function responsible for rotating our gun barrel
            Muzzle_Flash.SetActive(true); //enable muzzle effect particle effect
            bulletCasings.Emit(1); //Emit the bullet casing particle effect  

            if (_startWeaponNoise == true) //checking if we need to start the gun sound
            {
                _audioSource.Play(); //play audio clip attached to audio source
                _startWeaponNoise = false; //set the start weapon noise value to false to prevent calling it again
            }
        }

        public int Damage()
        {
            int damageAmount = 2;
            return damageAmount;
        }

        public void Target(GameObject enemy)
        {
            Vector3 direction = enemy.transform.position - _towerSource.position;
            transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
        }
    }

}
