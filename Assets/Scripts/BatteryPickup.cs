using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickup : MonoBehaviour
{
    public float batteryAmount = 40f;

    [Header("Audio")]
    public AudioClip pickupsound;

    private void OnTriggerEnter(Collider other)
    {
        FlashlightBattery battery = other.GetComponentInParent<FlashlightBattery>();

        if (battery == null)
            battery = other.GetComponentInChildren<FlashlightBattery>();

        if (battery != null)
        {
            battery.AddBattery(batteryAmount);

            // Play sound at pickup position
            if(pickupsound != null)
            {
                AudioSource.PlayClipAtPoint(pickupsound, transform.position, 0.1f);
            }

            Destroy(gameObject);
        }
    }
}
