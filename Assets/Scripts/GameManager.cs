using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    GameStart,          // start of the game
    PlayerBets,         // player wagers bets
    DealingPhase,       // dealer deals to player
    PlayerTurn,         // player takes turn
    DealerCardReveal,   // hole card reveal, dealer draws cards
    PlayerCardReveal,   // player card reveal, victory determination
    RoundOver,          // player chooses to continue playing or end game
    GameOver            // end of the game
}

public class GameManager : Singleton<GameManager> {

    public GameState gameState;

    private void Awake()
    {
        gameState = GameState.GameStart;
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.GameStart:
                // wait until all game items are ready
                break;

            case GameState.PlayerBets:
                // change canvas to player bet canvas
                // wait for player entry
                break;

            case GameState.DealingPhase:
                // change canvas to in-game canvas
                // wait for dealer to finish dealing
                // if non-hole card is an ace or 10-card
                // wait for dealer to peak at hole card
                // if hole card leads to blackjack
                // go to card reveal phase
                break;

            case GameState.PlayerTurn:
                // change cnavas to turn canvas
                // wait for player to make turn
                break;

            case GameState.DealerCardReveal:
                // change canvas to in-game canvas
                // wait for dealer to reveal hole card
                // wait for dealer to draw cards if necessary
                break;

            case GameState.PlayerCardReveal:
                // wait for player score and dealer to be totalled
                break;

            case GameState.RoundOver:
                // prompt player to play again
                // if yes, prompt which card value changes
                break;

            case GameState.GameOver:
                // return to main menu
                break;

            default:
                throw new UnityException("Attempted to enter unknown state");
        }
    }
}
