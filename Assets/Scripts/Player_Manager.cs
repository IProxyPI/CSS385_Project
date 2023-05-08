using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Manager : MonoBehaviour
{
    [SerializeField] private string scene_0 = "0_Select_Fighter";
    [SerializeField] private string scene_1 = "1_Fight";
    [SerializeField] private string scene_2 = "2_Select_Next";
    public int scene_num = 0;
    private Player_Controller p1;
    private Player_Controller p2;
    [SerializeField] private string p1_name = "DummyPlayer1";
    [SerializeField] private string p2_name = "DummyPlayer2";
    private int players_ready = 0;

    void Start()
    {
        // Update scene_num if starting from a different scene for dev debug
        // (UPON COMPLETION: REMOVE if, else if, THEIR CONTENTS, AND else.
        //                   LEAVE else'S CONTENTS.)
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name == scene_1)
        {
            scene_num = 1;
        }
        else if (scene.name == scene_2)
        {
            scene_num = 2;
        }
        else // (scene.name = scene_select_fighter)
        {
            Debug.Log("SELECT YOUR FIGHTER!");
        }
        
        // Store pointers to both players
        p1 = GameObject.Find(p1_name).GetComponent<Player_Controller>();
        p2 = GameObject.Find(p2_name).GetComponent<Player_Controller>();
    }

    void Update()
    {
        // Possible in scene_fight
        if (p1.hurt)
        {
            HurtOutcome(p1);
        }
        if (p2.hurt)
        {
            HurtOutcome(p2);
        }

        // Possible in scene_select_fighter, scene_fight pause, or scene_select_next
        if (p1.menu_select_change)
        {
            MenuSelectOutcome(p1);
        }
        if (p1.menu_choice_change)
        {
            MenuChoiceOutcome(p1);
        }
        if (p2.menu_select_change)
        {
            MenuSelectOutcome(p2);
        }
        if (p2.menu_choice_change)
        {
            MenuChoiceOutcome(p2);
        }
    }

    private void HurtOutcome(Player_Controller p)
    {
        p.lives--;
        // Tree fall animation

        if (p.lives > 0)
        {
            // Load next round
        }
        else 
        {
            scene_num = 2;
            SceneManager.LoadScene(scene_2, LoadSceneMode.Single);
        }
    }

    private void MenuSelectOutcome(Player_Controller p)
    {
        // // p moves their selection forward
        // animation stuff

        // // p moves their selection backward
        // animation stuff
    }

    private void MenuChoiceOutcome(Player_Controller p)
    {
        // p chooses to stay on the current scene
        if (p.menu_choice == -1)
        {
            // p undoes their vote to load scene_fight
            Debug.Log("P" + p.player + " undid ready");
            players_ready--;
            p.menu_choice = 0;
        }

        // p chooses to load or stay on scene_fight
        if (p.menu_choice == 1)
        {
            // p votes to load scene_fight
            if (scene_num == 0 || scene_num == 2)
            {
                Debug.Log("P" + p.player + " ready!");
                players_ready++;
                if (players_ready == 2)
                {
                    // Load scene_fight
                    Debug.Log("FIGHT!");
                    scene_num = 1;
                    SceneManager.LoadScene(scene_1, LoadSceneMode.Single);

                    // Reset counter
                    players_ready = 0;
                }
            }
            
            // p, as the pauser, wants to resume
            else // (scene_num == 1)
            {
                // Resume scene_fight
                Debug.Log("P" + p.player + " unpaused");
                // Destroy pause menu prefab
                p.paused = false;
                p.menu_select = 0;
                p.menu_choice = 0;
            }
        }

        // p chooses to load scene_select_fighter
        else if (p.menu_choice == 2)
        {
            // Load scene_select_fighter (from either other scene)
            Debug.Log("SELECT YOUR FIGHTER!");
            scene_num = 2;
            SceneManager.LoadScene(scene_0, LoadSceneMode.Single);
            p.menu_select = 0;
            p.menu_choice = 0;
        }

        // p chooses to quit out of the application
        else if (p.menu_choice == 3)
        {
            Debug.Log("BUH-BYE!");
            Application.Quit();
        }

        // Reset counter
        p.menu_choice_change = false;
    }
}
