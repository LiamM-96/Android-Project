using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public AudioClip pickupSound;
    public float volume = 0.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (GameState.Instance != null)
        {
            GameState.Instance.hasExitKey = true;
            Debug.Log("Picked up exit key.");
        }

            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position, volume);

            Destroy(gameObject);
        }
    }
