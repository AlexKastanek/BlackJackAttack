using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum GameState
{
    GameStart,          // start of the game
    PlayerBets,         // player wagers bets
    DealingPhase,       // dealer deals to player
    AttackPhase,        // the twist! dealer and player race to 50
    PlayerTurn,         // player takes turn
    DealerCardReveal,   // hole card reveal, dealer draws cards
    PlayerCardReveal,   // player card reveal, victory determination
    RoundOver,          // player chooses to continue playing or end game
    GameOver            // end of the game
}

public class StateChangedEvent : UnityEvent<GameState>
{

}

public class GameManager : Singleton<GameManager> {

    public StateChangedEvent stateChangedEvent;

    public GameState gameState;

    public string victor;

    private GameState lastGameState;

    private void Awake()
    {
        stateChangedEvent = new StateChangedEvent();

        gameState = GameState.GameStart;
        lastGameState = GameState.GameOver;
    }

    private void Update()
    {
        // check if current state is different than last state
        if (gameState != lastGameState)
        {
            Debug.Log("state changed (game manager)");

            // invoke the state changed event
            stateChangedEvent.Invoke(gameState);
            Debug.Log("Game manager invoked state changed event");

            lastGameState = gameState;
            Debug.Log("Game manager reset lastGameState");
        }

        switch (gameState)
        {
            case GameState.GameStart:
                // immediately transition to next state
                StopAllCoroutines();
                gameState = GameState.PlayerBets;
                break;

            case GameState.PlayerBets:
                // wait for player entry
                break;

            case GameState.DealingPhase:
                // change canvas to in-game canvas
                // wait for dealer to finish dealing
                // if non-hole card is an ace or 10-card
                // wait for dealer to flip hole card.
                // Once the dealer flips hole card
                // initiate attack phase
                break;

            case GameState.AttackPhase:
                // dealer and player race to 50
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
                break;

            case GameState.GameOver:
                // return to main menu
                break;

            default:
                throw new UnityException("Attempted to enter unknown game state");
        }
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
    }
}
