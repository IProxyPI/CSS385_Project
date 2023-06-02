using UnityEngine;

public class Character : MonoBehaviour
{
    private Player_Controller pc;
    public Player_Controller opc;  // opponent pc
    public SpriteRenderer sr;
    private string p1_name = "DummyPlayer1";
    private string p2_name = "DummyPlayer2";
    public GameObject DummyPlayer1, DummyPlayer2;

    // Movement
    public bool forward = false;
    public bool backward = false;
    private float step = 0.1f;
    private float stagger = 0.1f;
    private float boundary_backward = -10f;
    private float boundary_forward = 8.5f;
    public float friction = 0.5f;

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
    public float invincibility_timer = 0f;
    private float invincibility_lock = 0.05f;
    public bool roundStarted = false;

    // Start is called before the first frame update
    void Start()
    {
        // Set Player_Controller reference
        if (gameObject.name == p1_name)
        {
            pc = GameObject.Find(p1_name).GetComponent<Player_Controller>();
            opc = GameObject.Find(p2_name).GetComponent<Player_Controller>();
        }
        else // (gameObject.name == p2_name)
        {
            pc = GameObject.Find(p2_name).GetComponent<Player_Controller>();
            opc = GameObject.Find(p1_name).GetComponent<Player_Controller>();
            step *= -1;
            stagger *= -1;
            boundary_backward *= -1;
            boundary_forward *= -1;
        }
        
        // Set RPS Tool object references
        // if (DummyPlayer1.GetComponent<Spearman>().enabled) // Spearman
        // {
        //     sr = GameObject.Find("Spearman" + pc.player_tag).GetComponent<SpriteRenderer>();
        //     _attack_obj = GameObject.Find("SpearmanAttack" + pc.player_tag);
        //     _stun_obj = GameObject.Find("SpearmanStun" + pc.player_tag);
        //     _block_obj = GameObject.Find("SpearmanBlock" + pc.player_tag);
        // }
        // else if(DummyPlayer1.GetComponent<Ninja>().enabled) // Ninja
        // {
        //     sr = GameObject.Find("Ninja" + pc.player_tag).GetComponent<SpriteRenderer>();
        //     _attack_obj = GameObject.Find("NinjaAttack" + pc.player_tag);
        //     _stun_obj = GameObject.Find("NinjaStun" + pc.player_tag);
        //     _block_obj = GameObject.Find("NinjaBlock" + pc.player_tag);
        // }

        // Unnecessary?
        {
            // Already disabled by default in all scenes
            // // Ensures all RPS action renderers are disabled
            // _attack_obj.GetComponent<Renderer>().enabled = false;
            // _stun_obj.GetComponent<Renderer>().enabled = false;
            // _block_obj.GetComponent<Renderer>().enabled = false;

            // Tags already prevent players friction with their own moves
            // // Tells to ignore colliders with own moves
            // Physics2D.IgnoreCollision(_attack_obj.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            // Physics2D.IgnoreCollision(_stun_obj.GetComponent<Collider2D>(), GetComponent<Collider2D>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (DummyPlayer1.GetComponent<Spearman>().enabled) // Spearman
        {
            sr = GameObject.Find("Spearman" + pc.player_tag).GetComponent<SpriteRenderer>();
            _attack_obj = GameObject.Find("SpearmanAttack" + pc.player_tag);
            _stun_obj = GameObject.Find("SpearmanStun" + pc.player_tag);
            _block_obj = GameObject.Find("SpearmanBlock" + pc.player_tag);
        }
        else if(DummyPlayer1.GetComponent<Ninja>().enabled) // Ninja
        {
            sr = GameObject.Find("Ninja" + pc.player_tag).GetComponent<SpriteRenderer>();
            _attack_obj = GameObject.Find("NinjaAttack" + pc.player_tag);
            _stun_obj = GameObject.Find("NinjaStun" + pc.player_tag);
            _block_obj = GameObject.Find("NinjaBlock" + pc.player_tag);
        }
        // Movement
        {
            // forward
            if (forward && pc.actionable
             && (pc.player_tag == "P1" && pc.rb.position.x + step <= boundary_forward
              || pc.player_tag == "P2" && pc.rb.position.x + step >= boundary_forward))
            {
                // if not pushing opponent
                if (pc.player_tag == "P1" && pc.rb.position.x + step < opc.rb.position.x - 1.5
                 || pc.player_tag == "P2" && pc.rb.position.x + step > opc.rb.position.x + 1.5)
                {
                    pc.rb.position = new Vector3(pc.transform.position.x + step, pc.transform.position.y, 0);
                }
                
                // else if opponent isn't also moving forward 
                else if (!opc.ch.forward)
                {
                    pc.rb.position = new Vector3(pc.transform.position.x + (step * friction), pc.transform.position.y, 0);
                    opc.rb.position = new Vector3(opc.transform.position.x + (step * friction), opc.transform.position.y, 0);
                }
            }
            
            // backward
            if (backward && pc.actionable
             && (pc.player_tag == "P1" && pc.rb.position.x - step >= boundary_backward
              || pc.player_tag == "P2" && pc.rb.position.x - step <= boundary_backward))
            {
                pc.rb.position = new Vector3(pc.transform.position.x - step, pc.transform.position.y, 0);
            }
        }

        // RPS Tools
        {
            if (attack)
            {
                Toggle_Action(ref attack, _attack_obj, true, false);
            }

            if (stun)
            {
                Toggle_Action(ref stun, _stun_obj, true, false);
            }

            if (block)
            {
                Toggle_Action(ref block, _block_obj, false, false);
            }
        }

        // Statuses
        {
            if (hurt)
            {
                stunned = false;
                Toggle_Action(ref hurt, null, false, true);
            }

            if (stunned)
            {
                Toggle_Action(ref stunned, null, false, false);
                pc.rb.position = new Vector3(pc.transform.position.x - stagger, pc.transform.position.y, 0);
                stagger *= -1;
            }

            if (sr != null)
            {
                if (invincibility_timer > 0)
                {
                    invincibility_timer -= Time.deltaTime;
                    invincibility_lock -= Time.deltaTime;
                    
                    if (invincibility_lock <= 0)
                    {
                        sr.enabled = !sr.enabled;
                        invincibility_lock = 0.05f;
                        Debug.Log(sr.enabled);
                    }
                }
                else if (!sr.enabled)
                {
                    invincibility_timer = 0;
                    sr.enabled = !sr.enabled;
                }
            }
        }
    }

    private void Toggle_Action(ref bool action, GameObject tool_obj, bool has_collisions, bool trigger_invincibility)
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

                    // dash forward
                    pc.rb.position = new Vector3(pc.transform.position.x + (4 * step), pc.transform.position.y, 0);
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

            if (trigger_invincibility)
            {
                invincibility_timer = 2f;
                opc.ch.invincibility_timer = 2f;
            }
        }
        // if any non-Idle/Walk animation has started
        else if (!pc.anim.GetCurrentAnimatorStateInfo(0).IsTag("Idle/Walk"))
        {
            // unlock
            unlocked = true;
        }
    }
}
