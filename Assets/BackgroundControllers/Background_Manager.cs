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
    
    [SerializeField] private List<TreeBehavior> p1_trees;
    [SerializeField] private List<TreeBehavior> p2_trees;
    [SerializeField] private Round_Vis_Manager RVM;
    private int prev_p1_hp = 0;
    private int prev_p2_hp = 0;
    public void Awake()
    {
    }

    public void Update()
    {
        //vis_marker = Vector3.Lerp(vis_marker, vis_goal.position, lerp_speed * Time.deltaTime);
        vis_marker = vis_goal.position;
        for (int i = 0; i < layers.Length; i++)
        {
            var _x = (1 + i) * x_mod * vis_marker.x * compress_factor;
            var _y = (1 + i) * y_mod * vis_marker.y * compress_factor;
            Vector3 v = new Vector3(_x,_y,0);
            layers[i].transform.position = v;
        }
        
    }

    public void Reset_Tree_State( int _p1_wins, int _p2_wins )
    {
        prev_p1_hp = 2;
        prev_p2_hp = 2;
        
        var max_trees = 3;
        for (int i = 0; i < max_trees; i++)
        {
            if (i < _p2_wins)   {   p1_trees[i].set_state(0);   }
            else    {   p1_trees[i].set_state(2);   }
            if (i < _p1_wins)   {   p2_trees[i].set_state(0);   }
            else    {   p2_trees[i].set_state(2);   }
        }
    }
    
    public void Update_Tree_State( int _p1_hp, int _p2_hp, int _p1_wins, int _p2_wins )
    {
        p1_trees[_p2_wins].set_state(_p1_hp);
        p2_trees[_p1_wins].set_state(_p2_hp);

        if (_p1_hp != prev_p1_hp && _p1_hp != 2)
        {
            Trigger_HP_Loss_Effect(p1_trees[_p2_wins], _p1_hp);
            prev_p1_hp = _p1_hp;
        }
        if (_p2_hp != prev_p2_hp && _p2_hp != 2)
        {
            Trigger_HP_Loss_Effect(p2_trees[_p1_wins], _p2_hp);
            prev_p2_hp = _p2_hp;
        }
    }

    void Trigger_HP_Loss_Effect( TreeBehavior _tree, int _new_hp )
    {
        _tree.Trigger_Particles(_new_hp);
        RVM.Apply_Freezeframe(0.2f);
    }
}
