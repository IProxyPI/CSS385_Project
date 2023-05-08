using UnityEngine;

public class Character1 : MonoBehaviour
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
    [SerializeField] GameObject _attack_prefab;
    [SerializeField] GameObject _stun_prefab;
    [SerializeField] GameObject _block_prefab;
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
            step = -step;
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

<<<<<<< HEAD
        // Attack
        if (attack > 0)
        {
            if (!attack_instantiated)
            {
                // instantiate attack_prefab
                attack_instantiated = true;
            }
            DefiniteActionCounter(ref attack, attack_end, _attack_prefab, attack_instantiated);
        }

        // Stun
        if (stun > 0)
        {
            if (!stun_instantiated)
            {
                // instantiate stun_prefab
                stun_instantiated = true;
            }
            DefiniteActionCounter(ref stun, stun_end, _stun_prefab, stun_instantiated);
        }

        // Block
        /*
        if (block > 0 || block_stop > 0)
        {
            Debug.Log("Block: " + block + ", " + block_stop);
            if (!block_instantiated)
            {
                // instantiate block_prefab
                block_instantiated = true;
            }
            block_stop = IndefiniteActionStopper(block_stop);
        }
        */
        if (block > 0)
        {
            block = IndefiniteActionCounter(block);
        }
        if (block_stop > 0)
        {
            block_stop = IndefiniteActionStopper(block_stop);
=======
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
>>>>>>> 45b0b6d3f43bb7a44efb4db0e7e5f8ebf0994e43
        }
    }

    private float Endless_Animation_Counter(float action)
    {
        return action += Time.fixedDeltaTime;
    }
    
    private float Endless_Animation_Stopper(float action_stop)
    {
        // if (block_prefab != null)
        // {
        //     // destroy block_prefab
        //     block_instantiated = false;
        // }
        Debug.Log(action_stop);
        if (action_stop >= all_stop_ends)
        {
<<<<<<< HEAD
            pc.actionable = true;
            return 0;
=======
            case_movement = null;
            pc.actionable = true;
            return action_stop = 0;
>>>>>>> 45b0b6d3f43bb7a44efb4db0e7e5f8ebf0994e43
        }
        return action_stop += Time.fixedDeltaTime;
    }

<<<<<<< HEAD
    private void DefiniteActionCounter (ref float action, float action_end, GameObject action_prefab, bool action_instantiated)
=======
    private void Set_Animation_Counter (float action, float action_end, GameObject action_prefab, bool action_instantiated)
>>>>>>> 45b0b6d3f43bb7a44efb4db0e7e5f8ebf0994e43
    {
        if (action > 0)
        {
            action += Time.fixedDeltaTime;
            Debug.Log("P" + pc.player + " has been definitely acting for " + action + " seconds, end = " + action_end);
            if (action >= action_end)
            {
                if (action_prefab != null)
                {
                    // destroy block_prefab
                    action_instantiated = false;
                }
                action = 0;
                Debug.Log("P" + pc.player + " has finished acting");
                pc.actionable = true;
            }
        }
    }
}
