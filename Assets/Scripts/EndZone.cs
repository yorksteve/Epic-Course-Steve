using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Manager;

public class EndZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Destination Reached");
        other.gameObject.SetActive(false);
        // Reduce player's life count

        if (other.gameObject) //Check number of mechs that go through in addition to destroyed ones for starting next wave
        {
            SpawnManager.Instance.StartWave();
        }
    }
}
