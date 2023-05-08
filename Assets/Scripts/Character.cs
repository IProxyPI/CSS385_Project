using UnityEngine;

public class Character : MonoBehaviour
{
    private Player_Controller pc;
    private string p1_name = "DummyPlayer1";
    private string p2_name = "DummyPlayer2";

    // Movement
    public float forward = 0;
    public float forward_stop = 0;
    public float backward = 0;
    public float backward_stop = 0;
    private float step = 0.1f;

    // Actions
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
                pc.rb.position = new Vector3(pc.transform.position.x + step, pc.transform.position.y, 0);
                Endless_Animation_Counter(ref forward);
            }
            
            // backward
            if (backward > 0)
            {
                pc.rb.position = new Vector3(pc.transform.position.x - step, pc.transform.position.y, 0);
                Endless_Animation_Counter(ref backward);
            }

            // forward_stop
            if (forward_stop > 0)
            {
                Endless_Animation_Stopper(ref forward, ref forward_stop);
            }

            // backward_stop
            if (backward_stop > 0)
            {
                Endless_Animation_Stopper(ref backward, ref backward_stop);
            }
        }

        // Actions
        {
            // attack
            if (attack > 0)
            {
                if (!attack_instantiated)
                {
                    // instantiate attack_prefab
                    attack_instantiated = true;
                }
                Set_Animation_Counter(ref attack, attack_end, _attack_prefab, attack_instantiated);
            }

            // stun
            if (stun > 0)
            {
                if (!stun_instantiated)
                {
                    // instantiate stun_prefab
                    stun_instantiated = true;
                }
                Set_Animation_Counter(ref stun, stun_end, _stun_prefab, stun_instantiated);          
            }

            // block
            if (block > 0)
            {
                if (!block_instantiated)
                {
                    // instantiate block_prefab
                    block_instantiated = true;
                }
                Endless_Animation_Counter(ref block);
            }

            // block_stop
            if (block_stop > 0) {
                Endless_Animation_Stopper(ref block, ref block_stop);
            }
        }

        // Status
        {
            if (stunned > 0)
            {
                Set_Animation_Counter(ref stunned, ref stunned_end);
            }

            if (fall > 0)
            {
                Set_Animation_Counter(ref fall, ref fall_end);
            }
        }
            
    }

    private void Endless_Animation_Counter(ref float action)
    {
        action += Time.fixedDeltaTime;
    }

    private void Endless_Animation_Stopper(ref float action, ref float action_stop)
    {
        // if (block_prefab != null)
        // {
        //     // destroy block_prefab
        //     block_instantiated = false;
        // }
        if (action_stop >= all_stop_ends)
        {
            action_stop = 0;
            pc.actionable = true;
        }
        else
        {
            action = 0;
            action_stop += Time.fixedDeltaTime;
        }
    }

    private void Set_Animation_Counter(ref float action, float action_end, GameObject action_prefab, bool action_instantiated)
    {
        if (action > 0)
        {
            action += Time.fixedDeltaTime;
            // Debug.Log("P" + pc.player + " has been attacking/stunning/hurt for " + action + " seconds, end = " + action_end);
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
