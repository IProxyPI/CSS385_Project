using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dev_Mouse_Controller : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;
    }
}
