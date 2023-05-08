using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    private Player_Controller pc;
    private string p1_name = "DummyPlayer1";
    private string p2_name = "DummyPlayer2";

    public string case_movement;
    public float forward = 0;
    public float forward_stop = 0;
    public float backward = 0;
    public float backward_stop = 0;

    public string case_action;
    public float attack = 0;
    public float attack_end = 1;
    public float stun = 0;
    public float stun_end = 1;
    public float block = 0;
    public float block_stop = 0;

    private float all_stop_ends = 0.5f;
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
        switch (case_movement)
        {
            case "forward":
                pc.rb.position = new Vector3(pc.transform.position.x + step, pc.transform.position.y, 0);
                forward = Endless_Animation_Counter(forward);
                break;

            case "forward_stop":
                forward_stop = Endless_Animation_Stopper(forward_stop);
                break;
            
            case "backward":
                pc.rb.position = new Vector3(pc.transform.position.x - step, pc.transform.position.y, 0);
                backward = Endless_Animation_Counter(backward);
                break;
            
            case "backward_stop":
               backward_stop = Endless_Animation_Stopper(backward_stop);
               break;
        }

        switch (case_action)
        {
            case "attack":
                if (!attack_instantiated)
                {
                    // instantiate attack_prefab
                    attack_instantiated = true;
                }
                Set_Animation_Counter(attack, attack_end, attack_prefab, attack_instantiated);
                break;

            case "stun":
                if (!stun_instantiated)
                {
                    // instantiate stun_prefab
                    stun_instantiated = true;
                }
                Set_Animation_Counter(stun, stun_end, stun_prefab, stun_instantiated);          
                break;

            case "block":
                if (!block_instantiated)
                {
                    // instantiate block_prefab
                    block_instantiated = true;
                }
                Endless_Animation_Counter(block);
                break;

            case "block_stop":
                block_stop = Endless_Animation_Stopper(block_stop);
                break;
        }
    }

    private float Endless_Animation_Counter(float action)
    {
        return action += Time.deltaTime;
    }
    
    private float Endless_Animation_Stopper(float action_stop)
    {
        // if (block_prefab != null)
        // {
        //     // destroy block_prefab
        //     block_instantiated = false;
        // }
        if (action_stop >= all_stop_ends)
        {
            case_movement = null;
            pc.actionable = true;
            return action_stop = 0;
        }
        return action_stop += Time.deltaTime;
    }

    private void Set_Animation_Counter (float action, float action_end, GameObject action_prefab, bool action_instantiated)
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
