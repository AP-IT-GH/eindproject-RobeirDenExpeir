using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public int CheckpointNumber;

    private void Awake()
    {
        SetVisible(false);
    }
    
    public void SetVisible(bool visible)
    {
        this.GetComponent<MeshRenderer>().enabled = visible;
    }
}
