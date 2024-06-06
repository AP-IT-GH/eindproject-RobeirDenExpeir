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
        Checkpoints.Sort((a, b) => a.CheckpointNumber.CompareTo(b.CheckpointNumber));
        Checkpoints[0].SetVisible(true);
    }
    
    
}

