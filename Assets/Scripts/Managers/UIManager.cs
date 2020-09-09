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
        }

        public void Resume()
        {
            Time.timeScale = 1;
        }

        public void Restart()
        {
            
        }

        public void SpeedControls()
        {
            if (Time.timeScale == 1)
                Time.timeScale = 1.5f;

            else if (Time.timeScale == 1.5f)
                Time.timeScale = 1f;
        }
    }
}

