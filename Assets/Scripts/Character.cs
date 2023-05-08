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
    [SerializeField] GameObject _attack_obj;
    [SerializeField] GameObject _stun_obj;
    [SerializeField] GameObject _block_obj;
    [SerializeField] Transform _origin;
    private bool attack_active = false;
    private bool stun_active = false;
    private bool block_active = false;

    // Both
    private float all_stop_ends = 0.3f;

    // Start is called before the first frame update
    void Start()
    {

        if (gameObject.name == p1_name)
        {
            pc = GameObject.Find(p1_name).GetComponent<Player_Controller>();
            _attack_obj = GameObject.Find("Attack1");
            _stun_obj = GameObject.Find("Stun1");
            _block_obj = GameObject.Find("Block1");
        }
        else // (gameObject.name == p2_name)
        {
            pc = GameObject.Find(p2_name).GetComponent<Player_Controller>();
            _attack_obj = GameObject.Find("Attack2");
            _stun_obj = GameObject.Find("Stun2");
            _block_obj = GameObject.Find("Block2");
            step = -step;
        }

        // Turn off renderer and collision for moves
        // Block is purely visual as it's a state
        Toggle_Move(_attack_obj, false, false);
        Toggle_Move(_stun_obj, false, false);
        Toggle_Move(_block_obj, false, true);

        // Tells to ignore colliders with own moves
        Physics2D.IgnoreCollision(_attack_obj.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        Physics2D.IgnoreCollision(_stun_obj.GetComponent<Collider2D>(), GetComponent<Collider2D>());
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
                if (!attack_active)
                {
                    // unhide attack
                    attack_active = true;
                    Toggle_Move(_attack_obj, attack_active, false);
                }
                Set_Animation_Counter(ref attack, attack_end, _attack_obj, ref attack_active);
            }

            // stun
            if (stun > 0)
            {
                if (!stun_active)
                {
                    // unhide stun
                    stun_active = true;
                    Toggle_Move(_stun_obj, stun_active, false);
                }
                Set_Animation_Counter(ref stun, stun_end, _stun_obj, ref stun_active);          
            }

            // block
            if (block > 0)
            {
                if (!block_active)
                {
                    // unhide block
                    block_active = true;
                    Toggle_Move(_block_obj, block_active, true);
                }
                Endless_Animation_Counter(ref block);
            }

            // block_stop
            if (block_stop > 0)
            {
                Endless_Animation_Stopper(ref block, ref block_stop);
            }
        }
    }

    private void Endless_Animation_Counter(ref float action)
    {
        action += Time.fixedDeltaTime;
    }

    private void Endless_Animation_Stopper(ref float action, ref float action_stop)
    {
        if (block_active)
        {
            block_active = false;
            Toggle_Move(_block_obj, block_active, true);
        }
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

    private void Set_Animation_Counter(ref float action, float action_end, GameObject action_obj, ref bool action_active)
    {
        if (action > 0)
        {
            action += Time.fixedDeltaTime;
            Debug.Log("P" + pc.player + " has been attacking/stunning for " + action + " seconds, end = " + action_end);
            if (action >= action_end)
            {
                if (action_obj != null)
                {
                    // hide action (attack/stun)
                    action_active = false;
                    Toggle_Move(action_obj, action_active, false);
                }
                action = 0;
                Debug.Log("P" + pc.player + " has finished acting");
                pc.actionable = true;
            }
        }
    }

    private void Toggle_Move(GameObject action, bool state, bool isBlock)
    {
        action.GetComponent<Renderer>().enabled = state;
        if (!isBlock)
        {
            action.GetComponent<Collider2D>().enabled = state;
        }
    }
}
