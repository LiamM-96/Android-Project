using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeDetector : MonoBehaviour
{
    [Header("Flashlight Detection")]
    public Light flashlight;
    public float edgeBufferAngle = 2f;
    public FlashlightBattery flashlightBattery;

    // Update is called once per frame
    void Update()
    {
        SimpleEnemy[] enemies = FindObjectsOfType<SimpleEnemy>();

        if (flashlight == null || flashlightBattery == null || !flashlightBattery.flashlightOn)
        {

            foreach (SimpleEnemy enemy in enemies)
            {
                enemy.SetLookedAt(false);
            }

            return;
        }

        foreach (SimpleEnemy enemy in enemies)
        {
            bool insideFlashlight = IsEnemyInFlashlightCone(enemy.transform);
            enemy.SetLookedAt(insideFlashlight);
        }
    }

    bool IsEnemyInFlashlightCone(Transform enemy)
    {
        Vector3 directionToEnemy = enemy.position - transform.position;
        float distanceToEnemy = directionToEnemy.magnitude;

        if (distanceToEnemy > flashlight.range)
            return false;

        float angleToEnemy = Vector3.Angle(
                flashlight.transform.forward,
                directionToEnemy
                );

        float coneAngle = (flashlight.spotAngle * 0.5f) - edgeBufferAngle;

        return angleToEnemy < coneAngle;
    }
}
