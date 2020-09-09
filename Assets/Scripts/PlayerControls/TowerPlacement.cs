using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.PlayerControls
{
    public class TowerPlacement : MonoBehaviour
    {
        [SerializeField] private GameObject _decoyTower;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Ray rayOrigin = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(rayOrigin, out hitInfo))
            {
                _decoyTower.transform.position = hitInfo.point;

                //if valid spot
                //Instantiate(Tower, hitInfo.point, Quaternion.identity);
            }
        }
    }
}

