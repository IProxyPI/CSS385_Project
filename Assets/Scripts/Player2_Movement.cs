using UnityEngine;

public class Player2_Movement : MonoBehaviour
{
    public float step = 0.1f;
    private Rigidbody2D rb;

    [SerializeField] KeyCode _left = KeyCode.LeftArrow;
    [SerializeField] KeyCode _right = KeyCode.RightArrow;
    [SerializeField] KeyCode _attack = KeyCode.Keypad1;
    [SerializeField] KeyCode _stun = KeyCode.Keypad2;
    [SerializeField] KeyCode _block = KeyCode.Keypad3;
    private bool actionable;

    [SerializeField] private Fighter _moveset;

    // Start is called before the first frame update
    void Start()
    {
        step = 0.1f;
        rb = GetComponent<Rigidbody2D>();
        actionable = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (actionable)
        {
            if (Input.GetKey(_left))
            {
                rb.position = new Vector3(transform.position.x - step, transform.position.y, 0);
            }
            if (Input.GetKey(_right))
            {
                rb.position = new Vector3(transform.position.x + step, transform.position.y, 0);
            }
            if (Input.GetKey(_attack))
            {
                actionable = false;
            }
            if (Input.GetKey(_stun))
            {
                actionable = false;
            }
            if (Input.GetKeyDown(_block))
            {
                actionable = false;
            }
            if (Input.GetKeyUp(_block))
            {
                actionable = true;
            }
        }
    }
}
