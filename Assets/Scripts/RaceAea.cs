using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaceArea : MonoBehaviour
{
    public List<Checkpoint> Checkpoints { get; set; }

    public List<AgentRacer> Agents { get; private set; }


    private void Awake()
    {
        Agents = transform.GetComponentsInChildren<AgentRacer>().ToList();
        Debug.Log($"Found {Agents.Count} Agents");
    }


    void Start()
    {
        Checkpoints = GetComponentsInChildren<Checkpoint>().ToList();
        Debug.Log($"Found {Checkpoints.Count} checkpoints");
        Checkpoints.Sort((a, b) => a.checkpointNumber.CompareTo(b.checkpointNumber));
        Checkpoints[0].SetVisible(true);
    }

    public void ResetAgentPosition(AgentRacer agent)
    {
        int previousCheckpoint = agent.NextCheckpointIndex - 1;
        if (previousCheckpoint == -1) previousCheckpoint = Checkpoints.Count - 1;

        Vector3 startPosition = Checkpoints[0].gameObject.transform.position;


        // Calculate a horizontal offset so that agents are spread out
        Vector3 positionOffset = Vector3.right * (Agents.IndexOf(agent) - Agents.Count / 2f)
            * UnityEngine.Random.Range(9f, 10f);

        // Set the aircraft position and rotation
        agent.transform.position = startPosition + positionOffset;
        agent.transform.rotation = Checkpoints[previousCheckpoint].gameObject.transform.rotation;
    } 
}

