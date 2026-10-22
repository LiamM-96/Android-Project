using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class FadeManager : MonoBehaviour
{
    public Image fadeImage;

    public TMP_Text deathText;
    public TMP_Text winText;

    public AudioSource caughtSound;

    public MonoBehaviour playerMovementScript;

    public float fadeDuration = 1.5f;
    public float delayBeforeMenu = 2f;

    private bool isFading = false;

    void Start()
    {
        SetAlpha(0f);

        if (deathText != null)
        {
            deathText.gameObject.SetActive(false);
        }

        if (winText != null)
        {
            winText.gameObject.SetActive(false);
        }
    }

    public void FadeToBlackAndRestart()
    {
        if (!isFading)
            StartCoroutine(FadeDeathToMenu());
    }

    public void FadeToBlackWin()
    {
        if (!isFading)
            StartCoroutine(FadeWin());
    }

    IEnumerator FadeDeathToMenu()
    {
        isFading = true;

        StopPlayerAndEnemies();

        if (caughtSound != null)
            caughtSound.Play();

        if (deathText != null)
        {
            deathText.text = "The mannequins caught up to you!";
            deathText.gameObject.SetActive(true);
        }

        yield return FadeToBlack();

        yield return new WaitForSeconds(delayBeforeMenu);

        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator FadeWin()
    {
        isFading = true;

        StopPlayerAndEnemies();

        if (winText != null)
        {
            winText.text = "You escaped the warehouse...";
            winText.gameObject.SetActive(true);
        }

        yield return FadeToBlack();

        yield return new WaitForSeconds(delayBeforeMenu);

        SceneManager.LoadScene("MainMenu");
    }

    IEnumerator FadeToBlack()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;

            float alpha = timer / fadeDuration;

            SetAlpha(alpha);

            yield return null;
        }

        SetAlpha(1f);
    }

    void SetAlpha(float alpha)
    {
        if (fadeImage == null)
            return;

        Color c = fadeImage.color;
        c.a = alpha;
        fadeImage.color = c;
    }

    void StopPlayerAndEnemies()
    {
        if (playerMovementScript != null)
            playerMovementScript.enabled = false;

        SimpleEnemy[] enemies = FindObjectsOfType<SimpleEnemy>();

        foreach (SimpleEnemy enemy in enemies)
        {
            enemy.enabled = false;
        }
    }
}