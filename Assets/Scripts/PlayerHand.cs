using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : Hand {

    public float cardSeparationAmt;
    public float pivotOffset;

	protected override void DetermineFinalTransform(ref Transform finalTransform)
    {
        Transform card = finalTransform;
        Transform cardHolder = card.parent;

        // Hand of cards works by setting a low bound and high bound angle.
        // The rotation of the cards is then clamped between the two bounds.
        float angleLowBound = 180 + 30;
        float angleHighBound = 180 - 30;
        float angleStep = (angleHighBound - angleLowBound) / (contents.Count + 1);
        float startingAngle = angleLowBound + angleStep;

        // adjust each card holder's local rotation and position
        Vector3 localRotationEuler;
        float theta = startingAngle;
        for (int i = 0; i < contents.Count; i++)
        {
            contents[i].transform.parent.SetPositionAndRotation(
                contents[i].transform.parent.parent.position,
                contents[i].transform.parent.parent.rotation);

            localRotationEuler = contents[i].transform.parent.localEulerAngles;
            localRotationEuler.z = (theta + 180);
            localRotationEuler.x += cardSeparationAmt * i;
            contents[i].transform.parent.localEulerAngles = localRotationEuler;

            theta += angleStep;
        }

        // set the card rotation
        Vector3 rotationEuler = card.transform.eulerAngles;
        Quaternion rotation = Quaternion.Euler(
            -20,                // tilt the pitch of the card
            180,                // flip the card over on the y-axis
            0);                 // roll stays the same

        // set the card position
        Vector3 position = cardHolder.position + card.up * pivotOffset;

        finalTransform.SetPositionAndRotation(position, rotation);
    }
}
