using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SharpNEAT;
using static SharpNeat.Phenomes.IBlackBox;


public class NEAT : UnitController
{

    public LayerMask buildings;
    private Rigidbody rb;

    private Vector3 start;
    private Vector3 startRot;

    public float elapsedTime = 0.0f;
    public float fitness;
    public float distanceWeight = 1.0f;
    public float speedWeight = 0.3f;

    private Vector3 previousPosition;
    private float distanceTravelled;
    private float averageSpeed;
    private SharpNeat.Phenomes.IBlackBox box;
    private float leftSensor, forwardLeftSensor, forwardSensor, forwardRightSensor, rightSensor, backSensor;
    private Vector3 forwardAction, rotateAction;

    void Awake()
    {
        this.start = transform.position;
        this.startRot = transform.eulerAngles;
    }

    public override void Stop()
    {
        this.elapsedTime = 0.0f;
        this.distanceTravelled = 0.0f;
        this.averageSpeed = 0.0f;
        this.previousPosition = start;
        transform.position = start;
        transform.eulerAngles = startRot;

    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();
       
    }

    void FixedUpdate()
    {
        getInputs(this.rb);
        previousPosition = transform.position;
        Activate(this.box);
        //float[] output = box.Activate();
        //float motor = output[0];
        //float steering = output[1];
        //rb.AddForce(motor);
        //rb.AddTorque(steering);
        elapsedTime += Time.deltaTime;
        GetFitness();
    }

    private void OnCollisionEnter(Collision collide)
    {
        Stop();
    }

    public override float GetFitness()
    {
        distanceTravelled += Vector3.Distance(transform.position, this.previousPosition);
        averageSpeed = distanceTravelled / elapsedTime;
        float fitness = (distanceTravelled * distanceWeight) + (averageSpeed * speedWeight);
        if (elapsedTime > 50 && fitness < 100)
        {
            Stop();
        }
        return fitness;
    }
    public override void Activate(SharpNeat.Phenomes.IBlackBox box)
    {
        this.box = box;
        //box.setInputSignalArray(new float[]{ leftSensor, forwardLeftSensor, forwardSensor, forwardRightSensor, rightSensor, backSensor });
    }

    void getInputs(Rigidbody rb)
    {
        RaycastHit hit;
        if (Physics.Raycast(rb.transform.position, transform.TransformDirection(Vector3.left), out hit, 10.0f, buildings))
        {
            this.leftSensor = hit.distance/10;
        }
        if (Physics.Raycast(rb.transform.position, transform.TransformDirection(new Vector3(-1, 0, 1)), out hit, 10.0f, buildings))
        {
            this.forwardLeftSensor = hit.distance/10;
        }
        if (Physics.Raycast(rb.transform.position, transform.TransformDirection(Vector3.forward), out hit, 10.0f, buildings))
        {
            this.forwardSensor = hit.distance/10;
        }
        if (Physics.Raycast(rb.transform.position, transform.TransformDirection(new Vector3(1, 0, 1)), out hit, 10.0f, buildings))
        {
            this.forwardRightSensor = hit.distance/10;
        }
        if (Physics.Raycast(rb.transform.position, transform.TransformDirection(Vector3.right), out hit, 10.0f, buildings))
        {
            this.rightSensor = hit.distance/10;
        }
        if (Physics.Raycast(rb.transform.position, transform.TransformDirection(Vector3.back), out hit, 10.0f, buildings))
        {
            this.backSensor = hit.distance/10;
        }

    }
}


// https://sketchfab.com/3d-models/generic-passenger-car-pack-20f9af9b8a404d5cb022ac6fe87f21f5
// https://www.youtube.com/watch?v=C6SZUU8XQQ0