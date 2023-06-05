using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Manager : MonoBehaviour
{
    // Scenes
    [SerializeField] private string scene_0 = "0_Select_Fighter";
    [SerializeField] private string scene_1 = "1_Fight";
    [SerializeField] private string scene_2 = "2_Victory";
    private Scene scene;
    // public int scene_num = 0;

    // Background / Round Visuals
    [SerializeField] private string bgm_name = "Background_Manager";
    private Round_Vis_Manager rvm;
    
    // Players
    [SerializeField] private string p1_name = "DummyPlayer1";
    [SerializeField] private string p2_name = "DummyPlayer2";
    public Player_Controller p1;
    public Player_Controller p2;
    private int players_ready = 0;

    // Character Selection
    public GameObject NinjaP1;
    public GameObject NinjaP2;
    public GameObject SpearmanP1;
    public GameObject SpearmanP2;

    public static Player_Manager Instance;

    void Start()
    {
        // // Update scene_num if starting from a different scene for dev debug
        // // (UPON COMPLETION: REMOVE if, else if, THEIR CONTENTS, AND else.
        // //                   LEAVE else'S CONTENTS.)
        // Scene scene = SceneManager.GetActiveScene();
        // if (scene.name == scene_1)
        // {
        //     scene_num = 1;
        // }
        // else if (scene.name == scene_2)
        // {
        //     scene_num = 2;
        //     Destroy(gameObject);
        // }
        // else if (scene.name == scene_0)
        // {
        //     scene_num = 0;
        //     Instance = this;
        //     DontDestroyOnLoad(gameObject);
        //     Debug.Log("SELECT YOUR FIGHTER!");
        // }
        
        // // Store reference to background manager's round visual manager
        // rvm = GameObject.Find(bgm_name).GetComponent<Round_Vis_Manager>();

        // Store references to both player scripts
        p1 = GameObject.Find(p1_name).GetComponent<Player_Controller>();
        p2 = GameObject.Find(p2_name).GetComponent<Player_Controller>();

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        scene = SceneManager.GetActiveScene();
        if (scene.name == scene_1)
        {
            if (rvm == null)
            {
                rvm = GameObject.Find(bgm_name).GetComponent<Round_Vis_Manager>();
            }
        }
        else if (scene.name == scene_2)
        {
            SpearmanP1.SetActive(false);
            SpearmanP2.SetActive(false);
            NinjaP1.SetActive(false);
            NinjaP2.SetActive(false);
        }

        // Possible in scene_fight
        if (p1.ch.hurt || p1.ch.dead || p2.ch.hurt || p2.ch.dead)
        {
            rvm.Update_Round_State(p1.lives, p2.lives);
            if (p1.ch.dead || p2.ch.dead)
            {
                p1.lives = 2;
                p2.lives = 2;
            }
            // if (p1.lives == 0 || p2.lives == 0)
            // {
            //     rvm = null;
            // }
        }

        if (p1.staggered_opp)
        {
            p2.anim.SetTrigger("Stunned");
            p2.ch.staggering = true;
            p1.staggered_opp = false;
        }
        if (p2.staggered_opp)
        {
            p1.anim.SetTrigger("Stunned");
            p1.ch.staggering = true;
            p2.staggered_opp = false;
        }

        // // Possible in scene_select_fighter, scene_fight pause, or scene_select_next
        // if (p1.menu_select_change)
        // {
        //     MenuSelectOutcome(p1);
        // }
        // if (p1.menu_choice_change)
        // {
        //     MenuChoiceOutcome(p1);
        // }
        // if (p2.menu_select_change)
        // {
        //     MenuSelectOutcome(p2);
        // }
        // if (p2.menu_choice_change)
        // {
        //     MenuChoiceOutcome(p2);
        // }
    }

    // private void HurtOutcome(Player_Controller p)
    // {
    //     p.lives--;
    //     // Tree fall animation

    //     if (p.lives > 0)
    //     {
    //         // Load next round
    //     }
    //     else 
    //     {
    //         scene_num = 2;
    //         SceneManager.LoadScene(scene_2, LoadSceneMode.Single);
    //     }
    // }

    // private void MenuSelectOutcome(Player_Controller p)
    // {
    //     // // p moves their selection forward
    //     // animation stuff

    //     // // p moves their selection backward
    //     // animation stuff
    // }

    // private void MenuChoiceOutcome(Player_Controller p)
    // {
    //     // p chooses to stay on the current scene
    //     if (p.menu_choice == -1)
    //     {
    //         // p undoes their vote to load scene_fight
    //         Debug.Log(p.player_tag + " undid ready");
    //         players_ready--;
    //         p.menu_choice = 0;
    //     }

    //     // p chooses to load or stay on scene_fight
    //     if (p.menu_choice == 1)
    //     {
    //         // p votes to load scene_fight
    //         if (scene_num == 0 || scene_num == 2)
    //         {
    //             Debug.Log(p.player_tag + " ready!");
    //             players_ready++;
    //             if (players_ready == 2)
    //             {
    //                 // Load scene_fight
    //                 Debug.Log("FIGHT!");
    //                 scene_num = 1;
    //                 SceneManager.LoadScene(scene_1, LoadSceneMode.Single);

    //                 // Reset counter
    //                 players_ready = 0;
    //             }
    //         }
            
    //         // p, as the pauser, wants to resume
    //         else // (scene_num == 1)
    //         {
    //             // Resume scene_fight
    //             Debug.Log(p.player_tag + " unpaused");
    //             // Destroy pause menu prefab
    //             p.paused = false;
    //             p.menu_select = 0;
    //             p.menu_choice = 0;
    //         }
    //     }

    //     // p chooses to load scene_select_fighter
    //     else if (p.menu_choice == 2)
    //     {
    //         // Load scene_select_fighter (from either other scene)
    //         Debug.Log("SELECT YOUR FIGHTER!");
    //         scene_num = 2;
    //         SceneManager.LoadScene(scene_0, LoadSceneMode.Single);
    //         p.menu_select = 0;
    //         p.menu_choice = 0;
    //     }

    //     // p chooses to quit out of the application
    //     else if (p.menu_choice == 3)
    //     {
    //         Debug.Log("BUH-BYE!");
    //         Application.Quit();
    //     }

    //     // Reset counter
    //     p.menu_choice_change = false;
    // }
}
