using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background_Manager : MonoBehaviour
{
    public Transform vis_goal;
    private Vector2 vis_marker;
    
    public Transform[] layers;
    
    private float x_mod = -0.025f;
    private float y_mod = -0.015f;

    private float compress_factor = 1f;
    
    private float lerp_speed = 2.5f;
    public void Awake()
    {
    }

    public void Update()
    {
        vis_marker = Vector3.Lerp(vis_marker, vis_goal.position, lerp_speed * Time.deltaTime);
        for (int i = 0; i < layers.Length; i++)
        {
            var _x = (1 + i) * x_mod * vis_marker.x * compress_factor;
            var _y = (1 + i) * y_mod * vis_marker.y * compress_factor;
            Vector3 v = new Vector3(_x,_y,0);
            layers[i].transform.position = v;
        }
    }
}
