using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RaceArea : MonoBehaviour
{
    public List<Checkpoint> Checkpoints { get; set; }
    

    void Start()
    {
        Checkpoints = GetComponentsInChildren<Checkpoint>().ToList();
        Debug.Log($"Found {Checkpoints.Count} checkpoints");
        Checkpoints.Sort((a, b) => a.checkpointNumber.CompareTo(b.checkpointNumber));
        Checkpoints[0].SetVisible(true);
    }
    
    
}

