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
        if (GameManager.Instance.State == GameState.InGame)
        {
            float maxRollAngle = 45f; // Maximale roll hoek in graden

            float roll = 0f;
            float pitch = 0f;
            float yaw = 0f;

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                yaw = -1f;
                roll = Mathf.Clamp(roll - (rollSpeed * Time.deltaTime), -maxRollAngle, 0f); // Clamping de roll naar links
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                yaw = 1f;
                roll = Mathf.Clamp(roll + (rollSpeed * Time.deltaTime), 0f, maxRollAngle); // Clamping de roll naar rechts
            }
            else
            {
                // Als geen van de roll toetsen ingedrukt is, ga terug naar neutraal
                if (roll < 0f)
                    roll = Mathf.Min(roll + (rollSpeed * Time.deltaTime), 0f);
                else if (roll > 0f)
                    roll = Mathf.Max(roll - (rollSpeed * Time.deltaTime), 0f);
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                pitch = 1f;
            }
            else if (Input.GetKey(KeyCode.UpArrow))
            {
                pitch = -1f;
            }
            else
            {
                // Als geen van de pitch toetsen ingedrukt is, ga terug naar neutraal
                if (pitch < 0f)
                    pitch = Mathf.Min(pitch + (pitchSpeed * Time.deltaTime), 0f);
                else if (pitch > 0f)
                    pitch = Mathf.Max(pitch - (pitchSpeed * Time.deltaTime), 0f);
            }

            float boost = Input.GetKey(KeyCode.Space) ? 1f : 0f;
            HandleBoosting(boost);

            float currentSpeed = isBoosting ? boostSpeed : speed;

            // Bereken de rotatie
            float rollRotation = roll * Time.deltaTime * rollSpeed;
            float pitchRotation = pitch * Time.deltaTime * pitchSpeed;
            float yawRotation = yaw * Time.deltaTime * yawSpeed;

            // Hier de volgorde van de Euler hoeken mogelijk aanpassen afhankelijk van de asoriëntatie van je model
            transform.Rotate(-pitchRotation, yawRotation, -rollRotation, Space.Self);

            // Beweeg het vliegtuig naar voren
            transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);
        }
    }
}
