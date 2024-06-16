using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaceArea : MonoBehaviour
{
    public List<Checkpoint> Checkpoints { get; set; }
    public StartPoint startPoint;
    public List<AgentRacer> Agents { get; private set; }


    private void Awake()
    {
        Agents = FindObjectsOfType<AgentRacer>().ToList();
        startPoint = GetComponentInChildren<StartPoint>();
        Debug.Log($"Found {Agents.Count} Agents");
    }

    void Start()
    {
        ResetCheckpoints();
    }
    public void ResetAgentPosition(AgentRacer agent)
    {
        var agentTransform = agent.transform;
        int previousCheckpoint = agent.NextCheckpointIndex - 1;
        if (previousCheckpoint == -1)
        {
            agentTransform.position = startPoint.gameObject.transform.position;
            agentTransform.rotation = startPoint.gameObject.transform.rotation;
        }
        

        // Set the aircraft position and rotation
        
        agentTransform.position = Checkpoints[previousCheckpoint].gameObject.transform.position;
        agentTransform.rotation = Checkpoints[previousCheckpoint].gameObject.transform.rotation;
    }

    public void ResetRace()
    {
        foreach (var agent in Agents)
        {
            SpawnAgent(agent);
        }
        ResetCheckpoints();
    }

    private void ResetCheckpoints()
    {
        Checkpoints = GetComponentsInChildren<Checkpoint>().ToList();
        Debug.Log($"Found {Checkpoints.Count} checkpoints");
        Checkpoints.Sort((a, b) => a.checkpointNumber.CompareTo(b.checkpointNumber));
        Checkpoints[0].SetVisible(true);
    }
    
    public void SpawnAgent(AgentRacer agent)
    {
            Vector3 startPosition = startPoint.gameObject.transform.position;
            
            // Calculate a horizontal offset so that agents are spread out
            Vector3 positionOffset = Vector3.forward * (Agents.IndexOf(agent) - Agents.Count / 2f)
                                                   * 30f;
            
            // Set the aircraft position and rotation
            var agentTransform = agent.transform;
            agentTransform.position = startPosition + positionOffset;
            agentTransform.rotation = startPoint.gameObject.transform.rotation;
    }
}

