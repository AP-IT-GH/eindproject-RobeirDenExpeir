using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AgentRacer : Agent
{
    public int NextCheckpointIndex { get; set; }
    private RaceArea raceArea;
    protected TriggerEnterStrategy _triggerEnterStrategy;
    
    public bool IsPlayer { get; set; }
    public float Position { get; set; }

    private new Rigidbody rigidbody;


    public virtual void Awake()
    {
        raceArea = FindObjectOfType<RaceArea>();
        _triggerEnterStrategy = new RacerTriggerEnterStrategy(); // Replace with AgentTriggerEnterStrategy?
    }
    
    private void OnTriggerEnter(Collider other)
    {
        NextCheckpointIndex = _triggerEnterStrategy.HandleTriggerEnter(other, raceArea, NextCheckpointIndex);
        if (other.gameObject == raceArea.Checkpoints[NextCheckpointIndex])
        {
            GotCheckpoint(); 
        }
    }

public override void Initialize()
    {
        raceArea = GetComponentInParent<RaceArea>();
        rigidbody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        raceArea.ResetAgentPosition(agent: this);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Observe aircraft velocity (1 Vector3 = 3 values)
        sensor.AddObservation(transform.InverseTransformDirection(rigidbody.velocity));

        // Where is the next checkpoint? (1 Vector3 = 3 values)
        sensor.AddObservation(VectorToNextCheckpoint());

        // Orientation of the next checkpoint (1 Vector3 = 3 values)
        Vector3 nextCheckpointForward = raceArea.Checkpoints[NextCheckpointIndex].transform.forward;
        sensor.AddObservation(transform.InverseTransformDirection(nextCheckpointForward));

        // Total Observations = 3 + 3 + 3 = 9
    }

    private Vector3 VectorToNextCheckpoint()
    {
        Vector3 nextCheckpointDir = raceArea.Checkpoints[NextCheckpointIndex].transform.position - transform.position;
        Vector3 localCheckpointDir = transform.InverseTransformDirection(nextCheckpointDir);
        return localCheckpointDir;
    }

    private void GotCheckpoint()
    {
        // Next checkpoint reached, update
        NextCheckpointIndex = (NextCheckpointIndex + 1) % raceArea.Checkpoints.Count;
        AddReward(0.5f);
    }

}
