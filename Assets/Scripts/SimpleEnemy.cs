using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 0.5f;
    public float retreatSpeed = 1.5f;
    public float stareTimeToRepel = 2f;

    [Header("Activation")]
    public bool isActivated = false;
    public float deactivateDistance = 18f;

    [Header("References")]
    public Transform player;

    [Header("Footstep Audio")]
    public AudioSource audioSource;
    public AudioClip step1;
    public AudioClip step2;
    public float stepInterval = 0.8f;

    private float stepTimer = 0f;
    private bool useFirstStep = true;

    private bool isFrozen = false;
    private float stareTimer = 0f;
    private bool isRetreating = false;

    private Vector3 startPosition;
    private Quaternion startRotation;

    [Header("Catch")]
    public float catchDistance = 1.2f;
    private bool hasCaughtPlayer = false;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        if (!hasCaughtPlayer && Vector3.Distance(transform.position, player.position) <= catchDistance)
        {
            hasCaughtPlayer = true;
            Debug.Log("Caught by mannequin");
            FindObjectOfType<FadeManager>().FadeToBlackAndRestart();
        }

        if (isActivated && Vector3.Distance(transform.position, player.position) > deactivateDistance)
        {
            Deactivate();
            return;
        }

        if (!isActivated)
            return;

        bool isMoving = false;
        float currentSpeed = moveSpeed;

        if (isRetreating)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                startPosition,
                retreatSpeed * Time.deltaTime
                );

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                startRotation,
                retreatSpeed * 80f * Time.deltaTime
                );

            isMoving = true;
            currentSpeed = retreatSpeed;

            if (Vector3.Distance(transform.position, startPosition) < 0.05f)
            {
                transform.position = startPosition;
                transform.rotation = startRotation;
                isRetreating = false;
            }
        }

        else if (!isFrozen)
        {
            MoveTowardPlayer();
            isMoving = true;
            currentSpeed = moveSpeed;
        }

        HandleFootsteps(isMoving, currentSpeed);
    }

    public void SetLookedAt(bool lookedAt)
    {
        if (lookedAt && !isActivated)
        {
            Activate();
        }
        Freeze(lookedAt);
    }

    void Activate()
    {
        isActivated = true;
    }

    void Deactivate()
    {
        isActivated = false;
        isRetreating = false;
        isFrozen = false;
        stareTimer = 0f;
        stepTimer = 0f;

        if (audioSource != null)
            audioSource.Stop();
    }

    public void Freeze(bool state)
    {
        isFrozen = state;

        if (!isActivated)
            return;

        if (isFrozen)
        {
            stareTimer += Time.deltaTime;

            if (stareTimer >= stareTimeToRepel)
            {
                Repel();
            }
        }
        else
        {
            stareTimer = 0f;
        }
    }

    void MoveTowardPlayer()
    {
        Vector3 targetPosition = player.position;
        targetPosition.y = transform.position.y;

        Vector3 direction = (targetPosition - transform.position).normalized;

        FacePlayer();

        // Check if safe zone is directly ahead
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, 2f))
        {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("SafeZone"))
            {
                isRetreating = true;
                return;
            }
        }

        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void Repel()
    {
        stareTimer = 0f;

        if (Vector3.Distance(transform.position, startPosition) > 0.1f)
        {
            isRetreating = true;
        }
    }

    void HandleFootsteps(bool isMoving, float speed)
    {
        if (!isMoving)
        {
            stepTimer = 0f;
            return;
        }

        // Faster movement, faster steps
        float adjustedInterval = stepInterval / (speed * 1.5f);

        stepTimer += Time.deltaTime;

        if (stepTimer >= adjustedInterval)
        {
            PlayStep();
            stepTimer = 0f;
        }
    }

    void PlayStep()
    {
        if (audioSource == null) return;

        AudioClip clipToPlay = useFirstStep ? step1 : step2;

        if (clipToPlay != null)
            audioSource.PlayOneShot(clipToPlay);

        useFirstStep = !useFirstStep;
    }

    void FacePlayer()
    {
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0f;

        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}
