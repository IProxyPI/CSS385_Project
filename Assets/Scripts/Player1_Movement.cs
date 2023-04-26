using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1_Movement : MonoBehaviour
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
        if (Input.GetKey(KeyCode.W))
        {
        }
        if (Input.GetKey(KeyCode.S))
        {
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.velocity = new Vector2(step, rb.velocity.y);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.velocity = new Vector2(-step, rb.velocity.y);
        }
    }
}
