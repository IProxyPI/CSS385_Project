using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehavior : MonoBehaviour
{
    
    private Player_Controller pc;

    public Sprite healthy = null;
    public Sprite low_hp = null;
    public Sprite dead = null;

    public ParticleSystem lose_leaves;
    public ParticleSystem tree_dies;
    public void Awake()
    {
        set_state(2);
    }

    public void Start()
    {
        // pc = ...        
    }

    public void Update()
    {
        // set_state(pc.lives - 1);
    }
    public void set_state(int _state)
    {
        state = _state;
        if (state == 2)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = healthy;
        }
        else if (state == 1)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = low_hp;
        }
        else if (state == 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = dead;
        }
    }

    public void Trigger_Particles(int p)
    {
        if (p == 1)
        {
            lose_leaves.Play();
        }
        else
        {
            tree_dies.Play();
        }
    }
}
