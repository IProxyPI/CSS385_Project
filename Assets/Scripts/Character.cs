using UnityEngine;

public class Character : MonoBehaviour
{
    private Player_Controller pc;
    [SerializeField] private string p1_name = "DummyPlayer1";
    [SerializeField] private string p2_name = "DummyPlayer2";

    // Movement
    public float forward = 0;
    public float forward_stop = 0;
    public float backward = 0;
    public float backward_stop = 0;
    private float step = 0.1f;

    // RPS Tools
    public float attack = 0;
    public float attack_end = 0.5f;
    public float stun = 0;
    public float stun_end = 1f;
    public float block = 0;
    public float block_stop = 0;
    [SerializeField] GameObject _attack_prefab;
    [SerializeField] GameObject _stun_prefab;
    [SerializeField] GameObject _block_prefab;
    private bool attack_instantiated = false;
    private bool stun_instantiated = false;
    private bool block_instantiated = false;

    // Both
    private float all_stop_ends = 0.5f;

    // Status
    public float stunned = 0;
    public float stunned_end = 2;
    public float fall = 0;
    public int fall_end = 2;

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
        // Movement
        {
            // forward
            if (forward > 0)
            {
                Action_Counter(ref forward, -1);
                pc.rb.position = new Vector3(pc.transform.position.x + step, pc.transform.position.y, 0);
            }
            
            // backward
            if (backward > 0)
            {
                Action_Counter(ref backward, -1);
                pc.rb.position = new Vector3(pc.transform.position.x - step, pc.transform.position.y, 0);
            }

            // forward_stop
            if (forward_stop > 0)
            {
                forward = 0;
                Action_Counter(ref forward_stop, all_stop_ends);
            }

            // backward_stop
            if (backward_stop > 0)
            {
                backward = 0;
                Action_Counter(ref backward_stop, all_stop_ends);
            }
        }

        // RPS Tools
        {
            // attack
            if (attack > 0)
            {
                Action_Counter(ref attack, attack_end);
                Tool_Handler(ref attack, attack, attack_end, _attack_prefab, ref attack_instantiated);          
            }

            // stun
            if (stun > 0)
            {
                Action_Counter(ref stun, stun_end);
                Tool_Handler(ref stun, stun, stun_end, _stun_prefab, ref stun_instantiated);          
            }

            // block
            if (block > 0)
            {
                Action_Counter(ref block, -1);
                Tool_Handler(ref block, block_stop, all_stop_ends, _block_prefab, ref block_instantiated);
            }

            // block_stop
            if (block_stop > 0) {
                block = 0;
                Action_Counter(ref block_stop, all_stop_ends);
                Tool_Handler(ref block, block_stop, all_stop_ends, _block_prefab, ref block_instantiated);
            }
        }

        // Status
        {
            if (stunned > 0)
            {
                Action_Counter(ref stunned, stunned_end);
            }

            if (fall > 0)
            {
                Action_Counter(ref fall, fall_end);
            }
        }
            
    }

    private void Action_Counter(ref float action, float action_end)
    {
        action += Time.fixedDeltaTime;
        if (action_end != -1 && action >= action_end)
        {
            action = 0;
            pc.actionable = true;
        }
    }

    private void Tool_Handler(ref float action, float action_stop, float action_end, GameObject action_prefab, ref bool tool_instantiated)
    {
        if (!tool_instantiated)
        {
            // instantiate _prefab
            tool_instantiated = true;
        }
        
        if (action_stop >= action_end)
        {
            if (action_prefab != null)
            {
                // destroy prefab
                tool_instantiated = false;
            }
            action = 0;
            Debug.Log("P" + pc.player + " has finished acting");
            pc.actionable = true;
        }
    }
}
