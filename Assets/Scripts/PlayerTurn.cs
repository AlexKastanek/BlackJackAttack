using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurn : MonoBehaviour {

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
        StartCoroutine(player.DrawCards(1));
    }

    public void Double()
    {
        if (playerBet.DoubleWager())
        {
            StartCoroutine(player.DrawCards(1));
            GameManager.Instance.gameState = GameState.DealerCardReveal;
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
            GameManager.Instance.gameState = GameState.RoundOver;
        }
        else
        {
            surrenderError.enabled = true;
        }
    }
}
