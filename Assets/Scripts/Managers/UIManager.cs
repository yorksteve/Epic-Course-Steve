using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YorkSDK.Util;

namespace Scripts.Managers
{
    public class UIManager : MonoSingleton<UIManager>
    {
        public override void Init()
        {
            base.Init();
        }

        public void Pause()
        {
            Time.timeScale = 0;
            Debug.Log("Game is paused");
        }

        public void Resume()
        {
            Time.timeScale = 1;
            Debug.Log("Game has been resumed");
        }

        public void Restart()
        {
            SpawnManager.Instance.RestartGame();
            Debug.Log("Game restarted");
        }

        public void SpeedControls()
        {
            if (Time.timeScale == 1)
            {
                Time.timeScale = 1.5f;
                Debug.Log("Increased speed");
            }

            else if (Time.timeScale == 1.5f)
            {
                Time.timeScale = 1f;
                Debug.Log("Decreased speed");
            }
        }
    }
}

