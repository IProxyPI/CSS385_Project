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
    public Player_Controller pc1, pc2;
    public Character ch1, ch2;
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
            pc1.anim = GameObject.Find("NinjaP1").GetComponent<Animator>();
            ch1 = pc1.GetComponent<Character>();
            ch1.sr = GameObject.Find("NinjaP1").GetComponent<SpriteRenderer>();
            ch1._attack_obj = GameObject.Find("NinjaAttackP1");
            ch1._stun_obj = GameObject.Find("NinjaStunP1");
            ch1._block_obj = GameObject.Find("NinjaBlockP1");
        }
        else if (player == 2)
        {
            player2 = "Ninja";
            playerText.text = "Ready?";
            startable = true;
            playerManager.NinjaP2.SetActive(true);
            pc2.anim = GameObject.Find("NinjaP2").GetComponent<Animator>();
            ch2 = pc2.GetComponent<Character>();
            ch2.sr = GameObject.Find("NinjaP2").GetComponent<SpriteRenderer>();
            ch2._attack_obj = GameObject.Find("NinjaAttackP2");
            ch2._stun_obj = GameObject.Find("NinjaStunP2");
            ch2._block_obj = GameObject.Find("NinjaBlockP2");
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
            pc1.anim = GameObject.Find("SpearmanP1").GetComponent<Animator>();
            ch1 = pc1.GetComponent<Character>();
            ch1.sr = GameObject.Find("SpearmanP1").GetComponent<SpriteRenderer>();
            ch1._attack_obj = GameObject.Find("SpearmanAttackP1");
            ch1._stun_obj = GameObject.Find("SpearmanStunP1");
            ch1._block_obj = GameObject.Find("SpearmanBlockP1");
        }
        else if (player == 2)
        {
            player2 = "Spearman";
            playerText.text = "Ready?";
            startable = true;
            playerManager.SpearmanP2.SetActive(true);
            pc2.anim = GameObject.Find("SpearmanP2").GetComponent<Animator>();
            ch2 = pc2.GetComponent<Character>();
            ch2.sr = GameObject.Find("SpearmanP2").GetComponent<SpriteRenderer>();
            ch2._attack_obj = GameObject.Find("SpearmanAttackP2");
            ch2._stun_obj = GameObject.Find("SpearmanStunP2");
            ch2._block_obj = GameObject.Find("SpearmanBlockP2");
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
            playerManager.SpearmanP2.SetActive(false);
            playerManager.NinjaP2.SetActive(false);
        }
        else if (player == 2)
        {
            player = 1;
            player1 = "";
            playerText.text = "Player 1 Selecting";
            playerManager.SpearmanP1.SetActive(false);
            playerManager.NinjaP1.SetActive(false);
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
