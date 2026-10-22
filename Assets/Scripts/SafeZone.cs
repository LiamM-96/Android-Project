using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        SimpleEnemy enemy = other.GetComponent<SimpleEnemy>();

        if (enemy != null)
        {
            enemy.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        SimpleEnemy enemy = other.GetComponent<SimpleEnemy>();

        if (enemy != null)
        {
            enemy.enabled = true;
        }
    }
}
