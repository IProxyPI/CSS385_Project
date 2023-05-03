using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round_Vis_Manager : MonoBehaviour
{

    private int cur_round = 0; // The current round of play - first round = round 0, increment it for each win
    
    private int p1_wins = 0;
    private int p2_wins = 0;

    private int victory_condition = 3; // Round wins required to win the game
    
    [SerializeField] private Background_Manager bgm;
        
    // Resets all round stats, updates background visuals to represent previous rounds, etc.
    // Call this at the start of EVERY NEW ROUND
    public void New_Round()
    {
        
        bgm.Reset_Tree_State(p1_wins, p2_wins);
    }
    
    // Just pass in the new values of player hp, and the visuals will be updated to match
    public void Update_Round_State( int _p1_hp, int _p2_hp )
    {
        
        bgm.Update_Tree_State( _p1_hp, _p2_hp, p1_wins, p2_wins );
    }

    // Full game reset
    public void New_Game()
    {
        p1_wins = 0;
        p2_wins = 0;
        cur_round = 0;
    }

    private int p1hp = 2;
    private int p2hp = 2;
    
    // QUICK HACK FOR TESTING
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            p1hp--;
            Update_Round_State(p1hp, p2hp);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            p2hp--;
            Update_Round_State(p1hp, p2hp);
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            p1hp = 2;
            p2hp = 2;
            p1_wins = 0;
            p2_wins = 0;
            New_Round();
        }
        if (p1hp <= 0)
        {
            p1hp = 2;
            p2hp = 2;
            p2_wins++;
            Update_Round_State(p1hp, p2hp);
        }
        if (p2hp <= 0)
        {
            
            p1hp = 2;
            p2hp = 2;
            p1_wins++;
            Update_Round_State(p1hp, p2hp);
        }
    }

    // Getter and setter methods for stats
    public int Get_P1_Wins()
    {
        return p1_wins;
    }
    public int Get_P2_Wins()
    {
        return p2_wins;
    }
    public int Get_Cur_Round()
    {
        return cur_round;
    }
    public void Set_P1_Wins( int _p1_wins )
    {
        p1_wins = _p1_wins;
    }
    public void Set_P2_Wins( int _p2_wins )
    {
        p2_wins = _p2_wins;
    }
    public void Set_Cur_Round( int _cur_round )
    {
        cur_round = _cur_round;
    }
}
