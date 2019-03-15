﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Object that holds information about
 * the dealer's hand. Inherits from Hand.
 */
public class DealerHand : Hand {

    public bool holeCardRevealed = false;
    public bool firstDraw = true;

    /**
     * Overridden CalculateHandScore() function.
     * The reason this needed to be overridden is
     * because of the hole card, and how it is not
     * supposed to be considered until it is revealed.
     * Once it is revealed, this just calls the
     * base class function.
     */
    public override void CalculateHandScore()
    {
        if (holeCardRevealed)
        {
            base.CalculateHandScore();
        }
        else
        {
            bool tenCard = false;
            int aceCount = 0;
            score = 0f;

            for (int i = 1; i < contents.Count; i++)
            {
                Card card = contents[i].GetComponent<Card>();

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

            if (firstDraw && score >= 10)
            {
                firstDraw = false;
                RevealHoleCard();

                CalculateHandScore();

                GameManager.Instance.gameState = GameState.AttackPhase;
            }
            else if (firstDraw && score < 10)
            {
                firstDraw = false;
                GameManager.Instance.gameState = GameState.PlayerTurn;
            }
        }

        firstDraw = false;
    }

    public void RevealHoleCard()
    {
        Debug.Log("revealing hole card...");
        StartCoroutine(contents[0].GetComponent<Card>().FlipOver());
        holeCardRevealed = true;
    }

    public override void ResetStats()
    {
        base.ResetStats();

        firstDraw = true;
        holeCardRevealed = false;
    }

    /**
     * Overridden DetermineFinalTransform() function.
     * This will essentially calculate the final
     * position and rotation for when a card is drawn
     * and iterpolated from the draw pile to the hand.
     * For the dealer hand, this is simply a matter of
     * if it is the hole card, put it on the left, and
     * if it's not, put it on the right, flip it over,
     * and stack it
     */
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
