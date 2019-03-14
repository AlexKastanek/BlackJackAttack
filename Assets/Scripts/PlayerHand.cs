using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHand : Hand {

	protected override void DetermineFinalPosition()
    {
        // this is called :)
        Debug.Log("derived class call");

        Vector3 finalPos;

        float angleLowBound = 180 - 5 * (contents.Count - 1) / 2;
        float angleHighBound = 180 + 5 * (contents.Count - 1) / 2;
        float angleStep = (angleHighBound - angleLowBound) / contents.Count;

        float theta = angleLowBound;
        for (int i = 0; i < contents.Count; i++)
        {
            Vector3 rotation = contents[i].transform.localEulerAngles;
            rotation.z = (theta + 180) % 360;
            contents[i].transform.localEulerAngles = rotation;

            theta += angleStep;
        }
    }
}
