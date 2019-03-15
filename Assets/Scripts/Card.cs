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
            //Debug.Log("position of " + gameObject.name + ": " + transform.position);
            //Debug.Log("rotation of " + gameObject.name + ": " + transform.rotation);
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

    public IEnumerator FlipOver()
    {
        float moveTime = 0.5f, flipTime = 0.25f;

        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;

        Vector3 endPos = startPos + Vector3.up * 0.25f;
        Quaternion endRot = startRot;
        endRot.eulerAngles += new Vector3(180f, 0f, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(startPos, endPos, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = endPos;

        elapsedTime = 0f;

        while (elapsedTime < flipTime)
        {
            transform.rotation = Quaternion.Lerp(startRot, endRot, elapsedTime / flipTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.rotation = endRot;

        elapsedTime = 0f;

        while (elapsedTime < moveTime)
        {
            transform.position = Vector3.Lerp(endPos, startPos, elapsedTime / moveTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transform.position = startPos;
    }

}
