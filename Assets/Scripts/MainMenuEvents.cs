using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button _startButton;
    private Button _pauseButton;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _startButton = _uiDocument.rootVisualElement.Q<Button>("start-button");
        _startButton.RegisterCallback<ClickEvent>(OnStartButtonClicked);
        
        _pauseButton = _uiDocument.rootVisualElement.Q<Button>("pause-button");
        _pauseButton.RegisterCallback<ClickEvent>(OnPauseButtonClicked);
    }

    private void OnDisable()
    {
        _startButton.UnregisterCallback<ClickEvent>(OnStartButtonClicked);
        _pauseButton.UnregisterCallback<ClickEvent>(OnPauseButtonClicked);
    }

    private void OnStartButtonClicked(ClickEvent evt)
    {
        Debug.Log("Start button clicked");
    }
    
    private void OnPauseButtonClicked(ClickEvent evt)
    {
        Debug.Log("Start button clicked");
    }
    
}
