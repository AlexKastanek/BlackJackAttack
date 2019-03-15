using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Contains all player data and functionality.
 * This mostly includes victory conditions
 * for the player and the player's balance
 * information.
 * 
 * Note that some of this functionality
 * would be much better suited in either
 * a base class that both a player object
 * and dealer object inherit from or in the
 * hand object since lots of this code is
 * duplicated in the dealer object.
 */
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

    private void Update()
    {
        // get the score from the current hand
        score = hand.score;

        // if info has changed update the display
        if (lastBalance != balance || lastWager != wager || lastScore != score)
        {
            UpdateDisplay();
        }

        lastBalance = balance;
        lastWager = wager;
        lastScore = score;

        // if in the attack state, keep drawing cards
        // until 50 is reached. Then determine the victor
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
        // else check if hand has busted
        else if (GameManager.Instance.gameState == GameState.PlayerTurn)
        {
            if (hand.bust)
            {
                DeclareLoss();
            }
        }
    }

    /**
     * Function to determine how much to add
     * to the player balance
     */
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
            GameManager.Instance.gameState = GameState.GameOver;
        }
    }

    public void UpdateDisplay()
    {
        balanceText.text = "Balance: " + balance.ToString("c2");
        wagerText.text = "Wager: " + wager.ToString("c2");
        scoreText.text = "Hand: " + score;
    }

    /**
     * This function prioritizes blackjacks
     * but if no one has a blackjack then
     * it determines the victor by score
     */
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

    /**
     * A coroutine that will continuously draw
     * cards for numCards amount of times
     */
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
