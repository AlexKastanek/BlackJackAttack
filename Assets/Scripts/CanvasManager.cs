using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour {

    public Canvas defaultCanvas;
    public Canvas betCanvas;
    public Canvas scoreCanvas;

    private void Start()
    {
        GameManager.Instance.stateChangedEvent.AddListener(OnStateChanged);
        Debug.Log("Canvas manager added listener to state changed event");

        DisableAll();
    }

    public void OnStateChanged(GameState gameState)
    {
        Debug.Log("State changed (canvas manager)");

        DisableAll();

        switch (gameState)
        {
            case GameState.GameStart:
                defaultCanvas.enabled = true;
                break;
            case GameState.PlayerBets:
                defaultCanvas.enabled = true;
                betCanvas.enabled = true;
                break;
            case GameState.DealingPhase:
                defaultCanvas.enabled = true;
                scoreCanvas.enabled = true;
                break;
            default:
                defaultCanvas.enabled = true;
                break;
        }
    }

    private void DisableAll()
    {
        defaultCanvas.enabled = false;
        betCanvas.enabled = false;
    }
}
