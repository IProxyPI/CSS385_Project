using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private int player;
    private bool startable;
    public Player_Manager playerManager;
    public TMP_Text playerText;
    public Button readyButton;
    public Round_Vis_Manager rvm;
    private string player1, player2;

    void Start()
    {
        player = 1;
        if (playerText != null)
        {
            playerText.text = "Player 1 Selecting";
        }
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
            playerManager.NinjaP1.SetActive(true);
        }
        else if (player == 2)
        {
            player2 = "Ninja";
            playerText.text = "Ready?";
            startable = true;
            playerManager.NinjaP2.SetActive(true);
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
            playerManager.SpearmanP1.SetActive(true);
        }
        else if (player == 2)
        {
            player2 = "Spearman";
            playerText.text = "Ready?";
            startable = true;
            playerManager.SpearmanP2.SetActive(true);

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

    // Go to character selection screen
    public void OnCharacterSelectClick()
    {
        rvm.Start_Game();
        SceneManager.LoadScene("0_Select_Fighter");
        player1 = "";
        player2 = "";
        player = 1;
        startable = false;
        Debug.Log("Btn Select!");

    }

    // Go to main menu
    public void OnHomeClick()
    {
        SceneManager.LoadScene("MainMenu");
        Debug.Log("Btn Main Menu!");

    }

    // Go to controls
    public void OnControlsClick()
    {
        SceneManager.LoadScene("Controls");
        Debug.Log("Btn Controls!");

    }
}
