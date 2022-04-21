using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class MLAgent : Agent
{
    private Rigidbody player;
    public float speedMultiplier =20.0f;
    public LayerMask buildings;
    public VectorSensor sensor;
    public int count = 0;

    void Start()
    {
        player = GetComponent<Rigidbody>();
        
    }
    void FixedUpdate()
    {
        count += 1;
    }
    public override void OnEpisodeBegin()
    {
        transform.position = new Vector3(-166.8707f, 22.5f, 745.3023f);
        transform.rotation = Quaternion.Euler(-90f, 0f, 2.741f);
        player.velocity = new Vector3(0, 0, 0); 
    }


    public override void OnActionReceived(ActionBuffers moves)
    {
        float forward = moves.ContinuousActions[0];
        float rotate = moves.ContinuousActions[1];
        this.player.AddForce(-transform.up * forward *-55 );
        this.player.AddTorque(new Vector3(0f, rotate, 0f) * 25 );

        
    }
    public override void Heuristic(in ActionBuffers actions)
    {
        if (actions.ContinuousActions[0] > 0.5) {
            AddReward(3.0f);
            Debug.Log("rewarded");
        }
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, transform.TransformDirection(Vector3.left), out hit, 10.0f, buildings))
        {
            if((hit.distance / 10) > 0.9)
            {
                AddReward(5.0f);
            }
        }
        if (Physics.Raycast(player.transform.position, transform.TransformDirection(new Vector3(-1, 0, 1)), out hit, 10.0f, buildings))
        {
            if ((hit.distance / 10) > 0.9)
            {
                AddReward(5.0f);
            }
        }
        if (Physics.Raycast(player.transform.position, transform.TransformDirection(Vector3.forward), out hit, 10.0f, buildings))
        {
            if ((hit.distance / 10) > 0.9)
            {
                AddReward(5.0f);
            }
        }
        if (Physics.Raycast(player.transform.position, transform.TransformDirection(new Vector3(1, 0, 1)), out hit, 10.0f, buildings))
        {
            if ((hit.distance / 10) > 0.9)
            {
                AddReward(5.0f);
            }
        }
        if (Physics.Raycast(player.transform.position, transform.TransformDirection(Vector3.right), out hit, 10.0f, buildings))
        {
            if ((hit.distance / 10) > 0.9)
            {
                AddReward(5.0f);
            }
        }
        if (Physics.Raycast(player.transform.position, transform.TransformDirection(Vector3.back), out hit, 10.0f, buildings))
        {
            if ((hit.distance / 10) > 0.9)
            {
                AddReward(5.0f);
            }
        }
           
        AddReward(this.count/10);
        


    }
    public override void CollectObservations(VectorSensor sensor)
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, transform.TransformDirection(Vector3.left), out hit, 10.0f, buildings))
        {
            this.sensor.AddObservation(hit.distance / 10);
        }
        if (Physics.Raycast(player.transform.position, transform.TransformDirection(new Vector3(-1, 0, 1)), out hit, 10.0f, buildings))
        {
            this.sensor.AddObservation(hit.distance / 10);
        }
        if (Physics.Raycast(player.transform.position, transform.TransformDirection(Vector3.forward), out hit, 10.0f, buildings))
        {
            this.sensor.AddObservation(hit.distance / 10);
        }
        if (Physics.Raycast(player.transform.position, transform.TransformDirection(new Vector3(1, 0, 1)), out hit, 10.0f, buildings))
        {
            this.sensor.AddObservation(hit.distance / 10);
        }
        if (Physics.Raycast(player.transform.position, transform.TransformDirection(Vector3.right), out hit, 10.0f, buildings))
        {
            this.sensor.AddObservation(hit.distance / 10);
        }
        if (Physics.Raycast(player.transform.position, transform.TransformDirection(Vector3.back), out hit, 10.0f, buildings))
        {
            this.sensor.AddObservation(hit.distance / 10);
        }
        
    }
    private void OnCollisionEnter(Collision obstacle)
    {
        if (obstacle.collider.tag == "Floor")
        {
            Debug.Log("Hit floor");
        } else {
            Debug.Log("collision");
            
            SetReward(0.0f);
            this.count = 0;
            EndEpisode();
        }
    }
}
