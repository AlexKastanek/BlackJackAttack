using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerBet : MonoBehaviour {

    public Text message;
    public Text inputFieldText;

    public Button submitButton;

    private Player player;

    private void Start()
    {
        player = GetComponent<Player>();

        submitButton.onClick.AddListener(CheckWagerEntry);
    }

    public void CheckWagerEntry()
    {
        float wager;

        // check if wager is valid
        try
        {
            wager = float.Parse(inputFieldText.text);
        }
        catch
        {
            DisplayInvalidWager();
            return;
        }

        // check if wager exceeds bounds
        if (wager <= 0f || wager > player.balance)
        {
            DisplayInvalidWager();
            return;
        }

        // if wager is valid, set the wager and go to next state
        player.balance -= wager;
        player.wager = wager;
        GameManager.Instance.gameState = GameState.DealingPhase;
    }

    private void DisplayInvalidWager()
    {
        message.color = Color.red;
        message.text = "Invalid wager";
    }
}
