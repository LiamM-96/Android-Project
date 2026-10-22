using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroEvent : MonoBehaviour
{
    public Transform mannequin;
    public GameObject heldGlass;
    public GameObject brokenGlass;

    public Transform movedPosition;

    public AudioSource glassSound;

    private float lookAtAngle = 15f;
    private float lookAwayAngle = 45f;

    private bool hasSeen = false;
    private bool triggered = false;

    public float activationDistance = 10f;

    // Update is called once per frame
    void Update()
    {
        if (triggered) return;

        float distance = Vector3.Distance(Camera.main.transform.position, mannequin.position);

        if (distance > activationDistance)
            return;

        float angle = AngleToMannequin();

        if (angle < lookAtAngle)
        {
            hasSeen = true;
        }

        if (hasSeen && angle > lookAwayAngle)
        {
            TriggerEvent();
        }
    }

    float AngleToMannequin()
    {
        Vector3 directionToMannequin = mannequin.position - Camera.main.transform.position;
        return Vector3.Angle(Camera.main.transform.forward, directionToMannequin);
    }

    void TriggerEvent()
    {
        triggered = true;

        // Move mannequin
        mannequin.position = movedPosition.position;
        mannequin.rotation = movedPosition.rotation;

        // Swap glass
        heldGlass.SetActive(false);
        brokenGlass.SetActive(true);

        // Play sound
        if (glassSound != null)
            StartCoroutine(PlayPartialSound(1f)); // play first second
    }

    IEnumerator PlayPartialSound(float duration)
    {
        glassSound.Play();
        yield return new WaitForSeconds(duration);
        glassSound.Stop();
    }
}
