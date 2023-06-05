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

    public static AudioSource track_1;
    public static AudioSource track_2;
    public static AudioSource track_3;
    public static bool audio_set_up = false;
}

public class Round_Vis_Manager : MonoBehaviour
{
    
    private int victory_condition = 3; // Round wins required to win the game

    private float freezer = 0;
    private bool round_over = false;
    public float end_of_round_countdown = 2f;
    public bool round_loading;

    private int p1hp = 2;
    private int p2hp = 2;
    
    public AudioSource track_1;
    public AudioSource track_2;
    public AudioSource track_3;
    
    [SerializeField] private Background_Manager bgm;
        
    // Resets all round stats, updates background visuals to represent previous rounds, etc.
    // Call this at the start of EVERY NEW ROUND
    public void New_Round()
    {
        
        bgm.Reset_Tree_State(Game_Data.p1_wins, Game_Data.p2_wins);
    }

    public void Start()
    {

        Game_Data.track_1 = track_1;
        Game_Data.track_2 = track_2;
        Game_Data.track_3 = track_3;
        Game_Data.track_1.Play();
        Game_Data.track_2.Play();
        Game_Data.track_3.Play();
        Game_Data.audio_set_up = true;

        New_Round();

        if (Game_Data.p1_wins == victory_condition - 1 || Game_Data.p2_wins == victory_condition - 1)
        {
            Manage_Music(3);
        } else
        {
            Manage_Music(1);
        }
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

        if (p1hp == 2 && _p2_hp == 2)
        {
            Manage_Music(1);
        }
        else
        {
            Manage_Music(2);
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

    public void Manage_Music( int phase )
    {
        Game_Data.track_1.volume = 0;
        Game_Data.track_2.volume = 0;
        Game_Data.track_3.volume = 0;
        switch (phase)
        {
            case(1): Game_Data.track_1.volume = 1; break;
            case(2): Game_Data.track_2.volume = 1; break;
            case(3): Game_Data.track_3.volume = 1; break;
            default: break;
        }
        
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
                    Player_Manager.Instance.p1.anim.SetTrigger("Exit");
                    Player_Manager.Instance.p2.anim.SetTrigger("Exit");
                    Player_Manager.Instance.p1.ch.dead = false;
                    Player_Manager.Instance.p2.ch.dead = false;
                    Player_Manager.Instance.p1.ch.invincibility_timer = 0;
                    Player_Manager.Instance.p2.ch.invincibility_timer = 0;
                    Player_Manager.Instance.p1.rb.position = new Vector3(-5, Player_Manager.Instance.p1.transform.position.y, 0);
                    Player_Manager.Instance.p2.rb.position = new Vector3( 5, Player_Manager.Instance.p2.transform.position.y, 0);
                    Player_Manager.Instance.p1.actionable = true;
                    Player_Manager.Instance.p2.actionable = true;
                    SceneManager.LoadScene("1_Fight");
                }
                else
                {
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

    public void Apply_Freezeframe( float _time = 0.5f )
    {
        freezer = _time;
        GetComponent<AudioSource>().Play();
    }
}
