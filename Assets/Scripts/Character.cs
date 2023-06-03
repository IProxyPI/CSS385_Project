using UnityEngine;
using UnityEngine.SceneManagement;

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
    public GameObject _attack_obj;
    public GameObject _stun_obj;
    public GameObject _block_obj;

    // Statuses
    public bool stunned = false;
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
            step *= -1;
            stagger *= -1;
            boundary_backward *= -1;
            boundary_forward *= -1;
        }
        
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
        // // RPS Tool object reference assignment
        // {
        //     if (sr == null)
        //     {
        //         if (pc.player_tag == "P1" && pc.character_select == 0)
        //         {
        //             sr = Player_Manager.Instance.SpearmanP1.GetComponent<SpriteRenderer>();
        //             _attack_obj = Player_Manager.Instance.SpearmanP1.gameObject.transform.GetChild(0).gameObject;
        //             _stun_obj = Player_Manager.Instance.SpearmanP1.gameObject.transform.GetChild(1).gameObject;
        //             _block_obj = Player_Manager.Instance.SpearmanP1.gameObject.transform.GetChild(2).gameObject;
        //         }
        //         else if (pc.player_tag == "P1" && pc.character_select == 1)
        //         {
        //             Debug.Log("hello"); 
        //             sr = Player_Manager.Instance.NinjaP1.GetComponent<SpriteRenderer>();
        //             _attack_obj = Player_Manager.Instance.NinjaP1.gameObject.transform.GetChild(0).gameObject;
        //             _stun_obj = Player_Manager.Instance.NinjaP1.gameObject.transform.GetChild(1).gameObject;
        //             _block_obj = Player_Manager.Instance.NinjaP1.gameObject.transform.GetChild(2).gameObject;
        //         }
        //         else if (pc.player_tag == "P2" && pc.character_select == 0)
        //         {
        //             sr = Player_Manager.Instance.SpearmanP1.GetComponent<SpriteRenderer>();
        //             _attack_obj = Player_Manager.Instance.SpearmanP2.gameObject.transform.GetChild(0).gameObject;
        //             _stun_obj = Player_Manager.Instance.SpearmanP2.gameObject.transform.GetChild(1).gameObject;
        //             _block_obj = Player_Manager.Instance.SpearmanP2.gameObject.transform.GetChild(2).gameObject;
        //         }
        //         else if (pc.player_tag == "P2" && pc.character_select == 1)
        //         {
        //             sr = Player_Manager.Instance.NinjaP2.GetComponent<SpriteRenderer>();
        //             _attack_obj = Player_Manager.Instance.NinjaP2.gameObject.transform.GetChild(0).gameObject;
        //             _stun_obj = Player_Manager.Instance.NinjaP2.gameObject.transform.GetChild(1).gameObject;
        //             _block_obj = Player_Manager.Instance.NinjaP2.gameObject.transform.GetChild(2).gameObject;
        //         }
        //     }
        // }

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
            if (stunned)
            {
                Toggle_Action(ref stunned, null, false, false);
                pc.rb.position = new Vector3(pc.transform.position.x - stagger, pc.transform.position.y, 0);
                stagger *= -1;
            }

            if (hurt)
            {
                stunned = false;
                // invincibility_timer = 2f;
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
                        pc.tools_usable = true;
                        invincibility_timer = -1;
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
            if (!opc.ch.hurt && !opc.ch.dead)
            {
                pc.actionable = true;
            }
            action = false;

            if (trigger_invincibility)
            {
                pc.actionable = true;
                pc.tools_usable = false;
                // invincibility_timer = 2f;

                opc.actionable = true;
                opc.tools_usable = false;
                // opc.ch.invincibility_timer = 2f;
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
