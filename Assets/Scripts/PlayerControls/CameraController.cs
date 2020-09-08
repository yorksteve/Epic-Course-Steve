using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.PlayerControls
{
    public class CameraController : MonoBehaviour
    {
        private float _width = Screen.width;
        private float _height = Screen.height;
        private float _moveWidth;
        private float _moveHeight;

        [SerializeField] private int _cameraSpeed = 5;
        [SerializeField] private int _zoomSpeed = 40;

        private void Start()
        {
            _moveWidth = _width - 60;
            _moveHeight = _height - 60;
        }

        // Update is called once per frame
        void Update()
        {
            // Using mousewheel to zoom in and out
            if (Input.mouseScrollDelta.y > 0)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * _zoomSpeed);
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                transform.Translate(Vector3.back * Time.deltaTime *_zoomSpeed);
            }

            // Using WSAD keys to move
            float xTrans = Input.GetAxis("Horizontal") * Time.deltaTime * _cameraSpeed;
            float yTrans = Input.GetAxis("Vertical") * Time.deltaTime * _cameraSpeed; //work on y, has to be angled

            transform.Translate(xTrans, yTrans, 0);
        }
    }
}

