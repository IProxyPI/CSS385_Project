using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTempCameraControl : MonoBehaviour
{
    public float speed = 25f;
    public Transform transform1;
    public Transform transform2;
    private Vector2 vis_marker;
    // Update is called once per frame
    void Update()
    {   
        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");
        Vector3 averagePosition = (transform1.position + transform2.position) / 2.0f;
        vis_marker = Vector3.Lerp(vis_marker, averagePosition, speed * Time.deltaTime);
        vis_marker.y = 0;
        transform.position = vis_marker;

        //transform.position = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
    }
}
