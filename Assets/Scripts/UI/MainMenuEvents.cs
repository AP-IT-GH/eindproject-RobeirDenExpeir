using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button _startButton;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _startButton = _uiDocument.rootVisualElement.Q<Button>("start-button");
        _startButton.RegisterCallback<ClickEvent>(OnStartButtonClicked);
    }

    private void OnDisable()
    {
        _startButton.UnregisterCallback<ClickEvent>(OnStartButtonClicked);
    }

    private void OnStartButtonClicked(ClickEvent evt)
    {
        Debug.Log("Start button clicked");
        GameManager.Instance.UpdateGameState(GameState.CountDown);
    }
    
}
