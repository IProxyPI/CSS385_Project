using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Game_Data
{
    public static int cur_round = 0; // The current round of play - first round = round 0, increment it for each win
    
    public static int p1_wins = 0;
    public static int p2_wins = 0;
    
    public static float current_timescale = 1f;
    public static bool freeze_frame = false; // Doesnt do anything yet, but ideally we should slap this little guy into every
    
}

public class Round_Vis_Manager : MonoBehaviour
{
    
    private int victory_condition = 3; // Round wins required to win the game

    private float freezer = 0;
    private float end_of_round_countdown = 2f;
    private bool round_over = false;

    private int p1hp = 2;
    private int p2hp = 2;
    
    [SerializeField] private Background_Manager bgm;
        
    // Resets all round stats, updates background visuals to represent previous rounds, etc.
    // Call this at the start of EVERY NEW ROUND
    public void New_Round()
    {
        
        bgm.Reset_Tree_State(Game_Data.p1_wins, Game_Data.p2_wins);
    }

    public void Start()
    {
        New_Round();
    }
    
    // Just pass in the new values of player hp, and the visuals will be updated to match
    public void Update_Round_State( int _p1_hp, int _p2_hp )
    {
        p1hp = _p1_hp;
        p2hp = _p2_hp;
        if (!round_over && Game_Data.p1_wins < victory_condition && Game_Data.p2_wins < victory_condition) {
            bgm.Update_Tree_State( p1hp, p2hp, Game_Data.p1_wins, Game_Data.p2_wins );
        }
        if (p1hp <= 0)
        {
            Round_Complete(1);
        }
        else if (p2hp <= 0)
        {
            Round_Complete(2);
        }
    }

    // Resets all round data and loads the combat scene
    public void Start_Game()
    {
        Game_Data.p1_wins = 0;
        Game_Data.p2_wins = 0;
        Game_Data.cur_round = 0;
        SceneManager.LoadScene("1_Fight");
    }
    
    // Im just gonna start building some game flow logic here

    public void Round_Complete( int _winner )
    {
        if (!round_over) {
            if (_winner == 1)
            {
                Game_Data.p2_wins++;
            } 
                else
            {
                Game_Data.p1_wins++;
            }

            round_over = true;
            Apply_Freezeframe();
        }
    }
    
    public void Update()
    {

        Game_Data.freeze_frame = false;
        Time.timeScale = Game_Data.current_timescale;
        if (freezer > 0)
        {
            Game_Data.freeze_frame = true;
            freezer -= Time.fixedDeltaTime;
            Time.timeScale = 0;
        }

        if (round_over)
        {
            end_of_round_countdown -= Time.deltaTime;
            if (end_of_round_countdown <= 0)
            {
                if (Game_Data.p1_wins < victory_condition && Game_Data.p2_wins < victory_condition)
                {
                    print("Loading fight");
                    SceneManager.LoadScene("1_Fight");
                }
                else
                {
                    print("loading victory");
                    SceneManager.LoadScene("2_Victory");
                }
            }
        }
    }
    

    // Getter and setter methods for stats
    public int Get_P1_Wins()
    {
        return Game_Data.p1_wins;
    }
    public int Get_P2_Wins()
    {
        return Game_Data.p2_wins;
    }
    public int Get_Cur_Round()
    {
        return Game_Data.cur_round;
    }
    public void Set_P1_Wins( int _p1_wins )
    {
        Game_Data.p1_wins = _p1_wins;
    }
    public void Set_P2_Wins( int _p2_wins )
    {
        Game_Data.p2_wins = _p2_wins;
    }
    public void Set_Cur_Round( int _cur_round )
    {
        Game_Data.cur_round = _cur_round;
    }

    public void Apply_Freezeframe( float _time = 0.25f )
    {
        freezer = _time;
        GetComponent<AudioSource>().Play();
    }
}
