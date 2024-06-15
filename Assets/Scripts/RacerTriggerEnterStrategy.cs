using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RacerTriggerEnterStrategy : TriggerEnterStrategy
{
    /// <summary>
    /// Handles the trigger enter event for a racer.
    /// This method is called when a racer collides with a checkpoint.
    /// It updates the next checkpoint index for the racer and manages the visibility of the checkpoints.
    /// </summary>
    /// <param name="other">The Collider that the racer collided with.</param>
    /// <param name="raceArea">The RaceArea that the racer is in.</param>
    /// <param name="nextCheckpointIndex">The index of the next checkpoint that the racer should go to.</param>
    /// <returns>The updated next checkpoint index.</returns>
    public int HandleTriggerEnter(Collider other, RaceArea raceArea, int nextCheckpointIndex)
    {
        Checkpoint c = other.GetComponentInParent<Checkpoint>();
        if(c.checkpointNumber == nextCheckpointIndex && nextCheckpointIndex == raceArea.Checkpoints.Count - 1)
        {
            //Final checkpoint
            Debug.Log("Finished the race!");
            GameManager.Instance.UpdateGameState(GameState.RaceEnd);
            return nextCheckpointIndex;
        }
        if (c.checkpointNumber == nextCheckpointIndex)
        {
            Debug.Log($"Collided with checkpoint: {nextCheckpointIndex} ");
            nextCheckpointIndex++;
            Debug.Log($"NextCheckpointIndex: {nextCheckpointIndex}, CheckpointNumber: {c.checkpointNumber}, RaceArea.Checkpoints.Count: {raceArea.Checkpoints.Count}");
        }

        return nextCheckpointIndex;
    }
}
