using UnityEngine;

public class Player_Controller : MonoBehaviour
{
    // Used in Start(), scene_select_fighter, scene_fight's pause, and/or scene_select_next
    private Player_Manager pm;
    public Rigidbody2D rb;
    public int player = 1;                          // Used in Debug.Log()'s
    public int facing_dir = 1;                      // 1 = left side facing right; -1 = right side facing left
    public Character ch;
    private int character_select = 0;               // 0 = Spearman, 1 = Ninja
    private bool character_select_change = false;
    public int menu_select = 0;                     // 0 = scene_fight, 1 = scene_select_fighter, 2 = quit application
    public bool menu_select_change = false;
    public int menu_choice = 0;                     // 0 = no change
    public bool menu_choice_change = false;
    private float quit_timer = 0;
    private int quit_timer_limit = 3;

    // Used in scene_fight
    public int lives = 3;
    public bool paused = false;
    public bool actionable = true;
    public bool hurt = false;
    private string input_pause = "space";
    private string input_forward = "s";
    private string input_backward = "a";
    private string input_attack = "z";
    private string input_stun = "x";
    private string input_block = "c";

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
            input_pause = "enter";
            input_forward = "j";
            input_backward = "k";
            input_attack = "m";
            input_stun = ",";
            input_block = ".";
        }
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

    // No object has a trigger or tags set to true yet since I forget how those work
    private void OnTriggerEnter(Collider other)
    {
        // ch.kneel and/or ch.die = 1; maybe?
        hurt = true;
    }

    private void ReadFightInputs()
    {
        // Pause
        if (Input.GetKeyDown(input_pause))
        {
            Debug.Log("P" + player + " paused");
            // Initialize pause menu prefab
            paused = true;
        }

        if (actionable)
        {
            // Movement
            if (Input.GetKey(input_forward) && ch.forward == 0 && ch.forward_stop == 0)
            {
                ch.forward = Time.fixedDeltaTime;
            }
            else if (Input.GetKeyUp(input_forward) && ch.forward_stop == 0)
            {
                ch.forward_stop = Time.fixedDeltaTime;
                ch.forward = 0;
            }

            if (Input.GetKey(input_backward) && ch.backward == 0 && ch.backward_stop == 0)
            {
                ch.backward = Time.fixedDeltaTime;
            }
            else if (Input.GetKeyUp(input_backward) && ch.backward_stop == 0)
            {
                ch.backward_stop = Time.fixedDeltaTime;
                ch.backward = 0;
            }

            // Actions
            if (Input.GetKeyDown(input_attack) && ch.attack == 0)
            {
                actionable = false;
                StopMovement();
                ch.attack = Time.fixedDeltaTime;  // changed from 0.  represents frame 1 of move, which can be incremented to count what happens each frame in ch?
            }
            
            if (Input.GetKeyDown(input_stun) && ch.stun == 0)
            {
                actionable = false;
                StopMovement();
                ch.stun = Time.fixedDeltaTime;
            }
            
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

    private void StopMovement()
    {
        ch.forward = 0;
        ch.forward_stop = 0;
        ch.backward = 0;
        ch.backward_stop = 0;
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
                    Debug.Log("P" + (player) + " selects Spearman");
                    Destroy(gameObject.GetComponent<Ninja>());
                    ch = gameObject.AddComponent<Spearman>() as Spearman;
                }
                else // (character_select == 1)
                {
                    Debug.Log("P" + (player) + " selects Ninja");
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
        Debug.Log("P" + player + " quitting out in " + (quit_timer_limit - quit_timer));

        if (quit_timer >= quit_timer_limit)
        {
            menu_choice = 3;
            menu_choice_change = true;
        }
    }
}