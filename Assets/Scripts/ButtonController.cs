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

    void Start(){
        player = 1;
    }

    void Update(){
    }

    // Current player picks ninja
    public void OnNinjaButtonClick(){
        if(player == 1){
            ++player;
            playerText.text = "Player 2";
        } else {
            startable = true;
        }
        Debug.Log("Btn Ninja!");
    }

    // Current player picks spearman
    public void OnSpearmanButtonClick(){
        if(player == 1){
            ++player;
            playerText.text = "Player 2";
        } else {
            startable = true;
        }
        Debug.Log("Btn Spear!");
    }
    
    // Undo player's pick
    public void OnUndoButtonClick(){
        if(player > 1){
        player = 1;
        startable = false;
        playerText.text = "Player 1";
        }
        Debug.Log("Btn undo!");
    }

    // Launch players to fight stage
    public void OnReadyButtonClick(){
        if(player == 2 && startable){
            
            
        playerText.text = "Player 1";
        player = 1;
        startable = false;
        } 
        Debug.Log("Btn Ready!");
    }

    // Quits game
    public void OnQuitClick(){
        
        Debug.Log("Btn Quit!");
    }

    // Go back to character selection
    public void OnPlayAgainClick(){
        
        playerText.text = "Player 1";
        player = 1;
        startable = false;
        Debug.Log("Btn Again!");
    }
}
