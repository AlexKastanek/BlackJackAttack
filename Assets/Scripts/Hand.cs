using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    public List<GameObject> contents;

    public DrawPile drawPile;

    public virtual void DrawCard()
    {
        // initialize the card positios and rotation
        Vector3 cardPos = drawPile.transform.position;
        Quaternion cardRot = Quaternion.identity;

        // create the card holder (used as a pivot for each card)
        /*
        GameObject empty = new GameObject();
        GameObject cardHolder = Instantiate(
            empty, 
            transform.position, 
            transform.rotation, 
            transform);
        */
        GameObject cardHolder = new GameObject();
        cardHolder.transform.SetParent(transform);
        cardHolder.transform.SetPositionAndRotation(
            transform.position, 
            transform.rotation);

        // create a card
        GameObject card = Instantiate(
            drawPile.DrawCard(),
            cardPos,
            cardRot,
            cardHolder.transform);
        contents.Add(card);

        cardHolder.name = contents[contents.Count - 1].name + "_Holder";

        // calculate the final position and rotation of the card
        DetermineFinalTransform(cardHolder.transform, out cardPos, out cardRot);

        contents[contents.Count - 1].transform.SetPositionAndRotation(
            cardPos,
            cardRot);
    }

    protected virtual void DetermineFinalTransform(Transform cardHolder, out Vector3 finalPos, out Quaternion finalRot)
    {
        Debug.Log("base class call");

        finalPos = Vector3.zero;
        finalRot = Quaternion.identity;
    }
}
