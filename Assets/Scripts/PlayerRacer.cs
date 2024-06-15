using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerRacer : AgentRacer
{
    //Boost UI
    // public TMP_Text speedometerText;           // Reference to the UI Text element
    // public Image boostBarFill;                 // Reference to the boost bar fill image
    //
    // public GameObject boostBarFull;            // Reference to the BoostBarFull GameObject
    // public GameObject boostBarEmpty;           // Reference to the BoostBarEmpty GameObject

    public float yawSpeed = 100f;
    public float rollSpeed = 100f;
    public float pitchSpeed = 100f;

    void FixedUpdate()
    {
        float roll = 0f;
        float pitch = 0f;
        float yaw = 0f;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            yaw = -1f;
            roll = -1f;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            yaw = 1f;
            roll = 1f;
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            pitch = 1f;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            pitch = -1f;
        }

        float boost = Input.GetKey(KeyCode.Space) ? 1f : 0f;
        HandleBoosting(boost);

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
