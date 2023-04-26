using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2_Movement : MonoBehaviour
{
    public float step;
    private Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        step = 3.5f;
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            rb.velocity = new Vector2(-step, rb.velocity.y);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            rb.velocity = new Vector2(step, rb.velocity.y);
        }
    }
}
