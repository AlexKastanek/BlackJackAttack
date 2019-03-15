using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * PlayerHand class is inherited from the Hand
 * class. The only difference between this and 
 * the base class is the DetermineFinalTransform,
 * which is a bit more complex than on the dealer's
 * hand due to how it must adjust according to
 * each additional card.
 */
public class PlayerHand : Hand {

    public float cardSeparationAmt;
    public float pivotOffset;

    /**
     * Overridden function for DetermineFinalTransform().
     * The final transform for the card is calculated
     * by setting a low bound and high bound angle for the
     * set of cards. A step is linearly interpolated 
     * between these two angles, depending on the size of
     * the hand. The function then adjust all cards in the
     * hand every time a new card is added.
     */
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
