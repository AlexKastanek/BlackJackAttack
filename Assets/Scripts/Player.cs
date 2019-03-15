using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public PlayerHand hand;

    public Dealer dealer;

    public float balance = 20f;
    public float wager = 0f;
    public float score = 0f;

    public Text balanceText;
    public Text wagerText;
    public Text scoreText;

    private IEnumerator drawCardsCoroutine;

    private float lastBalance;
    private float lastWager;
    private float lastScore;

    private bool attackPhase = false;

    private void Start()
    {
        GameManager.Instance.stateChangedEvent.AddListener(OnStateChanged);

        drawCardsCoroutine = DrawCards(1);

        lastBalance = balance;
        lastWager = wager;
        lastScore = score;

        UpdateDisplay();
    }

    /**
     * Most of this function should be moved to Hand.Update
     */
    private void Update()
    {
        score = hand.score;

        if (lastBalance != balance || lastWager != wager || lastScore != score)
        {
            if (GameManager.Instance.gameState != GameState.RoundOver)
                UpdateDisplay();
        }

        //TODO Move below to Hand.Update

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
                DetermineVictorByScore();
            }
        }
        else if (GameManager.Instance.gameState == GameState.PlayerTurn)
        {
            if (hand.bust)
            {
                DeclareLoss();
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
        else if (GameManager.Instance.victor == "Push")
        {
            // wager is given back to player
            balance += wager;
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

    public void DetermineVictorByScore()
    {
        if (score == 21 && dealer.score == 21)
        {
            if (hand.blackjack && !dealer.hand.blackjack)
            {
                DeclareVictory();
            }
            else if (!hand.blackjack && dealer.hand.blackjack)
            {
                DeclareLoss();
            }
            else
            {
                DeclarePush();
            }

            return;
        }

        if (score > dealer.score)
        {
            DeclareVictory();
        }
        else if (score < dealer.score)
        {
            DeclareLoss();
        }
        else
        {
            DeclarePush();
        }
    }

    public void DeclareVictory()
    { 
        GameManager.Instance.victor = "Player";
        GameManager.Instance.gameState = GameState.RoundOver;
        //StopAllCoroutines();
    }

    public void DeclareLoss()
    {
        GameManager.Instance.victor = "Dealer";
        GameManager.Instance.gameState = GameState.RoundOver;
        //StopAllCoroutines();
    }

    public void DeclarePush()
    {
        GameManager.Instance.victor = "Push";
        GameManager.Instance.gameState = GameState.RoundOver;
        //StopAllCoroutines();
    }

    public void EndRound()
    {
        StopCoroutine(drawCardsCoroutine);
        attackPhase = false; 
    }

    public void RoundReset()
    {
        score = 0f;
        hand.Reset();
        hand.ResetStats();
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
        if (gameState == GameState.GameStart)
        {
            RoundReset();
        }
        else if (gameState == GameState.DealingPhase)
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
            EndRound();
        }
    }

}
