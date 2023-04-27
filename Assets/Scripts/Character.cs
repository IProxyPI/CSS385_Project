using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int move_forward = 0;
    public int move_backward = 0;
    public int forward_stop = 0;
    public int backward_stop = 0;
    public int attack = 0;
    public int stun = 0;
    public int block = 0;
    public int block_stop = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // didn't these debug.logs to work, tired.  prob a simple fix idk
        if (move_forward > 0)
        {
            Debug.Log("Moving forward");
            move_forward = 0;
        }
        if (move_backward > 0)
        {
            Debug.Log("Moving backward");
            move_backward = 0;
        }
        if (forward_stop > 0)
        {
            Debug.Log("Forward stop");
            forward_stop = 0;
        }
        if (backward_stop > 0)
        {
            Debug.Log("Backward stop");
            backward_stop = 0;
        }
        if (attack > 0)
        {
            Debug.Log("Attacked");
            attack = 0;
        }
        if (stun > 0)
        {
            Debug.Log("Used stun");
            stun = 0;
        }
        if (block > 0)
        {
            Debug.Log("Blocking");
            block = 0;
        }
        if (block_stop > 0)
        {
            Debug.Log("Block stop");
            block_stop = 0;
        }
    }
}
