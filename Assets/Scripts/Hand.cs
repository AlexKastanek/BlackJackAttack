using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    public List<GameObject> contents;

    public DrawPile drawPile;

    public float score = 0f;

    public bool blackjack = false;
    public bool bust = false;
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

    public virtual void CalculateHandScore()
    {
        bool tenCard = false;
        int aceCount = 0;
        score = 0f;

        foreach (GameObject cardObject in contents)
        {
            Card card = cardObject.GetComponent<Card>();

            if (card.rank == 1)
            {
                aceCount++;
                
            }
            else if (card.rank >= 10)
            {
                tenCard = true;
                score += 10;
            }
            else
            {
                score += card.rank;
            }
        }

        if (contents.Count == 2 && aceCount > 0 && tenCard)
        {
            blackjack = true;
            score = 21;
        }
        else
        {
            for (int i = 0; i < aceCount; i++)
            {
                if (score + 11 > 21)
                {
                    score += 1;
                }
                else
                {
                    score += 11;
                }
            }

            if (score > 21)
            {
                bust = true;
            }
        }
    }

    public virtual void Reset()
    {
        blackjack = false;
        bust = false;
        score = 0;
        
        foreach (GameObject cardObject in contents)
        {
            Destroy(cardObject);
        }

        contents.Clear();

        isDrawing = false;

        drawPile.Reset();
    }

    protected virtual void DetermineFinalTransform(ref Transform finalTransform)
    {
        Debug.Log("base class call (hand)");
    }
}
