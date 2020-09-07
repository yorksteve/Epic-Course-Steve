using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.PlayerControls
{
    public class CameraController : MonoBehaviour
    {
        private float _width = Screen.width;
        [SerializeField] private int _speed = 5;

        // Update is called once per frame
        void Update()
        {
            KeyMovements();

        }

        private void KeyMovements()
        {
            if (Input.GetKey(KeyCode.W))
            {

            }

            if (Input.GetKey(KeyCode.A))
            {
                transform.Translate(Vector3.left * Time.deltaTime * _speed);
            }

            if (Input.GetKey(KeyCode.S))
            {

            }

            if (Input.GetKey(KeyCode.D))
            {
                transform.Translate(Vector3.right * Time.deltaTime * _speed);
            }
        }
    }
}

