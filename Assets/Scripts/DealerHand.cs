using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealerHand : Hand {

    protected override void DetermineFinalTransform(ref Transform finalTransform)
    {
        Vector3 finalPosition;
        Quaternion finalRotation;

        // check if hand is empty
        if (contents.Count <= 1)
        {
            // this card is the hole card

            // put it to the left
            finalPosition = finalTransform.parent.position - finalTransform.right * 0.5f;

            // keep back side facing up
            finalRotation = Quaternion.Euler(new Vector3(90f, 0f, 0f));
        }
        else
        {
            // this is not the hole card

            // put it to the right and stack on top
            finalPosition = finalTransform.parent.position + finalTransform.right * 0.5f + finalTransform.up * contents.Count * 0.01f;

            // flip over to show face
            finalRotation = Quaternion.Euler(new Vector3(-90f, 0f, 0f));
        }

        finalTransform.SetPositionAndRotation(
            finalPosition, 
            finalRotation);
    }
}
