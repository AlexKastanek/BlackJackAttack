using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dealer : MonoBehaviour {

    public DealerHand hand;

    public Player player;

    public float score = 0f;

    public Text scoreText;

    private IEnumerator drawCardsCoroutine;

    private float lastScore;

    private bool attackPhase = false;

    private void Start()
    {
        GameManager.Instance.stateChangedEvent.AddListener(OnStateChanged);

        drawCardsCoroutine = DrawCards(1);

        lastScore = score;

        UpdateDisplay();
    }

    private void Update()
    {
        score = hand.score;

        if (lastScore != score)
        {
            UpdateDisplay();
        }

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
        else if (GameManager.Instance.gameState == GameState.DealerCardReveal)
        {
            if (hand.bust)
            {
                DeclareLoss();
            }

            if (!hand.holeCardRevealed)
            {
                hand.RevealHoleCard();
            }
            else if (!hand.isDrawing && score <= 16)
            {
                StartCoroutine(DrawCards(1)); 
            }
            else if (!hand.isDrawing && score > 16)
            {
                DetermineVictorByScore();
            }

            hand.CalculateHandScore();
        }
    }

    public void UpdateDisplay()
    {
        scoreText.text = "Dealer's hand: " + score;
    }

    public void DetermineVictorByScore()
    {
        if (score == 21 && player.score == 21)
        {
            if (hand.blackjack && !player.hand.blackjack)
            {
                DeclareVictory();
            }
            else if (!hand.blackjack && player.hand.blackjack)
            {
                DeclareLoss();
            }
            else
            {
                DeclarePush();
            }

            return;
        }

        if (score > player.score)
        {
            DeclareVictory();
        }
        else if (score < player.score)
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
        GameManager.Instance.victor = "Dealer";
        GameManager.Instance.gameState = GameState.RoundOver;
        //StopAllCoroutines();
    }

    public void DeclareLoss()
    {
        GameManager.Instance.victor = "Player";
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
            EndRound();
        }
    }
}
