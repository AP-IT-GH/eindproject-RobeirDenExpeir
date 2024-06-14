using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRacer : AgentRacer
{
    
    public override void Awake()
    {
        base.Awake();
        _triggerEnterStrategy = new PlayerTriggerEnterStrategy();
        Debug.Log(_triggerEnterStrategy.ToString());
    }


}
