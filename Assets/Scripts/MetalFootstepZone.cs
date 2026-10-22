using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalFootstepZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        FootstepAudio footsteps = other.GetComponent<FootstepAudio>();

        if (footsteps != null)
        {
            footsteps.EnterMetalSurface();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        FootstepAudio footsteps = other.GetComponent<FootstepAudio>();

        if (footsteps != null)
        {
            footsteps.ExitMetalSurface();
        }
    }
}
