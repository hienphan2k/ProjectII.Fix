using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Junction : MonoBehaviour
{
    [SerializeField] private Junction[] junctions;
    public Junction[] Junctions { get => junctions; }
    public Queue<AGV> qu = new Queue<AGV>();

    public float weight = 0, lambda = 0.5f;
    public float time_free = 0;

    public const float interval_Time = 15; // time

    public void OnTriggerEnter(Collider other)
    {
        AGV agv;
        if (other.TryGetComponent<AGV>(out agv))
        {
            if(this.qu.Count > 0)
            {
                agv.time_start = (float) Time.realtimeSinceStartup;
                agv.blocked = true;
            }
            qu.Enqueue(agv);
        }
        
    }

    public void OnTriggerExit(Collider other)
    {
        AGV agv;
        if (other.TryGetComponent<AGV>(out agv))
        {
            qu.Dequeue();
            // Debug.Log(this.gameObject.name+" "+this.weight+" "+"out");
            if(qu.Count > 0)
            {
                agv = qu.Peek();
                agv.time_finish = (float) Time.realtimeSinceStartup;
                agv.blocked = false;
                this.weight += lambda * (agv.time_finish - agv.time_start); 
                // Debug.Log(this.gameObject.name+" "+this.weight+ " " + agv.time_start + " " + agv.time_finish + " "+"out");
            }
            
        }
    }

    void Update()
    {
        if(qu.Count == 0)
            this.time_free += Time.deltaTime;
        else
            this.time_free = 0;
        
        this.weight = (1-this.lambda) * this.weight;

        // if(this.time_free > Junction.interval_Time)
        // {
        //     this.weight = 0;
        //     print("set junction3 to 0");
        // }
        // if(this.gameObject.name == "Junction (3)")
        // {
        //     Debug.Log(this.gameObject.name+" "+this.weight);
        // }
    }
}
