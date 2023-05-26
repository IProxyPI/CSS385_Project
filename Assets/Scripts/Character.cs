using UnityEngine;

public class Character : MonoBehaviour
{
    private Player_Controller pc;
    private string p1_name = "DummyPlayer1";
    private string p2_name = "DummyPlayer2";

    // Movement
    public bool forward = false;
    public bool backward = false;
    private float step = 0.1f;

    // RPS Tools
    public bool attack = false;
    public bool stun = false;
    public bool block = false;
    [SerializeField] private GameObject _attack_obj;
    [SerializeField] private GameObject _stun_obj;
    [SerializeField] private GameObject _block_obj;
    [SerializeField] private Transform _origin;
    private bool attack_active = false;
    private bool stun_active = false;
    private bool block_active = false;

    // Statuses
    public bool stunned = false;
    public bool hurt = false;

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
        Toggle_Tool(_attack_obj, false, false);
        Toggle_Tool(_stun_obj, false, false);
        Toggle_Tool(_block_obj, false, true);

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
            if (forward)
            {
                pc.rb.position = new Vector3(pc.transform.position.x + step, pc.transform.position.y, 0);
            }
            
            // backward
            if (backward)
            {
                pc.rb.position = new Vector3(pc.transform.position.x - step, pc.transform.position.y, 0);
            }
        }

        // RPS Tools
        {
            // attack
            if (attack)
            {
                // if hidden
                if (!_attack_obj.GetComponent<Renderer>().enabled)
                {
                    // unhide
                    _attack_obj.GetComponent<Renderer>().enabled = true;
                }
                
                if (pc.anim.GetCurrentAnimatorStateInfo(1).IsTag("Idle"))
                {
                    attack = false;
                    _attack_obj.GetComponent<Renderer>().enabled = false;
                }
            }

            // stun
            if (stun)
            {
                // if hidden
                if (!_stun_obj.GetComponent<Renderer>().enabled)
                {
                    // unhide
                    _stun_obj.GetComponent<Renderer>().enabled = true;
                }
                
                if (pc.anim.GetCurrentAnimatorStateInfo(1).IsTag("Idle"))
                {
                    stun = false;
                    _stun_obj.GetComponent<Renderer>().enabled = false;
                }
            }

            // block
            if (block)
            {
                // if hidden
                if (!_block_obj.GetComponent<Renderer>().enabled)
                {
                    // unhide
                    _block_obj.GetComponent<Renderer>().enabled = true;
                }
            }
            if (!block)
            {
                // if not hidden
                if (_block_obj.GetComponent<Renderer>().enabled)
                {
                    // hide
                    _block_obj.GetComponent<Renderer>().enabled = false;
                }
            }
        }
    }

    private void Tool_Handler(bool action, GameObject tool_obj, ref bool tool_instantiated)
    {
        // if stopping tool use
        {
            if (block_active)
            {
                block_active = false;
                Toggle_Tool(_block_obj, block_active, true);
            }
            // if animation ends
            {
                if (tool_obj != null)
                {
                    tool_instantiated = false;
                    Toggle_Tool(tool_obj, tool_instantiated, false);
                }
                action = false;
                pc.actionable = true;
            }
        }
    }

    private void Toggle_Tool(GameObject action, bool state, bool isBlock)
    {
        action.GetComponent<Renderer>().enabled = state;
        if (!isBlock)
        {
            action.GetComponent<BoxCollider2D>().enabled = state;
        }
    }
}
