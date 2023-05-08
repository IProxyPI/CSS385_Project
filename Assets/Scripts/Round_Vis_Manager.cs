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
    // update function
}

public class Round_Vis_Manager : MonoBehaviour
{
    
    private int victory_condition = 3; // Round wins required to win the game

    private float freezer = 0;
    
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
        
        bgm.Update_Tree_State( _p1_hp, _p2_hp, Game_Data.p1_wins, Game_Data.p2_wins );
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

    public void Round_Complete( int _winner = 1 )
    {
        if (_winner == 1)
        {
            Game_Data.p2_wins++;
        } 
            else
        {
            Game_Data.p1_wins++;
        }
        
        if (Game_Data.p1_wins < victory_condition && Game_Data.p2_wins < victory_condition)
        {
            SceneManager.LoadScene("1_Fight");
        }
        else
        {
            SceneManager.LoadScene("2_Victory");
        }
    }
        

        // QUICK HACK FOR TESTING
    // delete all this shit later lmao
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            Round_Complete(1);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            Round_Complete(2);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Start_Game();
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            Update_Round_State(0,0);
            Apply_Freezeframe(0.2f);
            GetComponent<AudioSource>().Play();
        }

        Game_Data.freeze_frame = false;
        Time.timeScale = Game_Data.current_timescale;
        if (freezer > 0)
        {
            Game_Data.freeze_frame = true;
            freezer -= Time.fixedDeltaTime;
            Time.timeScale = 0;
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

    public void Apply_Freezeframe( float _time = 0.2f )
    {
        freezer = _time;
    }
}
