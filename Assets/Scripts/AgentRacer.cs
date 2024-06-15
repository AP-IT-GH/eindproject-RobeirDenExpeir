using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class AgentRacer : Agent
{
    public int NextCheckpointIndex { get; private set; }
    protected RaceArea raceArea;
    protected TriggerEnterStrategy _triggerEnterStrategy;

    protected new Rigidbody rigidbody;

    #region Controls

    //Regular Controls
    public float speed = 10f; // Normal forward speed
    public float yawSpeed = 100f;
    public float rollSpeed = 100f;
    public float pitchSpeed = 100f; // Normal forward speed
    public float boostSpeed = 20f; // Speed during boost

    //Boost
    protected bool isBoosting = false;
    protected float boostDuration = 3f;
    protected float boostCooldown = 5f;
    protected float boostTimer = 0f;
    protected float cooldownTimer = 0f;
    
    #endregion


    public virtual void Awake()
    {
        raceArea = GetComponentInParent<RaceArea>();
        _triggerEnterStrategy = new RacerTriggerEnterStrategy(); // Replace with AgentTriggerEnterStrategy?
    }

    public override void Initialize()
    {
        raceArea = GetComponentInParent<RaceArea>();
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false;
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.useGravity = false; // Disable gravity to simulate lift more effectively
    }

    public override void OnEpisodeBegin()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.angularVelocity = Vector3.zero;
        NextCheckpointIndex = 0;
        raceArea.SpawnAgent(agent: this);

        boostTimer = 0f;
        cooldownTimer = 0f;
        isBoosting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == raceArea.Checkpoints[NextCheckpointIndex].gameObject)
        {
            AddReward(1.0f);
        }
        Debug.Log($"Test index: {NextCheckpointIndex}");
        NextCheckpointIndex = _triggerEnterStrategy.HandleTriggerEnter(other, raceArea, NextCheckpointIndex);
        Debug.Log($"Next Test index: {NextCheckpointIndex}");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!collision.gameObject.CompareTag("checkpoint"))
        {
            AddReward(-0.5f);
            EndEpisode();
        }
    }

    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Continuous actions
        if (GameManager.Instance.State == GameState.InGame)
        {
            float roll = Mathf.Clamp(actionBuffers.ContinuousActions[0], -1f, 1f);
            float pitch = Mathf.Clamp(actionBuffers.ContinuousActions[1], -1f, 1f);
            float yaw = Mathf.Clamp(actionBuffers.ContinuousActions[2], -1f, 1f);
            float boost = Mathf.Clamp(actionBuffers.DiscreteActions[0], 0f, 1f);

            // Handle boost logic
            HandleBoosting(boost);

            // Determine current speed
            float currentSpeed = isBoosting ? boostSpeed : speed;

            float rollRotation = roll * rollSpeed * Time.deltaTime;
            float pitchRotation = pitch * pitchSpeed * Time.deltaTime;
            float yawRotation = yaw * yawSpeed * Time.deltaTime;

            // Hier de volgorde van de Euler hoeken mogelijk aanpassen afhankelijk van de asoriëntatie van je model
            Quaternion deltaRotation = Quaternion.Euler(-pitchRotation, yawRotation, -rollRotation);
            rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);

            Vector3 forwardMovement = transform.forward * currentSpeed * Time.deltaTime;
            rigidbody.MovePosition(rigidbody.position + forwardMovement);
        }
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        /*
        // Collect roll and pitch observations
        Vector3 localEulerAngles = transform.localRotation.eulerAngles;
        float roll = localEulerAngles.z;
        float pitch = localEulerAngles.x;
        sensor.AddObservation(roll);
        sensor.AddObservation(pitch);
        */
        // Observe aircraft velocity (1 Vector3 = 3 values)
        sensor.AddObservation(transform.InverseTransformDirection(rigidbody.velocity));

        // Where is the next checkpoint? (1 Vector3 = 3 values)
        sensor.AddObservation(VectorToNextCheckpoint());

        // Orientation of the next checkpoint (1 Vector3 = 3 values)
        Vector3 nextCheckpointForward = raceArea.Checkpoints[NextCheckpointIndex].transform.forward;
        sensor.AddObservation(transform.InverseTransformDirection(nextCheckpointForward));

        // Total Observations = 3 + 3 + 3 = 9
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;
        var discreteActionsOut = actionsOut.DiscreteActions;

        // Roll: Left/Right arrow keys for roll
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            continuousActionsOut[0] = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            continuousActionsOut[0] = 1f;
        }
        else
        {
            continuousActionsOut[0] = 0f;
        }

        // Pitch: Up/Down arrow keys for pitch
        if (Input.GetKey(KeyCode.UpArrow))
        {
            continuousActionsOut[1] = 1f;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            continuousActionsOut[1] = -1f;
        }
        else
        {
            continuousActionsOut[1] = 0f;
        }

        // Yaw: (Optional, can also be mapped to arrow keys or different keys if needed)
        // Since yaw isn't explicitly requested, it's set to zero.
        continuousActionsOut[2] = 0f;

        // Boost: Space key for boost
        if (Input.GetKey(KeyCode.Space))
        {
            discreteActionsOut[0] = 1;
        }
        else
        {
            discreteActionsOut[0] = 0;
        }
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
        AddReward(1.0f);
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
