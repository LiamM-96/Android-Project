using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NoteTextDisplay : MonoBehaviour
{
    public TMP_Text textBox;
    private Coroutine currentRoutine;

    public void ShowText(string message, float duration)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);

        currentRoutine = StartCoroutine(ShowRoutine(message, duration));
    }

    IEnumerator ShowRoutine(string message, float duration)
    {
        textBox.text = message;
        textBox.enabled = true;

        yield return new WaitForSeconds(duration);

        textBox.enabled = false;
    }
}
