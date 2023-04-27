using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller : MonoBehaviour
{

    private Player_Manager pm;
    public int player = 1;                  // 1 = player 1, 2 = player 2
    private int character_choice = 0;       // 0 = Spearman, 1 = Ninja
    private bool character_change = false;
    public Character ch = null;
    public int facing_dir = 1;             // 1 = right, -1 = left
    public int menu_choice = 0;             // 0 = No change
    
    private Rigidbody2D rb;
    private string input_Forward = "s";
    private string input_Backward = "a";
    private string input_Attack = "z";
    private string input_Stun = "x";
    private string input_Block = "c";
    private bool actionable = true;

    // Start is called before the first frame update
    void Start()
    {
        pm = GameObject.Find("Players").GetComponent<Player_Manager>();
        ch = gameObject.AddComponent<Spearman>() as Spearman;
        rb = GetComponent<Rigidbody2D>();

        if (gameObject.name == "DummyPlayer2")
        {
            player = 2;
            facing_dir = -1;
            input_Forward = "j";
            input_Backward = "k";
            input_Attack = "m";
            input_Stun = ",";
            input_Block = ".";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (pm.game_state == 1)
        {
            if (Input.GetKey(input_Forward))
            {
                ch.move_forward = 1;
            }
            else
            {
                ch.forward_stop = 1;
            }

            if (Input.GetKey(input_Backward))
            {
                ch.move_backward = 1;
            }
            else
            {
                ch.backward_stop = 1;
            }

            if (actionable)
            {
                if (Input.GetKeyDown(input_Attack))
                {
                    actionable = false;
                    ch.attack = 1;  // changed from 0.  represents frame 1 of move, which can be incremented to count what happens each frame in ch?
                }
                
                if (Input.GetKeyDown(input_Stun))
                {
                    actionable = false;
                    ch.stun = 1;
                }
                
                if (Input.GetKeyDown(input_Block))
                {
                    actionable = false;
                    ch.block = 1;
                }
            }
        }
        
        // Post-Match inputs
        // else if (gm.game_state = 2)
        // {
        //     // rematch, character select, quit, etc.
        // }
        
        // Character Selection Screen Inputs
        else // (gm.game_state = 0)
        {
            // If not ready to fight
            if (menu_choice == 0)
            {
                // Change character
                if (Input.GetKeyDown(input_Forward) || Input.GetKeyDown(input_Backward))
                {
                    character_choice = (character_choice + 1) % 2;
                    character_change = true;
                }
                if (character_change)
                {
                    // Not sure if this works, but would future-proof code for additional characters if so
                    // Destroy(gameObject.GetComponent<Character>());

                    if (character_choice == 0)
                    {
                        Debug.Log("P" + (player) + " Selects Spearman");
                        Destroy(gameObject.GetComponent<Ninja>());
                        ch = gameObject.AddComponent<Spearman>() as Spearman;
                    }
                    else // (character_choice == 1)
                    {
                        Debug.Log("P" + (player) + " Selects Ninja");
                        Destroy(gameObject.GetComponent<Spearman>());
                        ch = gameObject.AddComponent<Ninja>() as Ninja;
                    }

                    // See line 102
                    // ch = gameObject.GetComponent<Character>();
                    
                    character_change = false;
                }

                // Toggle ready to fight
                if (menu_choice == 0 && Input.GetKeyDown(input_Attack))
                {
                    Debug.Log("P" + player + " READY");
                    menu_choice = 1;
                }
            }

            // Untoggle ready to fight and change character
            else if (/*menu_choice == 1 &&*/ Input.GetKeyDown(input_Stun))
            {
                Debug.Log("Update() 0, UNDO");
                menu_choice = 0;
            }
        }
    }
}