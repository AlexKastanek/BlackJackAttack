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

    private bool attackPhase = false;

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

        if (attackPhase)
        {
            if (!hand.isDrawing)
            {
                StartCoroutine(DrawCards(1));
                hand.CalculateHandScore();
            }

            if (score >= 50)
            {
                // declare victory
                GameManager.Instance.StopAllCoroutines();
                GameManager.Instance.victor = "Player";
                GameManager.Instance.gameState = GameState.RoundOver;
            }
        }
    }

    private void CalculateWinnings()
    {
        if (GameManager.Instance.victor == "Player")
        {
            if (hand.blackjack && !attackPhase)
            {
                // payoff is 3:2
                balance += wager * 2.5f;
            }
            else
            {
                // payoff is 1:1
                balance += wager * 2f;
            }
        }

        wager = 0f;

        UpdateDisplay();

        if (balance == 0f)
        {
            Debug.Log("Game Over");
        }
    }

    public void UpdateDisplay()
    {
        balanceText.text = "Balance: " + balance.ToString("c2");
        wagerText.text = "Wager: " + wager.ToString("c2");
        scoreText.text = "Hand: " + score;
    }

    public void RoundReset()
    {
        attackPhase = false;
        score = 0f;
        hand.Reset();
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
        else if (gameState == GameState.AttackPhase)
        {
            attackPhase = true;
        }
        else if (gameState == GameState.RoundOver)
        {
            CalculateWinnings();
            RoundReset();
        }
    }

}
