using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlaneController : MonoBehaviour
{
    public float speed = 10f;                  // Normal forward speed
    public float boostSpeed = 20f;             // Speed during boost
    public float rotationSpeed = 100f;         // Rotation speed
    public float liftForce = 10f;              // Lift force to keep the plane in the air

    public TMP_Text speedometerText;           // Reference to the UI Text element
    public Image boostBarFill;                 // Reference to the boost bar fill image

    public GameObject boostBarFull;            // Reference to the BoostBarFull GameObject
    public GameObject boostBarEmpty;           // Reference to the BoostBarEmpty GameObject

    private Rigidbody rb;
    private bool isBoosting = false;
    private float boostDuration = 3f;
    private float boostCooldown = 5f;
    private float boostTimer = 0f;
    private float cooldownTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Disable gravity to simulate lift more effectively
    }

    void Update()
    {
        // Get input from arrow keys
        float roll = 0f;
        float pitch = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            roll = -1;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            roll = 1;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            pitch = 1;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            pitch = -1;
        }

        // Handle boost logic
        if (Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0f)
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

        // Determine current speed
        float currentSpeed = isBoosting ? boostSpeed : speed;

        // Apply rotation
        float rollRotation = roll * rotationSpeed * Time.deltaTime;
        float pitchRotation = pitch * rotationSpeed * Time.deltaTime;
        rb.MoveRotation(rb.rotation * Quaternion.Euler(pitchRotation, 0, -rollRotation));

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
            {
                boostBarFill.fillAmount = 1.0f - (cooldownTimer / boostCooldown);
            }
            else
            {
                boostBarFill.fillAmount = 1.0f;
            }
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
