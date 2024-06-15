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
        // Continuous actions
        if (GameManager.Instance.State == GameState.InGame)
        {
            float roll = Mathf.Clamp(actionBuffers.ContinuousActions[0], -1f, 1f);
            float pitch = Mathf.Clamp(actionBuffers.ContinuousActions[1], -1f, 1f);
            float boost = Mathf.Clamp(actionBuffers.DiscreteActions[0], 0f, 1f);

            // Handle boost logic
            HandleBoosting(boost);

            // Determine current speed
            float currentSpeed = isBoosting ? boostSpeed : speed;
            
            // Apply rotation
            float rollRotation = roll * rotationSpeed * Time.deltaTime;
            float pitchRotation = pitch * rotationSpeed * Time.deltaTime;
            rigidbody.MoveRotation(rigidbody.rotation * Quaternion.Euler(pitchRotation, 0, -rollRotation));

            // Apply constant forward movement
            Vector3 forwardMovement = transform.forward * currentSpeed * Time.deltaTime;
            rigidbody.MovePosition(rigidbody.position + forwardMovement);

            // Example: Reward for moving forward
            AddReward(currentSpeed * 0.001f);
        }
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

        boostTimer = 0f;
        cooldownTimer = 0f;
        isBoosting = false;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Collect roll and pitch observations
        Vector3 localEulerAngles = transform.localRotation.eulerAngles;
        float roll = localEulerAngles.z;
        float pitch = localEulerAngles.x;
        sensor.AddObservation(roll);
        sensor.AddObservation(pitch);
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
        Debug.Log($"Agent collided with Checkpoint {NextCheckpointIndex}");
        NextCheckpointIndex = (NextCheckpointIndex + 1) % raceArea.Checkpoints.Count;
        AddReward(0.5f);
    }
    protected void HandleBoosting(float boost)
    {
        if (boost > 0.5f && cooldownTimer <= 0f)
        {
            isBoosting = true;
            boostTimer = boostDuration;
        }

        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0f)
            {
                isBoosting = false;
                cooldownTimer = boostCooldown;
            }
        }
        else if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }
    }
}
