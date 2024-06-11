using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using UnityEngine;

public class AgentRacer : Agent
{
    public int NextCheckpointIndex { get; set; }
    private RaceArea raceArea;
    protected TriggerEnterStrategy _triggerEnterStrategy;
    
    public bool IsPlayer { get; set; }
    public float Position { get; set; }
    public virtual void Awake()
    {
        raceArea = FindObjectOfType<RaceArea>();
        _triggerEnterStrategy = new RacerTriggerEnterStrategy(); // Replace with AgentTriggerEnterStrategy?
    }
    
    private void OnTriggerEnter(Collider other)
    {
        NextCheckpointIndex = _triggerEnterStrategy.HandleTriggerEnter(other, raceArea, NextCheckpointIndex);
    }
}
