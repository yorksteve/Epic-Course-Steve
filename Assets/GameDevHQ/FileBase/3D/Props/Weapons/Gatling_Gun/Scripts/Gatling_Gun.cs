using Scripts.Interfaces;
using Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        [SerializeField] private Transform _gunBarrel; //Reference to hold the gun barrel
        public GameObject Muzzle_Flash; //reference to the muzzle flash effect to play when firing
        public ParticleSystem bulletCasings; //reference to the bullet casing effect to play when firing
        public AudioClip fireSound; //Reference to the audio clip

        private AudioSource _audioSource; //reference to the audio source component
        private bool _startWeaponNoise = true;

        [SerializeField] private int _warFundsRequired = 200;
        [SerializeField] private GameObject _upgradeModel;
        [SerializeField] private ParticleSystem _explosion;

        public int WarFundsRequired { get => _warFundsRequired; set => _warFundsRequired = value; }
        public GameObject CurrentModel { get; set; }

        public GameObject UpgradeModel => _upgradeModel;

        [SerializeField] private GameObject _towerBase;
        private Transform _towerSource;

        private ParticleSystem _muzzleFlash;
        private float _damageAmount = .02f;
        private float _maxHealth = 100;

        // Use this for initialization
        void Start()
        {
            CurrentModel = this.gameObject;
            _towerSource = _towerBase.GetComponent<Transform>();

            //_gunBarrel = GameObject.Find("Barrel_to_Spin").GetComponent<Transform>(); //assigning the transform of the gun barrel to the variable
            Muzzle_Flash.SetActive(false); //setting the initial state of the muzzle flash effect to off
            _audioSource = GetComponent<AudioSource>(); //ssign the Audio Source to the reference variable
            _audioSource.playOnAwake = false; //disabling play on awake
            _audioSource.loop = true; //making sure our sound effect loops
            _audioSource.clip = fireSound; //assign the clip to play

            _muzzleFlash = Muzzle_Flash.GetComponent<ParticleSystem>();
        }

        // Method to rotate gun barrel 
        void RotateBarrel() 
        {
            _gunBarrel.transform.Rotate(Vector3.forward * Time.deltaTime * -500.0f); //rotate the gun barrel along the "forward" (z) axis at 500 meters per second

        }

        public void Health(float damage, GameObject obj)
        {
            if (obj == this.gameObject)
            {
                float currentHealth = _maxHealth;
                currentHealth -= damage;
                EventManager.Fire("onHealthBar", currentHealth, obj);
                if (currentHealth <= 0)
                {
                    _explosion.Play();
                    currentHealth = 0f;
                    Destroy(this.gameObject);
                    EventManager.Fire("onTowerDestroyed", this.transform.position);
                    EventManager.Fire("onResetHealthTower", 100, this.gameObject);
                }
            }
        }

        public void Attack(bool attack)
        {
            if (attack == true)
            {
                RotateBarrel(); //Call the rotation function responsible for rotating our gun barrel
                Muzzle_Flash.SetActive(true); //enable muzzle effect particle effect
                _muzzleFlash.Play();
                bulletCasings.Emit(1); //Emit the bullet casing particle effect  

                Damage();

                if (_startWeaponNoise == true) //checking if we need to start the gun sound
                {
                    _audioSource.Play(); //play audio clip attached to audio source
                    _startWeaponNoise = false; //set the start weapon noise value to false to prevent calling it again
                }

            }
            else
            {
                Muzzle_Flash.SetActive(false); //turn off muzzle flash particle effect
                _muzzleFlash.Stop();
                _audioSource.Stop(); //stop the sound effect from playing
                _startWeaponNoise = true; //set the start weapon noise value to true
            }
        }

        public void Target(GameObject enemy)
        {
            if (enemy != null)
            {
                Vector3 direction = enemy.transform.position - _towerSource.position;
                _towerSource.rotation = Quaternion.LookRotation(direction, Vector3.up);
                EventManager.Fire("onTargetedMech", enemy);
            }
        }

        public float Damage()
        {
            EventManager.Fire("onDamage", _damageAmount);
            return _damageAmount;
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
