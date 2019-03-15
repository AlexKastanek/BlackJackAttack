using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : Hand {

    public float cardSeparationAmt;
    public float pivotOffset;

    public void AdjustTransforms()
    {
        
    }

	protected override void DetermineFinalTransform(ref Transform finalTransform)
    {
        Debug.Log("derived class call");

        Transform card = finalTransform;
        Transform cardHolder = card.parent;

        float angleLowBound = 180 + 30; /** (contents.Count - 1) / 2;*/
        float angleHighBound = 180 - 30; /** (contents.Count - 1) / 2;*/
        float angleStep = (angleHighBound - angleLowBound) / (contents.Count + 1);
        float startingAngle = angleLowBound + angleStep;

        /*
        Vector3 cardHolderPos = cardHolder.transform.parent.position;
        cardHolderPos += cardHolder.transform.forward * pivotOffset;
        cardHolder.transform.position = cardHolderPos;
        */

        // adjust each card holder's local rotation and position
        Vector3 localRotationEuler;
        Vector3 cardPosition;
        //Vector3 localPosition;
        float theta = startingAngle;
        for (int i = 0; i < contents.Count; i++)
        {
            //while (contents[i].GetComponent<Card>().isInterpolating) { };
            contents[i].transform.parent.SetPositionAndRotation(
                contents[i].transform.parent.parent.position,
                contents[i].transform.parent.parent.rotation);

            localRotationEuler = contents[i].transform.parent.localEulerAngles;
            localRotationEuler.z = (theta + 180);
            localRotationEuler.x += cardSeparationAmt * i;
            contents[i].transform.parent.localEulerAngles = localRotationEuler;

            //contents[i].transform.position = contents[i].transform.parent.position + contents[i].transform.up * pivotOffset;

            /*
            //localPosition = contents[i].transform.parent.localPosition;
            localPosition = Vector3.zero;
            localPosition.z = -cardSeparationAmt * i;
            localPosition.y = cardSeparationAmt * i;
            contents[i].transform.parent.localPosition = localPosition;
            */

            theta += angleStep;
        }

        // set the card rotation
        Vector3 rotationEuler = card.transform.eulerAngles;
        Quaternion rotation = Quaternion.Euler(
            -20,                // tilt the pitch of the card
            180,                // flip the card over on the y-axis
            0);                 // roll stays the same

        // set the card position
        Vector3 position = cardHolder.position + card.up * pivotOffset/*+ cardHolder.forward * pivotOffset*/;

        finalTransform.SetPositionAndRotation(position, rotation);
    }
}
