using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Victory_Display : MonoBehaviour
{
    [SerializeField] private TextMeshPro t;
    
    void Update()
    {
        if (Game_Data.p1_wins == Game_Data.p2_wins)
        {
            t.text = "Round: DRAW";
        }
        else if (Game_Data.p1_wins > Game_Data.p2_wins)
        {
            t.text = "P1 VICTORY";
        }
        else
        {
            t.text = "P2 VICTORY";
        }
    }
}
