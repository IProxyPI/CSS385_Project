using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Behavior : MonoBehaviour
{
    public GameObject Player1, Player2;
    public GameObject ChaseCamera;
    private Vector3 position;

    private float speed = 3.5f;
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
        position.x = (Player1.transform.position.x + Player2.transform.position.x) / 2;
        transform.position = Vector3.Lerp(transform.position, position, speed * Time.deltaTime);
    }
}
