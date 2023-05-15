using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPS_Tool : MonoBehaviour
{
    private BoxCollider2D bc;

    [SerializeField] private string p1_action = "1";
    // [SerializeField] private string p1_action = "2";
    
    void Start()
    {
        // Assigns tag as P1 or P2 based on object name
        // If we try to rework them as prefabs again, so assign positions based on script stuff
        //  instead of through the Inspector window, we can maintain vthisv code by looking into
        //  assigning names on instantiation, and using `int Player_Controller.player`
        if (gameObject.name.Contains(p1_action))
        {
            gameObject.tag = "P1";
        }
        else // (gameObject.name.Contains(p2_action))
        {
            gameObject.tag = "P2";
        }

        // Assigns Collider2D component if an Attack or Stun object
        if (!gameObject.name.Contains("Block"))
        {
            bc = GetComponent<BoxCollider2D>();
            // bc.isTrigger = true;
        }
    }
}
