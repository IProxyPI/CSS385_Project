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
    public bool unlocked = false;
    public bool attack = false;
    public bool stun = false;
    public bool block = false;
    [SerializeField] private GameObject _attack_obj;
    [SerializeField] private GameObject _stun_obj;
    [SerializeField] private GameObject _block_obj;

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

        // Unnecessary?
        {
            // // Ensures all RPS tool renderers are disabled
            // _attack_obj.GetComponent<Renderer>().enabled = false;
            // _stun_obj.GetComponent<Renderer>().enabled = false;
            // _block_obj.GetComponent<Renderer>().enabled = false;

            // // Tells to ignore colliders with own moves
            // Physics2D.IgnoreCollision(_attack_obj.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            // Physics2D.IgnoreCollision(_stun_obj.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        {
            if (forward)
            {
                pc.rb.position = new Vector3(pc.transform.position.x + step, pc.transform.position.y, 0);
            }
            
            if (backward)
            {
                pc.rb.position = new Vector3(pc.transform.position.x - step, pc.transform.position.y, 0);
            }
        }

        // RPS Tools
        {
            if (attack)
            {
                Toggle_Tool(ref attack, _attack_obj);
            }

            if (stun)
            {
                Toggle_Tool(ref stun, _stun_obj);
            }

            if (block)
            {
                Toggle_Tool(ref block, _block_obj);
            }
        }
    }

    private void Toggle_Tool(ref bool tool, GameObject tool_obj)
    {
        // if hidden
        if (!tool_obj.GetComponent<Renderer>().enabled)
        {
            // unhide
            tool_obj.GetComponent<Renderer>().enabled = true;
        }
        
        // if unlocked && RPS tool animation ends
        if (unlocked && pc.anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
        {
            // reset all variables associated with the RPS tools
            unlocked = false;
            tool = false;
            tool_obj.GetComponent<Renderer>().enabled = false;
            pc.actionable = true;
        }
        // if non-Idle animation (e.g. RPS tool animation) has started
        else if (!pc.anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle"))
        {
            // unlock
            unlocked = true;
        }
    }
}
