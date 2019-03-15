using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public PlayerHand hand;

    public float balance = 20f;
    public float wager = 0f;
    public float score = 0f;

    public Text balanceText;
    public Text wagerText;
    public Text scoreText;

    private float lastBalance;
    private float lastWager;
    private float lastScore;

    private void Start()
    {
        GameManager.Instance.stateChangedEvent.AddListener(OnStateChanged);

        lastBalance = balance;
        lastWager = wager;
        lastScore = score;

        UpdateDisplay();
    }

    private void Update()
    {
        score = hand.score;

        if (lastBalance != balance || lastWager != wager || lastScore != score)
        {
            UpdateDisplay();
        }

        lastBalance = balance;
        lastWager = wager;
        lastScore = score;
    }

    public void UpdateDisplay()
    {
        balanceText.text = "Balance: " + balance.ToString("c2");
        wagerText.text = "Wager: " + wager.ToString("c2");
        scoreText.text = "Hand: " + score;
    }

    public IEnumerator DrawCards(int numCards)
    {
        int drawnCards = 0;

        while (drawnCards < numCards)
        {
            hand.DrawCard();
            yield return new WaitUntil(() => !hand.isDrawing);

            Debug.Log("incrementing drawn cards");
            drawnCards++;
        }

        hand.CalculateHandScore();
    }

    public void OnStateChanged(GameState gameState)
    {
        if (gameState == GameState.DealingPhase)
        {
            StartCoroutine(DrawCards(2));
        }
    }

}
