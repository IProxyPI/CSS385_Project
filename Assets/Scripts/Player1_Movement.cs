using UnityEngine;

public class Player1_Movement : MonoBehaviour
{
    public float step = 0.05f;
    private Rigidbody2D rb;

    [SerializeField] KeyCode _left = KeyCode.A;
    [SerializeField] KeyCode _right = KeyCode.D;
    [SerializeField] KeyCode _attack = KeyCode.V;
    [SerializeField] KeyCode _stun = KeyCode.B;
    [SerializeField] KeyCode _block = KeyCode.N;
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
                //actionable = false;
                Debug.Log("Attack");
                actionable = _moveset.Action(1);
            }
            if (Input.GetKey(_stun))
            {
                //actionable = false;
                Debug.Log("Stun");
                actionable = _moveset.Action(2);
            }
            if (Input.GetKeyDown(_block))
            {
                //actionable = false;
                Debug.Log("Block");
                _moveset.Action(0);
            }
            if (Input.GetKeyUp(_block))
            {
                
                Debug.Log("Block End");
                actionable = _moveset.Action(-1);
            }
        }
    }
}
