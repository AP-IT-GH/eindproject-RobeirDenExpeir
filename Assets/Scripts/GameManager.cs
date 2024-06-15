using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton Instance
    public static GameManager Instance { get; private set; }
    public GameState State { get; private set; }
    
    public static event Action<GameState> OnGameStateChanged;
    public static event Action<string, int, int> OnRaceEnd;

    public bool isTraining = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist the GameManager across scenes
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
        }
    }

    private void Start()
    {
        if (isTraining)
        {
            UpdateGameState(GameState.InGame);
        }
        else
        {
            UpdateGameState(GameState.MainMenu);
        }
    }
    
    public void OnApplicationQuit()
    {
        Instance = null;
    }

    private void Update()
    {
        // Check for the escape key press to toggle pause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //Pause game
            if (State == GameState.InGame)
            {
                UpdateGameState(GameState.Paused);
            }
        }
    }


    public void UpdateGameState(GameState state)
    {
        State = state;

        switch (state)
        {
            case GameState.MainMenu:
                PauseRace();
                break;
            case GameState.CountDown:
                ResumeRace();
                break;
            case GameState.InGame:
                ResumeRace();
                break;
            case GameState.Paused:
                PauseRace();
                break;
            case GameState.RaceEnd:
                PauseRace();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }

        OnGameStateChanged?.Invoke(state);
    }
    
    public void ResumeRace()
    {
        Time.timeScale = 1;
    }
    
    public void PauseRace()
    {
        Time.timeScale = 0;
    }
    public void EndRace(string result, int playerPosition, int totalRacers)
    {
        UpdateGameState(GameState.RaceEnd);
        OnRaceEnd?.Invoke(result, playerPosition, totalRacers);
    }
    
    public void ResetRace()
    {
        UpdateGameState(GameState.CountDown);
    }
}

public enum GameState
{
    MainMenu,
    CountDown,
    InGame,
    Paused,
    RaceEnd
}
