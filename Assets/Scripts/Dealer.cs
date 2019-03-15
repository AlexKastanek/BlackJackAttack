using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dealer : MonoBehaviour {

    public DealerHand hand;

    public float score = 0f;

    public Text scoreText;

    private float lastScore;

    private bool attackPhase = false;

    private void Start()
    {
        GameManager.Instance.stateChangedEvent.AddListener(OnStateChanged);

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
                // declare victory
                GameManager.Instance.StopAllCoroutines();
                GameManager.Instance.victor = "Dealer";
                GameManager.Instance.gameState = GameState.RoundOver;
            }
        }
    }

    public void UpdateDisplay()
    {
        scoreText.text = "Dealer's hand: " + score;
    }

    public void RoundReset()
    {
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
            RoundReset();
        }
    }
}
