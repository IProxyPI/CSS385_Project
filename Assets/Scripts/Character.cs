using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Player_Controller pc;
    private string p1_name = "DummyPlayer1";
    private string p2_name = "DummyPlayer2";

    public float forward = 0;
    public float forward_stop = 0;
    public float backward = 0;
    public float backward_stop = 0;
    public float attack = 0;
    public float attack_end = 3;
    public float stun = 0;
    public float stun_end = 3;
    public float block = 0;
    public float block_stop = 0;
    public float all_stop_ends = 0.5f;

    private float step = 0.1f;
    private GameObject attack_prefab;
    private GameObject stun_prefab;
    private GameObject block_prefab;
    private bool attack_instantiated = false;
    private bool stun_instantiated = false;
    private bool block_instantiated = false;

    // Start is called before the first frame update
    void Start()
    {
        if (gameObject.name == p1_name)
        {
            pc = GameObject.Find(p1_name).GetComponent<Player_Controller>();
        }
        else // (gameObject.name == p2_name)
        {
            pc = GameObject.Find(p2_name).GetComponent<Player_Controller>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move forward
        if (forward > 0)
        {
            pc.rb.position = new Vector3(pc.transform.position.x + step, pc.transform.position.y, 0);
            forward = IndefiniteActionCounter(forward);
        }
        if (forward_stop > 0)
        {
            forward_stop = IndefiniteActionStopper(forward_stop);
        }

        // Move backward
        if (backward > 0)
        {
            pc.rb.position = new Vector3(pc.transform.position.x - step, pc.transform.position.y, 0);
            backward = IndefiniteActionCounter(backward);
        }
        if (backward_stop > 0)
        {
            backward_stop = IndefiniteActionStopper(backward_stop);
        }

        // Attack
        if (attack > 0)
        {
            if (!attack_instantiated)
            {
                // instantiate attack_prefab
                attack_instantiated = true;
            }
            DefiniteActionCounter(attack, attack_end, attack_prefab, attack_instantiated);
        }

        // Stun
        if (stun > 0)
        {
            if (!stun_instantiated)
            {
                // instantiate stun_prefab
                stun_instantiated = true;
            }
            DefiniteActionCounter(stun, stun_end, stun_prefab, stun_instantiated);
        }

        // Block
        if (block > 0 || block_stop > 0)
        {
            if (!block_instantiated)
            {
                // instantiate block_prefab
                block_instantiated = true;
            }
            IndefiniteActionStopper(block_stop);
        }
    }

    private float IndefiniteActionCounter(float action)
    {
        return action += Time.deltaTime;
    }
    
    private float IndefiniteActionStopper(float action_stop)
    {
        // if (block_prefab != null)
        // {
        //     // destroy block_prefab
        //     block_instantiated = false;
        // }
        if (action_stop >= all_stop_ends)
        {
            return action_stop = 0;
            pc.actionable = true;
        }
        return action_stop += Time.deltaTime;
    }

    private void DefiniteActionCounter (float action, float action_end, GameObject action_prefab, bool action_instantiated)
    {
        if (action > 0)
        {
            Debug.Log("P" + pc.player + " has been definitely acting for " + action + " seconds");
            action += Time.deltaTime;
            if (action >= action_end)
            {
                if (action_prefab != null)
                {
                    // destroy block_prefab
                    action_instantiated = false;
                }
                action = 0;
                pc.actionable = true;
            }
        }
    }
}
