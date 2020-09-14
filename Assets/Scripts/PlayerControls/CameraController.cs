using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Scripts.PlayerControls
{
    public class CameraController : MonoBehaviour
    {
        private float _width = Screen.width;
        private float _height = Screen.height;
        //private float _moveWidth;
        //private float _moveHeight;

        private float _xRestrict;
        private float _yRestrict;

        [SerializeField] private int _cameraSpeed = 5;
        [SerializeField] private int _zoomSpeed = 60;

        [SerializeField] private int _xMin = -50;
        [SerializeField] private int _xMax = -30;
        [SerializeField] private int _yMin = 10;
        [SerializeField] private int _yMax = 30;
        [SerializeField] private int _zMin = -10;
        [SerializeField] private int _zMax = 10;

        private void Start()
        {
            //_moveWidth = _width - 60;
            //_moveHeight = _height - 60;

            _xRestrict = transform.position.x;
            _yRestrict = transform.position.y;
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            var clampPos = transform.position;
            clampPos.x = Mathf.Clamp(clampPos.x, _xMin, _xMax);
            clampPos.y = Mathf.Clamp(clampPos.y, _yMin, _yMax);
            clampPos.z = Mathf.Clamp(clampPos.z, _zMin, _zMax);
            transform.position = clampPos;

            // Using mousewheel to zoom in and out
            if (Input.mouseScrollDelta.y > 0)
            {
                transform.Translate(Vector3.forward * Time.deltaTime * _zoomSpeed);
            }

            if (Input.mouseScrollDelta.y < 0)
            {
                transform.Translate(Vector3.back * Time.deltaTime *_zoomSpeed);
            }

            // Using mouse to move camera
            if (Input.mousePosition.y > _height || Input.GetKey(KeyCode.W))
            {
                transform.Translate(new Vector3(0, 1, 1) * Time.deltaTime * _cameraSpeed);
            }
            else if (Input.mousePosition.y < 0 || Input.GetKey(KeyCode.S))
            {
                transform.Translate(new Vector3(0, -1, -1) * Time.deltaTime * _cameraSpeed);
            }

            if (Input.mousePosition.x < 0)
            {
                transform.Translate(Vector3.left * Time.deltaTime * _cameraSpeed);
            }

            else if (Input.mousePosition.x > _width)
            {
                transform.Translate(Vector3.right * Time.deltaTime * _cameraSpeed);
            }



            // Using WSAD keys to move
            float xTrans = Input.GetAxis("Horizontal") * Time.deltaTime * _cameraSpeed;
            transform.Translate(xTrans, 0, 0);
        }
    }
}

