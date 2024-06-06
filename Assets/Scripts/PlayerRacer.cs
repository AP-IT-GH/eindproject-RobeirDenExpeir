using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRacer : MonoBehaviour
{
    public int NextCheckpointIndex { get; set; }
    private RaceArea _raceArea;
    private TriggerEnterStrategy _triggerEnterStrategy;
    
    public void Awake()
    {
        _raceArea = FindObjectOfType<RaceArea>();
        _triggerEnterStrategy = new RacerTriggerEnterStrategy();
    }
    
    private void OnTriggerEnter(Collider other)
    {
        NextCheckpointIndex = _triggerEnterStrategy.HandleTriggerEnter(other, _raceArea, NextCheckpointIndex);

    }
    
    


}
