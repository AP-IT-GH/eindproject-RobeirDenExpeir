using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.UI;
using TMPro;

public class PlaneController : MonoBehaviour
{
    public float speed = 10f;                  // Normal forward speed
    public float boostSpeed = 20f;             // Speed during boost
    public float rotationSpeed = 100f;         // Rotation speed
    public float liftForce = 10f;              // Lift force to keep the plane in the air
    public float lateralSpeed = 5f;            // Speed for lateral movement (left and right)
    public float maxRollAngle = 45f;           // Maximum roll angle

    public TMP_Text speedometerText;           // Reference to the UI Text element
    public Image boostBarFill;                 // Reference to the boost bar fill image

    public GameObject boostBarFull;            // Reference to the BoostBarFull GameObject
    public GameObject boostBarEmpty;           // Reference to the BoostBarEmpty GameObject
    public TrailRenderer trailRenderer;        // Reference to the Trail Renderer component

    private Rigidbody rb;
    private bool isBoosting = false;
    private float boostDuration = 3f;
    private float boostCooldown = 5f;
    private float boostTimer = 0f;
    private float cooldownTimer = 0f;

    private InputAction rollAction;
    private InputAction pitchAction;
    private InputAction boostAction;
    private InputAction moveLeftAction;
    private InputAction moveRightAction;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable gravity to simulate lift more effectively

        var playerInput = GetComponent<PlayerInput>();
        var actionMap = playerInput.actions.FindActionMap("Gameplay");

        rollAction = actionMap.FindAction("Roll");
        pitchAction = actionMap.FindAction("Pitch");
        boostAction = actionMap.FindAction("Boost");
        moveLeftAction = actionMap.FindAction("MoveLeft");
        moveRightAction = actionMap.FindAction("MoveRight");

        // Initially disable the trail renderer
        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
        }
        else
        {
            Debug.LogError("TrailRenderer not assigned!");
        }
    }

    void Update()
    {
        // Get input from XR controllers or keyboard
        float roll = 0f;
        float pitch = 0f;
        float lateralMovement = 0f;

        if (rollAction != null)
        {
            roll = rollAction.ReadValue<float>();
        }
        else
        {
            if (Input.GetKey(KeyCode.A))
            {
                roll = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                roll = 1;
            }
        }

        if (pitchAction != null)
        {
            pitch = pitchAction.ReadValue<float>();
        }
        else
        {
            if (Input.GetKey(KeyCode.W))
            {
                pitch = -1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                pitch = 1;
            }
        }

        // Read lateral movement input
        if (moveLeftAction != null && moveLeftAction.IsPressed())
        {
            lateralMovement = -1; // Move left
        }
        else if (moveRightAction != null && moveRightAction.IsPressed())
        {
            lateralMovement = 1; // Move right
        }

        // Handle boost logic
        if (boostAction != null && boostAction.triggered && cooldownTimer <= 0f)
        {
            isBoosting = true;
            boostTimer = boostDuration;
            // Enable trail renderer when boosting
            if (trailRenderer != null)
            {
                trailRenderer.emitting = true;
                Debug.Log("Boosting: TrailRenderer enabled");
            }
        }
        else if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0f)
        {
            isBoosting = true;
            boostTimer = boostDuration;
            // Enable trail renderer when boosting
            if (trailRenderer != null)
            {
                trailRenderer.emitting = true;
                Debug.Log("Boosting: TrailRenderer enabled");
            }
        }

        if (isBoosting)
        {
            boostTimer -= Time.deltaTime;
            if (boostTimer <= 0f)
            {
                isBoosting = false;
                cooldownTimer = boostCooldown;
                // Disable trail renderer when not boosting
                if (trailRenderer != null)
                {
                    trailRenderer.emitting = false;
                    Debug.Log("Boosting ended: TrailRenderer disabled");
                }
            }
        }
        else if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
            // Ensure trail renderer is disabled when cooling down
            if (trailRenderer != null)
            {
                trailRenderer.emitting = false;
                Debug.Log("Cooldown: TrailRenderer disabled");
            }
        }

        // Determine current speed
        float currentSpeed = isBoosting ? boostSpeed : speed;

        // Apply rotation using XR controllers or keyboard
        float rollRotation = roll * rotationSpeed * Time.deltaTime;
        float pitchRotation = pitch * rotationSpeed * Time.deltaTime;

        // Apply the roll rotation
        Quaternion targetRotation = rb.rotation * Quaternion.Euler(pitchRotation, 0, -rollRotation);

        // Clamp the roll angle to maxRollAngle
        float currentRoll = targetRotation.eulerAngles.z;
        currentRoll = (currentRoll > 180) ? currentRoll - 360 : currentRoll; // Convert to -180 to 180 range
        currentRoll = Mathf.Clamp(currentRoll, -maxRollAngle, maxRollAngle);

        // Set the clamped roll angle
        targetRotation = Quaternion.Euler(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, currentRoll);

        // Apply the clamped rotation
        rb.MoveRotation(targetRotation);

        // Apply lateral movement
        Vector3 lateralMovementVector = transform.right * lateralMovement * lateralSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + lateralMovementVector);

        // Apply constant forward movement
        Vector3 forwardMovement = transform.forward * currentSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + forwardMovement);

        // Apply lift force to keep the plane in the air
        Vector3 lift = transform.up * liftForce * Time.deltaTime;
        rb.AddForce(lift, ForceMode.Acceleration);

        // Update speedometer text
        speedometerText.text = $"Speed: {currentSpeed:F1} units/sec";

        // Update boost bar fill amount
        if (boostBarFill != null)
        {
            if (cooldownTimer > 0f)
                boostBarFill.fillAmount = 1.0f - (cooldownTimer / boostCooldown);
            else
                boostBarFill.fillAmount = 1.0f;
        }

        // Update boost bar visibility
        if (boostBarFull != null && boostBarEmpty != null)
        {
            if (cooldownTimer <= 0f)
            {
                boostBarFull.SetActive(true);
                boostBarEmpty.SetActive(false);
            }
            else
            {
                boostBarFull.SetActive(false);
                boostBarEmpty.SetActive(true);
            }
        }
    }
}
