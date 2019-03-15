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
        Quaternion cardStartRot = Quaternion.identity;

        // create a card
        GameObject card = Instantiate(
            drawPile.DrawCard(),
            cardStartPos,
            cardStartRot);
        contents.Add(card);

        // create the card holder (used as a pivot for each card)
        GameObject cardHolder = new GameObject();
        cardHolder.transform.SetParent(transform);
        cardHolder.transform.SetPositionAndRotation(
            transform.position, 
            transform.rotation);
        cardHolder.name = card.name + "_Holder";

        
        //Vector3 localPosition = cardHolder.transform.parent.position;
        //localPosition += cardHolder.transform.forward * 1;
        //cardHolder.transform.position = localPosition;
        
        

        
        // initialize target transform with current transform
        Transform targetCardTransform = card.transform;
        targetCardTransform.SetParent(cardHolder.transform);

        // calculate the final position and rotation of the card
        DetermineFinalTransform(ref targetCardTransform);

        card.transform.SetParent(cardHolder.transform);
        //card.transform.position = targetCardTransform.position;
        //card.transform.rotation = targetCardTransform.rotation;
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
        Debug.Log("base class call");
    }
}
