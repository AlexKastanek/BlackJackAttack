using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour {

    public List<GameObject> contents;

    public DrawPile drawPile;

    public virtual void DrawCard()
    {
        DetermineFinalPosition();
    }

    protected virtual void DetermineFinalPosition()
    {
        Debug.Log("base class call");
    }
}
