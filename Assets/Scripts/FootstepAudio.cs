using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepAudio : MonoBehaviour
{
    public AudioSource audioSource;

    public Transform cameraTransform;
    public float lookDownThreshold = -0.1f;

    [Header("Default Footsteps")]
    public AudioClip defaultLeftFootstep;
    public AudioClip defaultRightFootstep;

    [Header("Metal Footsteps")]
    public AudioClip metalLeftFootstep;
    public AudioClip metalRightFootstep;

    public float stepInterval = 0.5f;
    public float footstepVolume = 0.5f;

    private float stepTimer = 0f;
    private bool useLeftFoot = true;
    private int metalZoneCount = 0;
    private bool onMetalSurface => metalZoneCount > 0;

    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();

        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        bool lookingDown = cameraTransform.forward.y < lookDownThreshold;
        bool moving = controller.velocity.magnitude > 0.1f && lookingDown;

        if (moving)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void PlayFootstep()
    {
        AudioClip clip;

        if(onMetalSurface)
        {
            clip = useLeftFoot ? metalLeftFootstep : metalRightFootstep;
        }
        else
        {
            clip = useLeftFoot ? defaultLeftFootstep : defaultRightFootstep;
        }

        if (clip == null) return;

        audioSource.pitch = Random.Range(0.95f, 1.05f);
        audioSource.PlayOneShot(clip, footstepVolume);

        useLeftFoot = !useLeftFoot;
    }

    public void EnterMetalSurface()
    {
        metalZoneCount++;
    }

    public void ExitMetalSurface()
    {
        metalZoneCount = Mathf.Max(0, metalZoneCount - 1);
    }
}
