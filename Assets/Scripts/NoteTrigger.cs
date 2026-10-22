using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteTrigger : MonoBehaviour
{
    [TextArea]
    public string message;

    public float displayTime = 4f;
    public NoteTextDisplay textDisplay;

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return;

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;

            if (textDisplay != null)
                textDisplay.ShowText(message, displayTime);
        }
    }
}
