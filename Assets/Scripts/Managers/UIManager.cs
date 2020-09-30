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
        private int _countDown = 5;
        private bool _gameStarted;
        private Transform _purchaseButton;

        public override void Init()
        {
            base.Init();
        }

        private void OnEnable()
        {
            EventManager.Listen("onWaveCount", (Action<int>)WaveCount);
            EventManager.Listen("onSuccess", LifeCount);
        }

        public void Start()
        {
            _gameStarted = false;
            _levelStatusText.text = "Epic Tower Defense";
            _lifeCount.text = "100";
            _waveCount.text = "1";
       
            _levelStatusImage = _levelStatus.GetComponent<Image>();
            _sellDisplayImage = _sellDisplay.GetComponent<Image>();
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                PlayGame();
            }
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

        public void PurchaseUpgrade(int upgradeIndex)
        {
            TowerManager.Instance.UpgradeTower();
            _upgradeDisplay[upgradeIndex].SetActive(false);
        }

        public void CancelUpgrade(int i)
        {
            _upgradeDisplay[i].SetActive(false);
            Debug.Log("UIManager :: CancelUpgrade()");
        }

        public void ChangeFunds(int amount)
        {
            _warFunds.text = amount.ToString();
        }

        public void RestartGame()
        {
            Debug.Log("UIManager :: RestartGame()");
            SceneManager.LoadScene(0);
        }

        public void PauseGame()
        {
            Debug.Log("Game Paused");
            Time.timeScale = 0;
            _levelStatusText.text = "Paused";
            _levelStatus.SetActive(true);
        }

        public void PlayGame()
        {
            Debug.Log("UIManager :: PlayGame()");

            if (Time.timeScale == 0 && _gameStarted == true)
            {
                Time.timeScale = 1;
            }

            else if (_gameStarted == false)
            {
                Debug.Log("UIManager :: PlayGame() : else if()");
                StartCoroutine(StartingGame());
                _gameStarted = true;
            }
        }

        public void FastForward()
        {
            if (Time.timeScale == 1)
            {
                Debug.Log("FastForward");
                Time.timeScale = 2;
            }
            else if (Time.timeScale == 2)
            {
                Debug.Log("Normal speed");
                Time.timeScale = 1;
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
        }

        public void WaveCount(int wave)
        {
            if (wave <= 10)
            {
                _levelStatus.SetActive(false);
                _waveCount.text = (wave / 10).ToString();
            }
            else if (wave == 11)
            {
                _waveCount.text = (10 / 10).ToString();
                _levelStatusText.text = "LEVEL COMPLETE";
                _levelStatus.SetActive(true);
            }
        }

        public IEnumerator StartingGame()
        {
            while (_countDown >= 0)
            {
                _levelStatusText.text = _countDown.ToString();
                _countDown--;
                yield return new WaitForSeconds(1);
                if (_countDown == 0)
                {
                    _levelStatus.SetActive(false);
                    EventManager.Fire("onStartingGame");
                }
            }
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
            EventManager.UnsubscribeEvent("onWaveCount", (Action<int>)WaveCount);
            EventManager.UnsubscribeEvent("onSuccess", LifeCount);
        }
    }
}

