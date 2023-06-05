using UnityEngine;
using UnityEngine.SceneManagement;

public class Character : MonoBehaviour
{
    private Player_Controller pc;
    public Player_Controller opc;  // opponent pc
    public SpriteRenderer sr;
    private string p1_name = "DummyPlayer1";
    private string p2_name = "DummyPlayer2";

    // Movement
    public bool forward = false;
    public bool backward = false;
    public float step = 0f;
    public float dash = 0f; 
    private float stagger = -0.2f;
    private float stagger_forward = 0.1f;
    private float stagger_backward = -0.2f;
    private float boundary_backward = -10f;
    private float boundary_forward = 8.4f;  // .1f more than push distance to avoid clipping
    public float friction = 0.5f;

    // RPS Tools
    public float unlocked = -1f;
    public bool attack = false;
    public bool stun = false;
    public bool block = false;
    public GameObject _attack_obj;
    public GameObject _stun_obj;
    public GameObject _block_obj;

    // Statuses
    public bool stunned = false;
    private bool stunned_unchecked = false;
    public bool hurt = false;
    public float invincibility_timer = -1f;
    private float invincibility_lock = 0.05f;
    public bool dead = false;

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
            stagger_forward *= -1;
            stagger_backward *= -1;
            boundary_backward *= -1;
            boundary_forward *= -1;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (unlocked > 0)
        {
            unlocked -= Time.deltaTime;
        }
        else
        {
            unlocked = -1;
        }

        // Movement
        {
            // forward
            if (forward && pc.actionable)
            {
                Move(pc.facing_dir, step, boundary_forward);
            }
            
            // backward
            if (backward && pc.actionable)
            {
                Move(-pc.facing_dir, -step, boundary_backward);
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
            if (stunned)
            {
                stunned_unchecked = true;
                block = false;
                Toggle_Action(ref stunned, _block_obj, false, false);
                
                pc.rb.position = new Vector3(pc.transform.position.x + stagger, pc.transform.position.y, 0);
                if (stagger == stagger_backward)
                {
                    stagger = stagger_forward;
                }
                else
                {
                    stagger = stagger_backward;
                };
            }
            else if (stunned_unchecked)
            {
                if ((pc.player_tag == "P1" && pc.transform.position.x < boundary_backward)
                  || pc.player_tag == "P2" && pc.transform.position.x > boundary_backward)
                {
                    pc.rb.position = new Vector3(boundary_backward, pc.transform.position.y, 0);
                }
            }

            if (hurt)
            {
                stunned = false;
                Toggle_Action(ref hurt, null, false, true);
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
                    }
                }
                else
                {
                    if (invincibility_timer > -1)
                    {
                        invincibility_timer = -1;
                        pc.tools_usable = true;
                    }
                    if (!sr.enabled)
                    {
                        sr.enabled = !sr.enabled;
                    }
                }
            }

            if (dead)
            {
                stunned = false;
            }
        }
    }

    private void Move(int direction, float distance, float boundary)
    {
        // if distance goes beyond boundary
        if (direction > 0 && (pc.rb.position.x + distance > boundary)
         || direction < 0 && (pc.rb.position.x + distance < boundary))
        {
            // reduce distance to boundary limit
            float res = (boundary - pc.rb.position.x) / direction;

            // prevents funny glitch; comment out all but "distance = res" and back up to left boundary to see
            if (Mathf.Abs(distance) > Mathf.Abs(res))
            {
                distance = res * direction;
            }
            else
            {
                distance = 0;
            }
        }

        // if not pushing opponent forward
        if ((pc.player_tag == "P1" && pc.rb.position.x + distance < opc.rb.position.x - 1.5)
         || (pc.player_tag == "P2" && pc.rb.position.x + distance > opc.rb.position.x + 1.5))
        {
            // move at regular speed
            pc.rb.position = new Vector3(pc.transform.position.x + distance, pc.transform.position.y, 0);
        }

        // else if opponent isn't also moving forward 
        else if (!backward)
        {
            float distanceToOpp = (opc.transform.position.x - (1.5f * direction)) - pc.transform.position.x;
            distance -= distanceToOpp;
            if (opc.ch.forward)
            {
                distance = 0;
            }

            // move at regular speed until opponent's position is reached, then push them against 'friction' if they're not already equalizing push
            pc.rb.position  = new Vector3(pc.transform.position.x  + distanceToOpp + (distance * friction), pc.transform.position.y,  0);
            opc.rb.position = new Vector3(opc.transform.position.x + (distance * friction), opc.transform.position.y, 0);
        }
    }

    private void Toggle_Action(ref bool action, GameObject tool_obj, bool has_collisions, bool trigger_invincibility)
    {
        // if action has an associated RPS tool object
        if (tool_obj != null)
        {
            // if it is hidden and animation is starting block or entering attack/stun active frames
            if (!tool_obj.GetComponent<Renderer>().enabled
             && (pc.anim.GetCurrentAnimatorStateInfo(0).IsTag("Block")
             || (pc.anim.GetCurrentAnimatorStateInfo(0).IsTag("Active") && !stunned)))
            {
                // unhide it
                tool_obj.GetComponent<Renderer>().enabled = true;

                // if it has collisions
                if (has_collisions)
                {
                    // enable them
                    tool_obj.GetComponent<BoxCollider2D>().enabled = true;

                    // dash forward
                    Move(pc.facing_dir, dash, boundary_forward);
                }
            }

            // else if unlocked (see below) and animation is ending block or entering attack/stun endlag
            else if (unlocked > 0 
                  && (pc.anim.GetCurrentAnimatorStateInfo(0).IsTag("AnimEnder")
                   || pc.anim.GetCurrentAnimatorStateInfo(0).IsTag("Endlag"))
                  || pc.anim.GetCurrentAnimatorStateInfo(0).IsTag("Effect")
                  || pc.anim.GetCurrentAnimatorStateInfo(0).IsTag("Dead"))
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
        if (unlocked > 0 && pc.anim.GetCurrentAnimatorStateInfo(0).IsTag("AnimEnder"))
        {
            // reset variables associated with the action
            if (!opc.ch.hurt && !opc.ch.dead)
            {
                pc.actionable = true;
            }
            action = false;

            if (trigger_invincibility)
            {
                pc.actionable = true;
                pc.tools_usable = false;

                opc.actionable = true;
                opc.tools_usable = false;
            }
        }
        // if any non-Idle/Walk animation has started
        else if (action && !pc.anim.GetCurrentAnimatorStateInfo(0).IsTag("AnimEnder"))
        {
            // unlock
            unlocked = 2f;
        }
    }
}
