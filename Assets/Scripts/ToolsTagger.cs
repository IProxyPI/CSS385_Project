using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolsTagger : MonoBehaviour
{
    private BoxCollider2D bc;

    [SerializeField] private string p1_action = "";
    //[SerializeField] private string p2_action = "";
    
    void Start()
    {
        if (gameObject.name == p1_action)
        {
            gameObject.tag = "P1";
        }
        else // (gameObject.name == p2_action)
        {
            gameObject.tag = "P2";
        }

        bc = GetComponent<BoxCollider2D>();
        bc.isTrigger = true;
    }
}
