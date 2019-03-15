using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundOver : MonoBehaviour {

    public Text roundOverText;
    public Button restartButton;
    public Button mainMenuButton;

    private void Start()
    {
        restartButton.onClick.AddListener(OnRoundRestartClicked);
    }

    private void Update()
    {
        roundOverText.text = "This round goes to " + GameManager.Instance.victor;
    }

    private void OnRoundRestartClicked()
    {
        GameManager.Instance.gameState = GameState.GameStart;
    }
}
