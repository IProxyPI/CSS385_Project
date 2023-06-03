using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour
{
    // Used in Start(), scene_select_fighter, scene_fight's pause, and/or scene_select_next
    public Player_Manager pm;
    public Character ch;
    public Animator anim;
    public Rigidbody2D rb;
    public BoxCollider2D bc;
    public int facing_dir = 1;                      // 1 = left side facing right; -1 = right side facing left
    // public int character_select = -1;               // 0 = Spearman, 1 = Ninja
    // private bool character_select_change = false;
    // public int menu_select = 0;                     // 0 = scene_fight, 1 = scene_select_fighter, 2 = quit application
    // public bool menu_select_change = false;
    // public int menu_choice = 0;                     // 0 = no change
    // public bool menu_choice_change = false;
    // private float quit_timer = 0;
    // private int quit_timer_limit = 3;

    // Used in scene_fight
    public string player_tag = "P1";
    public string opponent_tag = "P2";
    public int lives = 2;
    // public bool paused = false;
    public bool actionable = false;
    public bool socd_neutral = false;               // False = L/R -> L, True = L/R -> N (Facing R)
    public bool tools_usable = true;
    private KeyCode input_pause = KeyCode.Space;
    private KeyCode input_forward = KeyCode.S;
    private KeyCode input_backward = KeyCode.A;
    private KeyCode input_attack = KeyCode.X;
    private KeyCode input_stun = KeyCode.C;
    private KeyCode input_block = KeyCode.V;

    // Start is called before the first frame update
    void Start()
    {
        pm = GameObject.Find("Players").GetComponent<Player_Manager>();
        ch = gameObject.GetComponent<Character>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();

        rb.gravityScale = 0;
        bc.isTrigger = true;

        if (gameObject.name == "DummyPlayer2")
        {
            facing_dir = -1;
            player_tag = "P2";
            opponent_tag = "P1";
            input_pause = KeyCode.Return;
            input_forward = KeyCode.LeftArrow;
            input_backward = KeyCode.RightArrow;
            input_attack = KeyCode.M;
            input_stun = KeyCode.Comma;
            input_block = KeyCode.Period;
        }
        // gameObject.tag = player_tag;

    }

    // Update is called once per frame
    void Update()
    {
        // // character assignment
        // if (character_select == -1)
        // {
        //     if (player_tag == "P1")
        //     {
        //         if (Player_Manager.Instance.SpearmanP1.activeInHierarchy)
        //         {
        //             character_select = 0;
        //         }
        //         else if (Player_Manager.Instance.NinjaP1.activeInHierarchy)
        //         {
        //             character_select = 1;
        //         }
        //     }
        //     else if (player_tag == "P2")
        //     {
        //         if (Player_Manager.Instance.SpearmanP2.activeInHierarchy)
        //         {
        //             character_select = 0;
        //         }
        //         else if (Player_Manager.Instance.NinjaP2.activeInHierarchy)
        //         {
        //             character_select = 1;
        //         }
        //     }
        // }
        
        // Each input function corresponds to a scene.  Ordered by frequency for better performance.
        if (pm.scene_num == 1 && !paused)
        {
            ReadFightInputs();
        }
        else if ((pm.scene_num == 1 && paused) || pm.scene_num == 2)
        {
            ReadMenuInputs();
        }
        else // (pm.scene_num == 0)
        {
            //ReadSelectFighterInputs();
        }
    }

    // Called by ReadFightInputs()
    private void StopMovement()
    {
        ch.forward = false;
        ch.backward = false;
        anim.SetFloat("Speed", 0);
    }
    
    // Hurt or Stunned
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == opponent_tag && ch.invincibility_timer == -1)
        {
            if (col.name.Contains("Attack") && !ch.block)
            {
                pm.p1.actionable = false;
                pm.p1.StopMovement();

                pm.p2.actionable = false;
                pm.p2.StopMovement();
                
                lives--;
                
                if (lives == 1)
                {
                    pm.p1.ch.invincibility_timer = 2f;
                    pm.p2.ch.invincibility_timer = 2f;
                    anim.SetTrigger("Hurt");
                    ch.hurt = true;
                }
                else if (lives == 0)
                {
                    anim.SetTrigger("Dead");
                    ch.dead = true;
                }
            }
            else if (col.name.Contains("Stun") && !ch.attack)
            {
                actionable = false;
                StopMovement();
                anim.SetTrigger("Stunned");
                ch.stunned = true;
            }
        }
    }

    private void ReadFightInputs()
    {
        // // Pause
        // if (Input.GetKeyDown(input_pause))
        // {
        //     Debug.Log(player_tag + " paused");
        //     // Initialize pause menu prefab
        //     paused = true;
        // }
        
        if (actionable)
        {
            // Movement
            {
                // move backward
                if (Input.GetKey(input_backward) && !ch.backward /*&& !ch.backward_stop*/)
                {
                    ch.backward = true;
                    anim.SetFloat("Speed", 1);
                }
                // stop backward
                else if (Input.GetKeyUp(input_backward) /*&& !ch.backward_stop*/)
                {
                    ch.backward = false;
                    anim.SetFloat("Speed", 0);
                }
                // move forward
                if (Input.GetKey(input_forward) && !ch.forward /*&& !ch.forward_stop*/)
                {
                    ch.forward = true;
                    anim.SetFloat("Speed", 1);
                }
                // stop forward
                else if (Input.GetKeyUp(input_forward) /*&& !ch.forward_stop*/)
                {
                    ch.forward = false;
                    anim.SetFloat("Speed", 0);
                }

                // SOCD Neutral
                if (socd_neutral && ch.forward && ch.backward)
                {
                    StopMovement();
                }
            }

            // RPS Tools
            if (tools_usable)
            {
                // attack
                if (Input.GetKeyDown(input_attack) && !ch.attack)
                {
                    actionable = false;
                    StopMovement();
                    anim.SetTrigger("Attack");
                    ch.attack = true;
                }

                // stun
                if (Input.GetKeyDown(input_stun) && !ch.stun)
                {
                    actionable = false;
                    StopMovement();
                    anim.SetTrigger("Stun");
                    ch.stun = true;
                }

                // block
                if (Input.GetKey(input_block) && !ch.block /*&& !ch.block_stop*/)
                {
                    actionable = false;
                    StopMovement();
                    anim.SetBool("Block", true);
                    ch.block = true;
                }
            }
        }

        if (Input.GetKeyUp(input_block) /*&& !ch.block_stop*/)
        {
            anim.SetBool("Block", false);
        }
    }

    // private void ReadMenuInputs()
    // {
    //     // Player has not yet chosen a menu option
    //     if (menu_choice == 0)
    //     {
    //         // Player changes their selected menu option
    //         // "((menu_select - 1) +/- 1)" is kept unsimplified so the following is readable:
    //         //       -1 shift               at the start    enables the modulus operation
    //         //     +/-1 in-/decrementation  in the middle
    //         //       +1 shift               at the end      returns menu_select to the proper value
    //         if (Input.GetKeyDown(input_forward))
    //         {
    //             menu_select = (((menu_select - 1) + 1) % 3) + 1;
    //             menu_select_change = true;
    //         }
    //         if (Input.GetKeyDown(input_forward))
    //         {
    //             menu_select = (((menu_select - 1) - 1) % 3) + 1;
    //             menu_select_change = true;
    //         }

    //         // Player chooses their selected menu option
    //         if (Input.GetKeyDown(input_attack))
    //         {
    //             menu_choice = menu_select;
    //         }
    //     }

    //     // Player has chosen to vote to rematch in scene_select_next, but wants to undo it
    //     else if (/*pm.scene_num == 2 &&*/ menu_choice == 1)
    //     {
    //         menu_choice = -1;
    //     }
    // }

    // private void ReadSelectFighterInputs()
    // {
    //     // Player has not voted to load scene_fight
    //     if (menu_choice == 0)
    //     {
    //         // Player changes their character
    //         if (Input.GetKeyDown(input_forward) || Input.GetKeyDown(input_backward))
    //         {
    //             character_select = (character_select + 1) % 2;
    //             character_select_change = true;
    //         }
    //         if (character_select_change)
    //         {
    //             // Not sure if this works, but would future-proof code for additional characters if so
    //             // Destroy(gameObject.GetComponent<Character>());

    //             // This should probably be moved to Player_Manager.cs
    //             if (character_select == 0)
    //             {
    //                 Debug.Log(player_tag + " selects Spearman");
    //                 Destroy(gameObject.GetComponent<Ninja>());
    //                 ch = gameObject.AddComponent<Spearman>() as Spearman;
    //             }
    //             else // (character_select == 1)
    //             {
    //                 Debug.Log(player_tag + " selects Ninja");
    //                 Destroy(gameObject.GetComponent<Spearman>());
    //                 ch = gameObject.AddComponent<Ninja>() as Ninja;
    //             }

    //             // See comment in the top line of this if statement's scope
    //             // ch = gameObject.GetComponent<Character>();

    //             // Resets lock
    //             character_select_change = false;
    //         }

    //         // Player votes to load scene_fight
    //         if (menu_choice == 0 && Input.GetKeyDown(input_attack))
    //         {
    //             menu_choice = 1;
    //             menu_choice_change = true;
    //         }
    //     }

    //     // Player has voted to load_scene fight, but wants to undo it
    //     else if (/*menu_choice == 1 &&*/ Input.GetKeyDown(input_stun))
    //     {
    //         menu_choice = -1;
    //         menu_choice_change = true;
    //     }

    //     if (Input.GetKey(input_block))
    //     {
    //         HeldQuitInput();
    //     }
    //     else if (Input.GetKeyUp(input_block))
    //     {
    //         quit_timer = 0;
    //     }
    // }

    // private void HeldQuitInput()
    // {
    //     quit_timer += Time.deltaTime;
    //     Debug.Log(player_tag + " quitting out in " + (quit_timer_limit - quit_timer));

    //     if (quit_timer >= quit_timer_limit)
    //     {
    //         menu_choice = 3;
    //         menu_choice_change = true;
    //     }
    // }
}