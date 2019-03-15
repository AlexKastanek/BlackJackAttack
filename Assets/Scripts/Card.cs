using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Suit
{
    HEARTS,
    DIAMONDS,
    CLUBS,
    SPADES
};

public class Card : MonoBehaviour
{

    public int rank;
    public Suit suit;

    public bool isInterpolating = false;

    public IEnumerator InterpolateTransform(Vector3 startPos, Quaternion startRot, Vector3 endPos, Quaternion endRot, float time)
    {
        isInterpolating = true;

        float elapsedTime = 0f;

        while (elapsedTime < time)
        {
            Debug.Log("position of " + gameObject.name + ": " + transform.position);
            Debug.Log("rotation of " + gameObject.name + ": " + transform.rotation);
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / time);
            transform.rotation = Quaternion.Lerp(startRot, endRot, elapsedTime / time);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        isInterpolating = false;
        Debug.Log("is interpolating set to false");

        transform.position = endPos;
        transform.rotation = endRot;

        transform.parent.parent.GetComponent<Hand>().isDrawing = false;
    }

}
