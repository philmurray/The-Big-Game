using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class RoundOverModalBehavior : MonoBehaviour {

    public Text TitleText;
    public List<Image> PlayerOnePanels;
    public List<Image> PlayerTwoPanels;
    public Text ButtonText;

    private GameController.Player _winner;

    public bool GameOver
    {
        get
        {
            return GameController.instance.GetPlayer(_winner).Winner;
        }
    }

    public void ShowWinner(GameController.Player player)
    {
        _winner = player;
        UpdateData();

        string scopeText = GameOver ? "Game" : "Round";
        string playerText = player == GameController.Player.One ? "Player One" : "Player Two";
        
        TitleText.text = playerText + " Wins the " + scopeText + "!";
    }

    public void ShowTie()
    {
        UpdateData();
        TitleText.text = "Its a tie!";
    }

    private void UpdateData()
    {
        gameObject.SetActive(true);
        ButtonText.text = GameOver ? "Return to Main Menu" : "Next Round";

        var playerOneScore = GameController.instance.GetPlayer(GameController.Player.One).Score;
        var playerTwoScore = GameController.instance.GetPlayer(GameController.Player.Two).Score;
        for (int i = 0; i < PlayerOnePanels.Count; i++)
        {
            var playerOneImg = PlayerOnePanels[i];
            var playerTwoImg = PlayerTwoPanels[i];

            if (playerOneScore >= i + 1)
            {
                playerOneImg.color = Color.green;
            }
            else
            {
                playerOneImg.color = new Color(255, 255, 255, 100);
            }

            if (playerTwoScore >= i + 1)
            {
                playerTwoImg.color = Color.green;
            }
            else
            {
                playerTwoImg.color = new Color(255, 255, 255, 100);
            }
        }
    }

    public void NextRound()
    {
        GameController.instance.NextScene();
    }
}
