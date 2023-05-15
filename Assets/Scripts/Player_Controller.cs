using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    // Used in Start(), scene_select_fighter, scene_fight's pause, and/or scene_select_next
    private Player_Manager pm;
    public Character ch;
    public Rigidbody2D rb;
    public BoxCollider2D bc;
    public int facing_dir = 1;                      // 1 = left side facing right; -1 = right side facing left
    private int character_select = 0;               // 0 = Spearman, 1 = Ninja
    private bool character_select_change = false;
    public int menu_select = 0;                     // 0 = scene_fight, 1 = scene_select_fighter, 2 = quit application
    public bool menu_select_change = false;
    public int menu_choice = 0;                     // 0 = no change
    public bool menu_choice_change = false;
    private float quit_timer = 0;
    private int quit_timer_limit = 3;

    // Used in scene_fight
    public string player_tag = "P1";
    public string opponent_tag = "P2";
    public int lives = 2;
    public bool paused = false;
    public bool actionable = true;
    public bool socd_neutral = false;               // False = L/R -> L, True = L/R -> N (Facing R)
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
        ch = gameObject.AddComponent<Spearman>() as Spearman;
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
            ReadSelectFighterInputs();
        }
    }

    // Called by ReadFightInputs()
    private void StopMovement()
    {
        ch.forward = 0;
        ch.forward_stop = 0;
        ch.backward = 0;
        ch.backward_stop = 0;
    }
    
    // Attacked or Stunned
    private void OnTriggerEnter2D(Collider2D col)
    {
        Debug.Log(col.name + " collided with " + player_tag);
        if (col.tag == opponent_tag)
        {
            if (col.name.Contains("Attack"))
            {
                ch.hurt = Time.fixedDeltaTime;
                lives--;
            }
            else // (col.name.Contains("Stun"))
            {
                ch.stun_end = Time.fixedDeltaTime;
            }
        }
    }

    private void ReadFightInputs()
    {
        // Pause
        if (Input.GetKeyDown(input_pause))
        {
            Debug.Log(player_tag + " paused");
            // Initialize pause menu prefab
            paused = true;
        }

        if (actionable)
        {
            // Movement

            // move forward
            if (Input.GetKey(input_forward) && ch.forward == 0 && ch.forward_stop == 0)
            {
                ch.forward = Time.fixedDeltaTime;
            }
            // stop forward
            else if (Input.GetKeyUp(input_forward) && ch.forward_stop == 0)
            {
                ch.forward_stop = Time.fixedDeltaTime;
                ch.forward = 0;
            }
            // move backward
            if (Input.GetKey(input_backward) && ch.backward == 0 && ch.backward_stop == 0)
            {
                ch.backward = Time.fixedDeltaTime;
            }
            // stop backward
            else if (Input.GetKeyUp(input_backward) && ch.backward_stop == 0)
            {
                ch.backward_stop = Time.fixedDeltaTime;
                ch.backward = 0;
            }

            // SOCD Neutral
            if (socd_neutral && ch.forward > 0 && ch.backward > 0)
            {
                Debug.Log("Neutral");
                StopMovement();
            }

            // Actions

            // attack
            if (Input.GetKeyDown(input_attack) && ch.attack == 0)
            {
                actionable = false;
                StopMovement();
                ch.attack = Time.fixedDeltaTime;
            }

            // stun
            if (Input.GetKeyDown(input_stun) && ch.stun == 0)
            {
                actionable = false;
                StopMovement();
                ch.stun = Time.fixedDeltaTime;
            }

            // block
            if (Input.GetKey(input_block) && ch.block == 0 && ch.block_stop == 0)
            {
                actionable = false;
                StopMovement();
                ch.block = Time.fixedDeltaTime;
            }
        }

        if (Input.GetKeyUp(input_block) && ch.block_stop == 0)
        {
            ch.block_stop = Time.fixedDeltaTime;
            ch.block = 0;
        }
    }

    private void ReadMenuInputs()
    {
        // Player has not yet chosen a menu option
        if (menu_choice == 0)
        {
            // Player changes their selected menu option
            // "((menu_select - 1) +/- 1)" is kept unsimplified so the following is readable:
            //       -1 shift               at the start    enables the modulus operation
            //     +/-1 in-/decrementation  in the middle
            //       +1 shift               at the end      returns menu_select to the proper value
            if (Input.GetKeyDown(input_forward))
            {
                menu_select = (((menu_select - 1) + 1) % 3) + 1;
                menu_select_change = true;
            }
            if (Input.GetKeyDown(input_forward))
            {
                menu_select = (((menu_select - 1) - 1) % 3) + 1;
                menu_select_change = true;
            }

            // Player chooses their selected menu option
            if (Input.GetKeyDown(input_attack))
            {
                menu_choice = menu_select;
            }
        }

        // Player has chosen to vote to rematch in scene_select_next, but wants to undo it
        else if (/*pm.scene_num == 2 &&*/ menu_choice == 1)
        {
            menu_choice = -1;
        }
    }

    private void ReadSelectFighterInputs()
    {
        // Player has not voted to load scene_fight
        if (menu_choice == 0)
        {
            // Player changes their character
            if (Input.GetKeyDown(input_forward) || Input.GetKeyDown(input_backward))
            {
                character_select = (character_select + 1) % 2;
                character_select_change = true;
            }
            if (character_select_change)
            {
                // Not sure if this works, but would future-proof code for additional characters if so
                // Destroy(gameObject.GetComponent<Character>());

                // This should probably be moved to Player_Manager.cs
                if (character_select == 0)
                {
                    Debug.Log(player_tag + " selects Spearman");
                    Destroy(gameObject.GetComponent<Ninja>());
                    ch = gameObject.AddComponent<Spearman>() as Spearman;
                }
                else // (character_select == 1)
                {
                    Debug.Log(player_tag + " selects Ninja");
                    Destroy(gameObject.GetComponent<Spearman>());
                    ch = gameObject.AddComponent<Ninja>() as Ninja;
                }

                // See comment in the top line of this if statement's scope
                // ch = gameObject.GetComponent<Character>();

                // Resets lock
                character_select_change = false;
            }

            // Player votes to load scene_fight
            if (menu_choice == 0 && Input.GetKeyDown(input_attack))
            {
                menu_choice = 1;
                menu_choice_change = true;
            }
        }

        // Player has voted to load_scene fight, but wants to undo it
        else if (/*menu_choice == 1 &&*/ Input.GetKeyDown(input_stun))
        {
            menu_choice = -1;
            menu_choice_change = true;
        }

        if (Input.GetKey(input_block))
        {
            HeldQuitInput();
        }
        else if (Input.GetKeyUp(input_block))
        {
            quit_timer = 0;
        }
    }

    private void HeldQuitInput()
    {
        quit_timer += Time.deltaTime;
        Debug.Log(player_tag + " quitting out in " + (quit_timer_limit - quit_timer));

        if (quit_timer >= quit_timer_limit)
        {
            menu_choice = 3;
            menu_choice_change = true;
        }
    }
}