using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    public List<GameObject> contents;

    public DrawPile drawPile;

    public bool isDrawing = false;

    public virtual void DrawCard()
    {
        isDrawing = true;

        // initialize the card position and rotation
        Vector3 cardStartPos = drawPile.transform.position;
        Quaternion cardStartRot = Quaternion.Euler(90f, 0f, 0f);

        // create a card
        GameObject card = Instantiate(
            drawPile.DrawCard(),
            cardStartPos,
            Quaternion.identity);
        contents.Add(card);

        // create the card holder (used as a pivot for each card)
        GameObject cardHolder = new GameObject();
        cardHolder.transform.SetParent(transform);
        cardHolder.transform.SetPositionAndRotation(
            transform.position, 
            transform.rotation);
        cardHolder.name = card.name + "_Holder";
        
        // initialize target transform with current transform
        Transform targetCardTransform = card.transform;
        targetCardTransform.SetParent(cardHolder.transform);

        // calculate the final position and rotation of the card
        DetermineFinalTransform(ref targetCardTransform);

        // set the card holder as a parent of the card
        card.transform.SetParent(cardHolder.transform);
 
        // start a coroutine to interpolate the card transform
        StartCoroutine(
            card.GetComponent<Card>().InterpolateTransform(
                cardStartPos,
                cardStartRot,
                targetCardTransform.position, 
                targetCardTransform.rotation, 
                0.5f));
    }

    protected virtual void DetermineFinalTransform(ref Transform finalTransform)
    {
        Debug.Log("base class call (hand)");
    }
}
