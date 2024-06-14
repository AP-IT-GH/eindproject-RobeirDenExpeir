using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RaceManager : MonoBehaviour
{
    public static RaceManager Instance { get; private set; }
    public List<AgentRacer> Racers { get; private set; }
    [SerializeField] private CountdownUIController _countdownUIController;

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
    }

    private void Update()
    {
        UpdateRacerPositions();
        UpdatePlayerPosition();
    }

    private void UpdateRacerPositions()
    {
        // Sort racers by their progress (you need to define how to measure progress)
        Racers.Sort((racer1, racer2) => racer2.Position.CompareTo(racer1.Position));
    }

    private void UpdatePlayerPosition()
    {
        // Find player's position
        for (int i = 0; i < Racers.Count; i++)
        {
            if (Racers[i].IsPlayer)
            {
                PlayerPosition = i + 1;
                break;
            }
        }
    }
}
