using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * A data structure that contains a list of cards
 * called "contents." The size of this list in the game
 * is the standard deck size of 52, and is used to
 * populate the pile by default with 2 decks.
 */
public class Deck : MonoBehaviour {

    public List<GameObject> contents;

}
