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
    
    public override void Awake()
    {
        base.Awake();
        _triggerEnterStrategy = new PlayerTriggerEnterStrategy();
        Debug.Log(_triggerEnterStrategy.ToString());
    }
    void FixedUpdate()
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
        float boost = Input.GetKey(KeyCode.Space) ? 1f : 0f;
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
        
    }


}
