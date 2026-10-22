using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMotion2 : MonoBehaviour
{
    public CharacterController characterController;
    public Transform cameraTransform;

    public float speed = 1.5f;
    public float lookDownThreshold = -0.2f;

    // Start is called before the first frame update
    void Start()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        if (characterController == null)
            cameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (characterController == null || cameraTransform == null)
            return;

        bool lookingDown = cameraTransform.forward.y < lookDownThreshold;

        if (lookingDown)
        {
            Vector3 moveDirection = cameraTransform.forward;
            moveDirection.y = 0f;
            moveDirection.Normalize();

            characterController.SimpleMove(moveDirection * speed);
        }
    }
}
