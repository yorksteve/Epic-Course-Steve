using Scripts.Managers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Extra
{
    public class AirRaidController : MonoBehaviour
    {
        private GameObject _ship;
        [SerializeField] private ParticleSystem[] _explosionArray;
        private bool _airRaid;
        private float _speed = -5f;

        private WaitForSeconds _bombingYield;
        private WaitForSeconds _explosionYield;

        private void OnEnable()
        {
            EventManager.Listen("onSendAirRaid", AirRaid);
        }

        void Start()
        {
            _ship = this.gameObject;
            _bombingYield = new WaitForSeconds(3);
            _explosionYield = new WaitForSeconds(.02f);
        }

        void Update()
        {
            if (_airRaid == false)
                return;

            if (_airRaid == true)
            {
                transform.Translate(Time.deltaTime * _speed, 0, 0);
                StartCoroutine(AirRaidRoutine());
            }
        }

        private void AirRaid()
        {
            _airRaid = true;
        }

        IEnumerator AirRaidRoutine()
        {
            yield return _bombingYield;
            for (int i = 0; i < _explosionArray.Length; i++)
            {
                yield return _explosionYield;
                _explosionArray[i].Play();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("RaidDestroyer"))
            {
                Destroy(_ship);
                Destroy(other.gameObject);
            }
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onSendAirRaid", AirRaid);
        }
    }
}


