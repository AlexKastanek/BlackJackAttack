using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
 * Handles all operations on the player's
 * balance and wagers. Handles all input 
 * from the bet canvas
 */
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

    /**
     * Checks to make sure the value inserted into the
     * input field is a valid entry
     */
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

    /**
     * Doubles the wager. Returns true if possible
     * and false if not.
     */
    public bool DoubleWager()
    {
        if (player.wager > player.balance)
        {
            return false;
        }
        else
        {
            player.balance -= player.wager;
            player.wager *= 2;
            return true;
        }
    }

    /**
     * Splits wager in half and adds to balance.
     * Used with Surrender().
     */
    public void ForfeitHalfOfWager()
    {
        player.wager /= 2;
        player.balance += player.wager;
    }

    private void DisplayInvalidWager()
    {
        message.color = Color.red;
        message.text = "Invalid wager";
    }
}
