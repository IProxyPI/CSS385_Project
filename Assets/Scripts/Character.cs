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
        // Set Player_Controller reference
        if (gameObject.name == p1_name)
        {
            pc = GameObject.Find(p1_name).GetComponent<Player_Controller>();
        }
        else // (gameObject.name == p2_name)
        {
            pc = GameObject.Find(p2_name).GetComponent<Player_Controller>();
            step = -step;
        }
        
        // Set RPS Tool object references
        if (pc.character_select == 0) // Spearman
        {
            _attack_obj = GameObject.Find("SpearmanAttack" + pc.player_tag);
            _stun_obj = GameObject.Find("SpearmanStun" + pc.player_tag);
            _block_obj = GameObject.Find("SpearmanBlock" + pc.player_tag);
        }
        else // (pc.character_select == 1) Ninja
        {
            _attack_obj = GameObject.Find("NinjaAttack" + pc.player_tag);
            _stun_obj = GameObject.Find("NinjaStun" + pc.player_tag);
            _block_obj = GameObject.Find("NinjaBlock" + pc.player_tag);
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
        // if action has an associated RPS tool object
        if (tool_obj != null)
        {
            // if it is hidden and animation is starting block or entering attack/stun active frames
            if (!tool_obj.GetComponent<Renderer>().enabled
             && (pc.anim.GetCurrentAnimatorStateInfo(0).IsTag(null)
              || pc.anim.GetCurrentAnimatorStateInfo(0).IsTag("Active")))
            {
                // unhide it
                tool_obj.GetComponent<Renderer>().enabled = true;

                // if it has collisions
                if (has_collisions)
                {
                    // enable them
                    tool_obj.GetComponent<BoxCollider2D>().enabled = true;
                }
            }

            // else if unlocked (see below) and animation is ending block or entering attack/stun endlag
            else if (unlocked 
                  && (pc.anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle/Walk")
                   || pc.anim.GetCurrentAnimatorStateInfo(0).IsTag("Endlag")))
            {
                // hide it
                tool_obj.GetComponent<Renderer>().enabled = false;

                // if it has collisions
                if (has_collisions)
                {
                    // disable them
                    tool_obj.GetComponent<BoxCollider2D>().enabled = false;
                }
            }
        }

        // if unlocked (see below) && the action animation ends
        if (unlocked && pc.anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle/Walk"))
        {
            // reset variables associated with the action
            unlocked = false;
            action = false;
            pc.actionable = true;
        }
        // if any non-Idle/Walk animation has started
        else if (!pc.anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle/Walk"))
        {
            // unlock
            unlocked = true;
        }
    }
}
