using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TriggerEnterStrategy
{
    int HandleTriggerEnter(Collider other, RaceArea raceArea, int nextCheckpointIndex);
}
