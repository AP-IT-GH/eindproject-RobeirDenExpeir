using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTriggerEnterStrategy : TriggerEnterStrategy
{
    public int HandleTriggerEnter(Collider other, RaceArea raceArea, int nextCheckpointIndex)
    {
        Checkpoint c = other.GetComponentInParent<Checkpoint>();
        Debug.Log(c.ToString());
        Debug.Log($"Collided with: {c.checkpointNumber}");
        
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

                                
            other.GetComponentInParent<Checkpoint>().SetVisible(false);
            raceArea.Checkpoints[nextCheckpointIndex].SetVisible(true);
        }

        return nextCheckpointIndex;
    }
}
