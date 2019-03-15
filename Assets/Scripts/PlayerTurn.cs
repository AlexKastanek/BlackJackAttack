using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Class to handle all user input and
 * operations done during the course of
 * the player's turn state
 */
public class PlayerTurn : MonoBehaviour {

    public Canvas canvas;
    public Button standButton, hitButton, doubleButton, surrenderButton;
    public Text doubleError, surrenderError;

    private Player player;
    private PlayerBet playerBet;

    private void Start()
    {
        standButton.onClick.AddListener(Stand);
        hitButton.onClick.AddListener(Hit);
        doubleButton.onClick.AddListener(Double);
        surrenderButton.onClick.AddListener(Surrender);

        player = GetComponent<Player>();
        playerBet = GetComponent<PlayerBet>();
    }

    public void Stand()
    {
        GameManager.Instance.gameState = GameState.DealerCardReveal;
    }

    public void Hit()
    {
        StartCoroutine(HitHelper());
    }

    public void Double()
    {
        if (playerBet.DoubleWager())
        {
            StartCoroutine(DoubleHelper());
        }
        else
        {
            doubleError.enabled = true;
        }
    }

    public void Surrender()
    {
        if (player.hand.contents.Count <= 2)
        {
            playerBet.ForfeitHalfOfWager();
            player.DeclareLoss();
        }
        else
        {
            surrenderError.enabled = true;
        }
    }

    /**
     * Disables the canvas while the card is being
     * drawn
     */
    private IEnumerator HitHelper()
    {
        canvas.enabled = false;
        StartCoroutine(player.DrawCards(1));
        yield return new WaitUntil(() => !player.hand.isDrawing);
        canvas.enabled = true;

        player.hand.CalculateHandScore();

        player.UpdateDisplay();

        if (player.hand.bust)
            player.DeclareLoss();
    }

    /**
     * Draws a card for the player and waits until
     * card has been drawn before checking for bust 
     * and transitioning to next state
     * 
     * Also disables the canvas while the card is
     * being drawn
     */
    private IEnumerator DoubleHelper()
    {
        canvas.enabled = false;
        StartCoroutine(player.DrawCards(1));
        yield return new WaitUntil(() => !player.hand.isDrawing);
        canvas.enabled = true;

        player.hand.CalculateHandScore();

        player.UpdateDisplay();

        if (player.hand.bust)
            player.DeclareLoss();
        else
            GameManager.Instance.gameState = GameState.DealerCardReveal;
    }
}
