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
    public bool hurt = false;
    public bool stunned = false;

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
            // Already disabled by default in all scenes
            // // Ensures all RPS action renderers are disabled
            // _attack_obj.GetComponent<Renderer>().enabled = false;
            // _stun_obj.GetComponent<Renderer>().enabled = false;
            // _block_obj.GetComponent<Renderer>().enabled = false;

            // Tags already prevent players colliding with their own moves
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
            if (forward && pc.actionable)
            {
                pc.rb.position = new Vector3(pc.transform.position.x + step, pc.transform.position.y, 0);
            }
            
            if (backward && pc.actionable)
            {
                pc.rb.position = new Vector3(pc.transform.position.x - step, pc.transform.position.y, 0);
            }
        }

        // RPS Tools
        {
            if (attack)
            {
                Toggle_Action(ref attack, _attack_obj, true);
            }

            if (stun)
            {
                Toggle_Action(ref stun, _stun_obj, true);
            }

            if (block)
            {
                Toggle_Action(ref block, _block_obj, false);
            }
        }

        // Statuses
        {
            if (hurt)
            {
                Toggle_Action(ref hurt, null, false);
            }

            if (stunned)
            {
                Toggle_Action(ref stunned, null, false);
            }
        }
    }

    private void Toggle_Action(ref bool action, GameObject tool_obj, bool has_collisions)
    {
        // if hidden
        if (tool_obj != null && !tool_obj.GetComponent<Renderer>().enabled)
        {
            // unhide
            tool_obj.GetComponent<Renderer>().enabled = true;
            if (has_collisions)
            {
                tool_obj.GetComponent<BoxCollider2D>().enabled = true;
            }
        }
        
        // if unlocked && RPS action animation ends
        if (unlocked && pc.anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle/Walk"))
        {
            // reset all variables associated with the RPS tools
            unlocked = false;
            if (tool_obj != null)
            {
                tool_obj.GetComponent<Renderer>().enabled = false;
            }
            if (has_collisions)
            {
                tool_obj.GetComponent<BoxCollider2D>().enabled = false;
            }
            action = false;
            pc.actionable = true;
        }
        // if non-Idle/Walk animation (e.g. RPS action animation) has started
        else if (!pc.anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle/Walk"))
        {
            // unlock
            unlocked = true;
        }
    }
}
