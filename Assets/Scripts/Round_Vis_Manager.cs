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
}
