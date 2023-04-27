using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehavior : MonoBehaviour
{
    
    private int state = 2; // 2 = full hp, 1 = 1 hp, 0 = dead

    public Sprite healthy = null;
    public Sprite low_hp = null;
    public Sprite dead = null;

    public void Awake()
    {
        set_state(2);
    }

    public void Update()
    {
        set_state(state);
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
}
