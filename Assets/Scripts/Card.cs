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

}
