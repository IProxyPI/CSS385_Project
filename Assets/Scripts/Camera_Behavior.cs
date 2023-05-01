using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Behavior : MonoBehaviour
{
    public GameObject Player1, Player2;
    public GameObject ChaseCamera;
    private Vector3 position;

    public float speed = 0.005f;
    // Start is called before the first frame update
    void Start()
    {
        position = transform.position;
        position.z = -10;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 velocity = Vector3.zero;
        // if (Mathf.Abs(Player1.transform.position.x - Player2.transform.position.x) > 7)
        // {
        position.x = (Player1.transform.position.x + Player2.transform.position.x) / 2;

        // }
        //transform.position = Vector3.SmoothDamp(transform.position, position, ref velocity, .4f);
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }
}
