using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

namespace Scripts.Managers
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField] private GameObject[] _towers;
        [SerializeField] private GameObject[] _upgradeDisplay;
        [SerializeField] private GameObject _sellDisplay;
        [SerializeField] private GameObject _levelStatus;
        [SerializeField] private GameObject _purchaseTextObject;

        [SerializeField] private Image _armoryDisplay;
        [SerializeField] private Image _warfundDisplay;
        [SerializeField] private Image _playbackSpeedDisplay;
        [SerializeField] private Image _restartDisplay;
        [SerializeField] private Image _livesAndWaveDisplay;
        private Image _levelStatusImage;
        private Image _sellDisplayImage;

        [SerializeField] private Text _warFunds;
        [SerializeField] private Text _sellAmount;
        [SerializeField] private Text _lifeCount;
        [SerializeField] private Text _waveCount;
        [SerializeField] private Text _levelStatusText;
        [SerializeField] private Text _statusIndicator;

        private int _lives = 100;
        private int _startCountDown = 5;
        private int _nextWaveCountDown = 10;
        private bool _gameStarted;
        private bool _airRaid;
        private bool _nextWave;
        private Transform _purchaseButton;
        private WaitForSeconds _countDownYield;
        private WaitForSeconds _airRaidYield;
        private WaitForSeconds _textFadeYield;


        public override void Init()
        {
            base.Init();
            _levelStatusImage = _levelStatus.GetComponent<Image>();
            _sellDisplayImage = _sellDisplay.GetComponent<Image>();
        }

        private void OnEnable()
        {
            EventManager.Listen("onSuccess", LifeCount);
            EventManager.Listen("onWaveCount", (Action<int>)WaveCount);
            EventManager.Listen("onStartNextWave", NextWaveBool);
        }

        public void Start()
        {
            _gameStarted = false;
            _levelStatusText.text = "Epic Tower Defense";
            _levelStatus.SetActive(true);
            _lifeCount.text = "100";
 
            _countDownYield = new WaitForSeconds(1);
            _airRaidYield = new WaitForSeconds(10);
            _textFadeYield = new WaitForSeconds(2);
            WaveCount(1);
        }

        public void TowerUpgradeAbility(bool enoughFunds, GameObject tower)
        {
            if (tower == _towers[0])
            {
                var colorChange = _upgradeDisplay[0].GetComponent<Image>().color;
                _purchaseButton = _upgradeDisplay[0].transform.Find("PurchaseButton");

                if (colorChange != null && _purchaseButton != null)
                {
                    if (enoughFunds == false)
                    {
                        _purchaseButton.gameObject.GetComponent<Button>().enabled = false;
                        colorChange.a = 1;
                        _upgradeDisplay[0].SetActive(true);
                        Debug.Log("Not enough WarFunds to upgrade tower");
                    }
                    else if (enoughFunds == true)
                    {
                        _purchaseButton.gameObject.GetComponent<Button>().enabled = true;
                        colorChange.a = 0;
                        _upgradeDisplay[0].SetActive(true);
                    }
                }
                
            }
            else
            {
                var colorChange = _upgradeDisplay[1].GetComponent<Image>().color;
                _purchaseButton = _upgradeDisplay[1].transform.Find("PurchaseButton");

                if (colorChange != null && _purchaseButton != null)
                {
                    if (enoughFunds == false)
                    {
                        _purchaseButton.gameObject.GetComponent<Button>().enabled = false;  // Disable the upgrade button
                        colorChange.a = 1; // Gray out the upgrade button
                        _upgradeDisplay[1].SetActive(true);
                        Debug.Log("Not enough WarFunds to upgrade tower");
                    }
                    else if (enoughFunds == true)
                    {
                        _purchaseButton.gameObject.GetComponent<Button>().enabled = true;
                        colorChange.a = 0;
                        _upgradeDisplay[1].SetActive(true);
                    }
                }
                
            }
        }

        public void SellingTower(int towerWorth)
        {
            _sellAmount.text = towerWorth.ToString();
            _sellDisplay.SetActive(true);
        }

        public void SellTower()
        {
            TowerManager.Instance.SellTower();
            _sellDisplay.SetActive(false);
        }

        public void CancelSale()
        {
            _sellDisplay.SetActive(false);
        }

        public void PurchaseUpgrade(int upgradeIndex)
        {
            TowerManager.Instance.UpgradeTower();
            _upgradeDisplay[upgradeIndex].SetActive(false);
        }

        public void CancelUpgrade(int i)
        {
            _upgradeDisplay[i].SetActive(false);
        }

        public IEnumerator NotEnoughFunds()
        {
            _purchaseTextObject.SetActive(true);
            yield return _textFadeYield;
            _purchaseTextObject.SetActive(false);
        }

        public void ChangeFunds(int amount)
        {
            _warFunds.text = amount.ToString();
        }

        public void RestartGame()
        {
            if (_gameStarted == true)
            {
                SceneManager.LoadScene(0);
                _gameStarted = false;
            }
        }

        public void PauseGame()
        {
            if (_gameStarted == true)
            {
                Time.timeScale = 0;
                _levelStatusText.text = "Paused";
                _levelStatus.SetActive(true);
            }
        }

        public void PlayGame()
        {
            if (Time.timeScale == 0 && _gameStarted == true)
            {
                Time.timeScale = 1;
                _levelStatus.SetActive(false);
            }

            else if (_gameStarted == false)
            {
                StartCoroutine(StartingGame());
                _gameStarted = true;
            }
        }

        public void FastForward()
        {
            if (_gameStarted == true)
            {
                if (Time.timeScale == 1)
                {
                    Time.timeScale = 2;
                }
                else if (Time.timeScale == 2)
                {
                    Time.timeScale = 1;
                }
            }
        }

        public void LifeCount()
        {
            _lives--;
            _lifeCount.text = _lives.ToString();

            if (_lives > 70)
            {
                _statusIndicator.text = "Good";
                ChangeStatus(Color.white);
            }
            else if (_lives <= 70 && _lives > 40)
            {
                _statusIndicator.text = "Fair";
                ChangeStatus(Color.yellow);
            }
            else if (_lives <= 40)
            {
                _statusIndicator.text = "Danger";
                ChangeStatus(Color.red);
            }

            if (_lives == 0)
            {
                _lives = 0;
                _levelStatusText.text = "Game Over";
                _levelStatus.SetActive(true);
            }
        }

        public void WaveCount(int waveNumber)
        {
            _waveCount.text = (waveNumber.ToString() + " / 10");
            
            if (waveNumber == 11)
            {
                _waveCount.text = (10.ToString() + " / 10");
                _levelStatusText.text = "LEVEL COMPLETE";
                _levelStatus.SetActive(true);
            }

            if (waveNumber == 6 && _lives > 70)
            {
                EventManager.Fire("onSendAirRaid");
                _airRaid = true;
            }
        }

        public IEnumerator StartingGame()
        {
            while (_startCountDown >= 0)
            {
                _levelStatusText.text = ("Starting in \n" + _startCountDown.ToString());
                _startCountDown--;
                yield return _countDownYield;
                if (_startCountDown == 0)
                {
                    _levelStatus.SetActive(false);
                    EventManager.Fire("onStartingGame");
                }
            }
        }

        public IEnumerator NextWave()
        {
            if (_nextWave == true)
            {
                while (_nextWaveCountDown > 0)
                {
                    _levelStatusText.text = ("Next Wave in \n" + _nextWaveCountDown.ToString());
                    _levelStatus.SetActive(true);
                    _nextWaveCountDown--;
                    yield return _countDownYield;
                    if (_nextWaveCountDown == 0)
                    {
                        _levelStatus.SetActive(false);
                        if (_airRaid == true)
                        {
                            yield return _airRaidYield;
                            _airRaid = false;
                        }
                        //SpawnManager.Instance.StartWave();
                    }
                }
                _nextWave = false;
            }
        }

        public void NextWaveBool()
        {
            _nextWave = true;
        }

        public void ChangeStatus(Color color)
        {
            Color newColor = color;
            for (int i = 0; i < _upgradeDisplay.Length; i++)
            {
                _upgradeDisplay[i].GetComponent<Image>().color = newColor;
            }
            _sellDisplayImage.color = newColor;
            _levelStatusImage.color = newColor;
            _armoryDisplay.color = newColor;
            _warfundDisplay.color = newColor;
            _playbackSpeedDisplay.color = newColor;
            _restartDisplay.color = newColor;
            _livesAndWaveDisplay.color = newColor;
        }

        private void OnDisable()
        {
            EventManager.UnsubscribeEvent("onSuccess", LifeCount);
            EventManager.UnsubscribeEvent("onWaveCount", (Action<int>)WaveCount);
            EventManager.UnsubscribeEvent("onStartNextWave", NextWaveBool);
        }
    }
}

