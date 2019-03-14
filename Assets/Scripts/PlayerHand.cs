using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : Hand {

    public float distanceFromPlayer;
    public float pivotOffset;

	protected override void DetermineFinalTransform(Transform cardHolder, out Vector3 finalPos, out Quaternion finalRot)
    {
        Debug.Log("derived class call");

        float angleLowBound = 180 - 5 * (contents.Count - 1) / 2;
        float angleHighBound = 180 + 5 * (contents.Count - 1) / 2;
        float angleStep = (angleHighBound - angleLowBound) / contents.Count;

        float theta = angleLowBound;
        for (int i = 0; i < contents.Count - 1; i++)
        {
            Vector3 rotation = contents[i].transform.localEulerAngles;
            rotation.z = (theta + 180) % 360;
            contents[i].transform.localEulerAngles = rotation;

            theta += angleStep;
        }

        finalRot = Quaternion.Euler(
            cardHolder.rotation.eulerAngles.x,
            180,
            (theta + 180) % 360);

        finalPos = cardHolder.position + cardHolder.forward * pivotOffset;
    }
}
