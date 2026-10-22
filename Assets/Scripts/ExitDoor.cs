using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    public string winMessage = "You escaped.";
    public string lockedMessage = "The door is locked. I need a key.";

    public NoteTextDisplay textDisplay;

    private bool hasTriggeredWin = false;
    private bool hasShownLockedMessage = false;

    private void OnTriggerEnter(Collider other)
    {
        // Ignore anything that is NOT the player
        if (!other.CompareTag("Player"))
            return;

        Debug.Log("Exit touched by Player");

        // Player HAS key
        if (GameState.Instance != null && GameState.Instance.hasExitKey)
        {
            if (hasTriggeredWin)
                return;

            hasTriggeredWin = true;

            Debug.Log(winMessage);

            FindObjectOfType<FadeManager>().FadeToBlackWin();
        }

        // Player does NOT have key
        else
        {
            if (hasShownLockedMessage)
                return;

            hasShownLockedMessage = true;

            Debug.Log(lockedMessage);

            if (textDisplay != null)
            {
                textDisplay.ShowText(lockedMessage, 3f);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            hasShownLockedMessage = false;
        }
    }
}
