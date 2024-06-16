using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance { get; private set; }
    public List<AgentRacer> Racers { get; private set; }
    public RaceArea RaceArea;
    [SerializeField] private CountdownUIController _countdownUIController;
    public bool RaceStarted { get; private set; }

    public int PlayerPosition { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        RaceArea = FindObjectOfType<RaceArea>();
        InitializeRace();
    }

    private void InitializeRace()
    {
        // Initialize racers list
        Racers = new List<AgentRacer>(FindObjectsOfType<AgentRacer>());
        foreach (var racer in Racers)
        {
            Console.WriteLine(racer.ToString());
        }

        StartCoroutine(StartRace());
    }

    private IEnumerator StartRace()
    {
        yield return _countdownUIController.StartCountdown();
        
            GameManager.Instance.UpdateGameState(GameState.InGame);
    }

    public void ResetRace()
    {
        InitializeRace();
        foreach (var racer in Racers)
        {
            racer.Reset();
        }

        RaceArea.ResetRace();
    }
    
    
    
}
