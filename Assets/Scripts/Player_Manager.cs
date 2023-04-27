using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    public int game_state = 0;  // 0 = Character Select, 1 = Fight, 2 = Post-match
    private Player_Controller p1;
    private Player_Controller p2;

    void Start()
    {
        p1 = GameObject.Find("DummyPlayer1").GetComponent<Player_Controller>();
        p2 = GameObject.Find("DummyPlayer2").GetComponent<Player_Controller>();
    }

    void Update()
    {
        if (game_state == 0 && p1.menu_choice == 1 && p2.menu_choice == 1)
        {
            Debug.Log("Fight!");
            game_state = 1;
        }
        // continue if/else tree for other game state transitions

        // Load scene "0" or "1" or "2"
    }
}
