using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.Manager;

public class EndZone : MonoBehaviour
{
    private int _mechsTriggered;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Destination Reached");
        _mechsTriggered++;
        other.gameObject.SetActive(false);
        // Reduce player's life count

        if (_mechsTriggered == SpawnManager.Instance.mechsInWave)
        {
            // Add functionality for destroyed mechs

            StartCoroutine(NextWave());
        }
    
    }

    IEnumerator NextWave()
    {
        Debug.Log("NextWave()");
        yield return new WaitForSeconds(10);
        SpawnManager.Instance.StartWave();
    }
}
