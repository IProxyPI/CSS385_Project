using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonController : MonoBehaviour
{
    private int player;
    private bool startable;
    public Player_Manager playerManager;
    public TMP_Text playerText;
    private string player1, player2;

    void Start()
    {
        player = 1;
    }

    void Update()
    {
    }

    // Current player picks ninja
    public void OnNinjaButtonClick()
    {
        if (player == 1)
        {
            ++player;
            player1 = "Ninja";
            playerText.text = "Player 2 Selecting";
        }
        else if (player == 2)
        {
            player2 = "Ninja";
            playerText.text = "Ready?";
            startable = true;
        }
        Debug.Log("Btn Ninja!");
    }

    // Current player picks spearman
    public void OnSpearmanButtonClick()
    {
        if (player == 1)
        {
            ++player;
            player1 = "Spearman";
            playerText.text = "Player 2 Selecting";
        }
        else if (player == 2)
        {
            player2 = "Spearman";
            playerText.text = "Ready?";
            startable = true;
        }
        Debug.Log("Btn Spear!");
    }

    // Undo player's pick
    public void OnUndoButtonClick()
    {
        if (player == 2 && startable)
        {
            player = 2;
            player2 = "";
            startable = false;
            playerText.text = "Player 2 Selecting";
        }
        else if (player == 2)
        {
            player = 1;
            player1 = "";
            playerText.text = "Player 1 Selecting";
        }
        Debug.Log("Btn undo!");
    }

    // Launch players to fight stage
    public void OnReadyButtonClick()
    {
        if (startable)
        {
            playerText.text = "Player 1 Selecting";
            player = 1;
            startable = false;
            SceneManager.LoadScene("1_Fight");
        }
        Debug.Log("Btn Ready!");
    }

    // Quits game
    public void OnQuitClick()
    {
        Application.Quit();
        Debug.Log("Btn Quit!");
    }

    // Go back to character selection
    public void OnPlayAgainClick()
    {
        SceneManager.LoadScene("0_Select_Fighter");
        player1 = "";
        player2 = "";
        playerText.text = "Player 1 Selecting";
        player = 1;
        startable = false;
        Debug.Log("Btn Again!");
    }
}
