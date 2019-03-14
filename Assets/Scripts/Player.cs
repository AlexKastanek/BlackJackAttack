using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public PlayerHand hand;

    public float balance = 20f;
    public float wager = 0f;

    public Text balanceText;
    public Text wagerText;

    private float lastBalance;
    private float lastWager;

    private void Start()
    {
        GameManager.Instance.stateChangedEvent.AddListener(OnStateChanged);

        lastBalance = balance;
        lastWager = wager;

        UpdateDisplay();
    }

    private void Update()
    {
        if (lastBalance != balance || lastWager != wager)
        {
            UpdateDisplay();
        }
    }

    public void UpdateDisplay()
    {
        balanceText.text = "Balance: " + balance.ToString("c2");
        wagerText.text = "Wager: " + wager.ToString("c2");
    }

    public void OnStateChanged(GameState gameState)
    {
        if (gameState == GameState.DealingPhase)
        {
            hand.DrawCard();
        }
    }

}
