using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisappearingMannequinEvent : MonoBehaviour
{
    public Transform mannequin;
    public Transform movedPosition;

    public float lookAtAngle = 30f;
    public float lookAwayAngle = 45f;

    private bool hasSeen = false;
    private bool triggered = false;

    public float activationDistance = 10f;

    // Update is called once per frame
    void Update()
    {
        if (triggered) return;

        float distance = Vector3.Distance(Camera.main.transform.position, mannequin.position);
        float angle = AngleToMannequin();

        // Debug.Log("Disappear distance: " + distance + " angle: " + angle + " hasSeen: " + hasSeen);

        if (distance > activationDistance)
            return;

        if (angle < lookAtAngle)
        {
            hasSeen = true;
            // Debug.Log("Disappear mannequin seen");
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

        // Debug.Log("Disappear event triggered.");

        mannequin.gameObject.SetActive(false);
    }
}
