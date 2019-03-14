using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPile : MonoBehaviour {

    public Deck deck;
    public int deckCount = 2;

    public Dictionary<int, GameObject> drawPile;

    private void Awake()
    {
        drawPile = new Dictionary<int, GameObject>();

        // create the drawPile from the decks of cards
        for (int i = 0; i < deckCount; i++)
        {
            for (int j = 0; j < deck.contents.Count; j++)
            {
                drawPile.Add(
                    i * deck.contents.Count + j,
                    deck.contents[j]);
            }
        }
    }

    private void Start()
    {
        // debug output all of the cards in the pile
        Debug.Log("Draw pile contains " + drawPile.Count + " cards :");
        foreach (KeyValuePair<int, GameObject> card in drawPile)
        {
            Debug.Log(card.Key + " - " + card.Value.name);
        }
    }

    public GameObject DrawCard()
    {
        // draw pile is shuffled, so drawn card is random
        int cardIndex = Random.Range(0, drawPile.Count + 1);

        // get the card and remove it from the pile
        GameObject card = drawPile[cardIndex];
        drawPile.Remove(cardIndex);

        // return the card
        return card;
    }
}
