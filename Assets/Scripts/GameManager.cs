using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public enum GameState
{
    GameStart,          // start of the game
    PlayerBets,         // player wagers bets
    DealingPhase,       // dealer deals to player
    AttackPhase,        // the twist! dealer and player race to 50
    PlayerTurn,         // player takes turn
    DealerCardReveal,   // hole card reveal, dealer draws cards, victory determination
    RoundOver,          // player chooses to continue playing or end game
    GameOver            // end of the game
}

public class StateChangedEvent : UnityEvent<GameState>
{

}

/**
 * Inherited from the Singleton class, this object
 * keeps track of what state the game is, and will
 * send out an event every time the state is changed
 */
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

        /**
         * This switch statement is mostly here for readability
         * and to clearly display what each state does
         */
        switch (gameState)
        {
            case GameState.GameStart:
                // immediately transition to next state and stop all hanging coroutines
                StopAllCoroutines();
                gameState = GameState.PlayerBets;
                break;

            case GameState.PlayerBets:
                /**
                 * In this state, the canvas changes to the bet canvas,
                 * and the player is prompted for a wager. Once the player
                 * clicks submit, it's on to the next state
                 */
                break;

            case GameState.DealingPhase:
                /**
                 * In this state, the player waits for the cards to be
                 * dealt. The player's initial cards are tallied and
                 * the dealer's face-up card is evaluated. If this card
                 * has a value of 10 or more, the attack phase is entered.
                 * Otherwise, we move on to the player's turn.
                 */
                break;

            case GameState.AttackPhase:
                /**
                 * In this phase, the dealer and the player draw
                 * cards at a fast rate and race to see who can get
                 * to fifty first. If they reached fifty at the same
                 * time, the larger score wins. Note that this phase
                 * is purely based on luck of the draw. It pays 1:1.
                 */
                break;

            case GameState.PlayerTurn:
                /**
                 * In this state, the canvas changes to the player
                 * turn canvas. The player now will have access to
                 * 4 different options: stand, hit, double, surrender.
                 * If the player chooses stand, we move on to the next
                 * state. If he/she chooses hit, they draw another card
                 * with the risk of busting. If they choose double,
                 * they double their bets, draw one more card,
                 * and then move on to the next event. If they choose
                 * surrender, the round is ended, and the player
                 * must sacrifice half of their wager.
                 */
                break;

            case GameState.DealerCardReveal:
                /**
                 * In this state the canvas is changed back to the
                 * default canvas, and the dealer draws until a
                 * card value greater than 16 is reached or until
                 * the dealer busts. The player gets to watch this
                 * unfold in anticipation.
                 */
                break;

            case GameState.RoundOver:
                /**
                 * In this state the canvas changes to the round
                 * over canvas, where the player has the option to
                 * keep playing, using the same balance as previous
                 * rounds, or to return to the main menu where
                 * progress will be lost.
                 */
                break;

            case GameState.GameOver:
                // load game over scene
                SceneManager.LoadScene("GameOver");
                break;

            default:
                // this will never be reached
                throw new UnityException("Attempted to enter unknown game state");
        }
    }

    public void SetGameState(GameState gameState)
    {
        this.gameState = gameState;
    }
}
