using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AgentRacer : Agent
{
    public int NextCheckpointIndex { get; private set; }
    private RaceArea raceArea;
    protected TriggerEnterStrategy _triggerEnterStrategy;
    
    public bool IsPlayer { get; set; }
    public float Position { get; set; }

    protected new Rigidbody rigidbody;

    #region Controls
    
    //Regular Controls
    public float speed = 10f;                  // Normal forward speed
    public float boostSpeed = 20f;             // Speed during boost
    public float rotationSpeed = 100f;         // Rotation speed
    public float liftForce = 10f;              // Lift force to keep the plane in the air
    
    //Boost
    protected bool isBoosting = false;
    protected float boostDuration = 3f;
    protected float boostCooldown = 5f;
    protected float boostTimer = 0f;
    protected float cooldownTimer = 0f;
    
    #endregion


    public virtual void Awake()
    {
        raceArea = FindObjectOfType<RaceArea>();
        _triggerEnterStrategy = new RacerTriggerEnterStrategy(); // Replace with AgentTriggerEnterStrategy?
    }
    
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false; // Disable gravity to simulate lift more effectively
    }
    
    private void OnTriggerEnter(Collider other)
    {
        NextCheckpointIndex = _triggerEnterStrategy.HandleTriggerEnter(other, raceArea, NextCheckpointIndex);
        if (other.gameObject == raceArea.Checkpoints[NextCheckpointIndex].gameObject)
        {
            GotCheckpoint(); 
        }
    }
    
    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        float forwardAmount = actionBuffers.ContinuousActions[0];
        float turnAmount = actionBuffers.ContinuousActions[1];
        rigidbody.AddForce(transform.forward * forwardAmount * 10f, ForceMode.VelocityChange);
        transform.Rotate(transform.up, turnAmount * 4f);
        AddReward(-0.0005f);
    }

    public override void Initialize()
    {
        raceArea = FindObjectOfType<RaceArea>();
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
