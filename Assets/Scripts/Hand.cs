﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The hand object. Contains a list of
 * card objects that make up the contents
 * of the "hand." Also contains info about
 * the hand such as if it's a blackjack or
 * a bust. It also handles setting the 
 * transform the hand and in its contents
 * in complex transitions such as drawing
 * a card from the drawpile. The player and
 * dealer handle this in different ways,
 * so PlayerHand and DealerHand inherit from
 * this object
 */
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

    /**
     * A function that carefully calculates
     * the score of the hand. It considers
     * the best options for if an ace should
     * be a 1 or 11, and it also determines
     * if this hand is a blackjack, a bust,
     * or neither
     */
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

    public virtual void ResetStats()
    {
        blackjack = false;
        bust = false;
        score = 0;
    }

    public virtual void Reset()
    {
        Debug.Log("Resetting...");

        foreach (GameObject cardObject in contents)
        {
            Destroy(cardObject);
        }

        contents.Clear();

        isDrawing = false;

        drawPile.Reset();
    }

    /**
     * A base function that calculates the final transform
     * of the card. This transform will be different
     * for the player and dealer
     */
    protected virtual void DetermineFinalTransform(ref Transform finalTransform)
    {
        Debug.Log("base class call (hand)");
    }
}
