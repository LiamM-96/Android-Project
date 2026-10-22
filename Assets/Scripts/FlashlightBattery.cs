using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightBattery : MonoBehaviour
{
    public Light flashlight;

    [Header("Battery")]
    public float maxBattery = 100f;
    public float currentBattery = 100f;
    public float drainRate = 12f;
    public float rechargeRate = 8f;

    [Header("Controls")]
    public KeyCode toggleKey = KeyCode.F;
    public bool flashlightOn = true;

    [Header("Low Battery Flicker")]
    public float lowBatteryPercent = 0.2f;
    public float normalIntensity = 30f;
    public float flickerMinIntensity = 8f;
    public float flickerMaxIntensity = 20f;
    public float flickerSpeed = 20f;

    [Header("HUD")]
    public Image batteryFill;

    private float flickerTimer;

    [Header("Toggle Feedback")]
    public AudioSource audioSource;
    public AudioClip toggleSound;
    public float toggleCooldown = 0.3f;

    private float lastToggleTime = -999f;

    // Start is called before the first frame update
    void Start()
    {
        if (flashlight == null)
            flashlight = GetComponent<Light>();

        normalIntensity = flashlight.intensity;

        UpdateFlashlight();
        UpdateHUD();
    }

    // Update is called once per frame
    void Update()
    {
        if (
            Time.time > lastToggleTime + toggleCooldown &&
                (
                Input.GetKeyDown(toggleKey) || 
                Input.GetMouseButtonDown(0) || 
                (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                )
            )
        {
            ToggleFlashlight();
            lastToggleTime = Time.time;
        }

        if (flashlightOn && currentBattery > 0)
        {
            currentBattery -= drainRate * Time.deltaTime;

            if (currentBattery <= 0)
            {
                currentBattery = 0;
                flashlightOn = false;
            }
        }
        else
        {
            currentBattery += rechargeRate * Time.deltaTime;
            currentBattery = Mathf.Clamp(currentBattery, 0, maxBattery);
        }

        UpdateFlashlight();
        UpdateHUD();
    }

    void UpdateFlashlight()
    {
        if (flashlight != null)
            flashlight.enabled = flashlightOn && currentBattery > 0;

        if (!flashlight.enabled) return;

        float batteryPercent = currentBattery / maxBattery;

        if (batteryPercent <= lowBatteryPercent)
        {
            flickerTimer += Time.deltaTime * flickerSpeed;

            if (flickerTimer >= 1f)
            {
                flashlight.intensity = Random.Range(flickerMinIntensity, flickerMaxIntensity);
                flickerTimer = 0f;
            }
        }
        else
        {
            flashlight.intensity = normalIntensity;
        }
    }

    void UpdateHUD()
    {
        if (batteryFill == null) return;

        float batteryPercent = currentBattery / maxBattery;
        batteryFill.fillAmount = batteryPercent;

        if (batteryPercent <= 0.2f)
            batteryFill.color = Color.red;
        else if (batteryPercent <= 0.5f)
            batteryFill.color = Color.yellow;
        else
            batteryFill.color = Color.green;
    }

    public void AddBattery(float amount)
    {
        currentBattery += amount;
        currentBattery = Mathf.Clamp(currentBattery, 0, maxBattery);

        UpdateHUD();
    }

    public void ToggleFlashlight()
    {
        flashlightOn = !flashlightOn;

        if (audioSource != null && toggleSound != null)
        {
            StartCoroutine(PlayToggleSound());
        }

        if (flashlight != null && flashlightOn && currentBattery > 0)
        {
            flashlight.intensity = normalIntensity * 1.3f;
        }
    }

    IEnumerator PlayToggleSound()
    {
        if (audioSource == null || toggleSound == null)
            yield break;

        audioSource.clip = toggleSound;
        audioSource.Play();

        yield return new WaitForSeconds(1f); // play for only 1 second

        audioSource.Stop();
    }
}
