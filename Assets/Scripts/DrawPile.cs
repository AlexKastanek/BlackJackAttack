using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPile : MonoBehaviour {

    public Deck deck;
    public int deckCount = 2;

    public List<GameObject> drawPile;

    private void Awake()
    {
        Reset();
    }

    private void Start()
    {
        // debug output all of the cards in the pile
        Debug.Log("Draw pile contains " + drawPile.Count + " cards :");
        int cardIndex = 0;
        foreach (GameObject card in drawPile)
        {
            Debug.Log(cardIndex + " - " + card.name);
            cardIndex++;
        }
    }

    public GameObject DrawCard()
    {
        // draw pile is shuffled, so drawn card is random
        int cardIndex = Random.Range(0, drawPile.Count);

        // get the card and remove it from the pile
        GameObject card = drawPile[cardIndex];
        drawPile.RemoveAt(cardIndex);

        // return the card
        return card;
    }

    public void Reset()
    {
        drawPile = new List<GameObject>();

        // create the drawPile from the decks of cards
        for (int i = 0; i < deckCount; i++)
        {
            for (int j = 0; j < deck.contents.Count; j++)
            {
                drawPile.Add(deck.contents[j]);
            }
        }
    }
}
