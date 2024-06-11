using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerEnterStrategy : TriggerEnterStrategy
{
    public int HandleTriggerEnter(Collider other, RaceArea raceArea, int nextCheckpointIndex)
    {
        Checkpoint c = other.GetComponent<Checkpoint>();
        Debug.Log($"Collided with: {c.checkpointNumber}");
        if (c.checkpointNumber == nextCheckpointIndex)
        {
            Debug.Log($"Collided with checkpoint: {nextCheckpointIndex} ");
            nextCheckpointIndex = (nextCheckpointIndex + 1) % raceArea.Checkpoints.Count;
            Debug.Log($"NextCheckpointIndex: {nextCheckpointIndex}, CheckpointNumber: {other.GetComponent<Checkpoint>().checkpointNumber}, RaceArea.Checkpoints.Count: {raceArea.Checkpoints.Count}");

                                
            other.GetComponent<Checkpoint>().SetVisible(false);
            raceArea.Checkpoints[nextCheckpointIndex].SetVisible(true);
        }

        return nextCheckpointIndex;
    }
}
