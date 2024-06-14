using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenuEvents : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button _startButton;
    private Button _pauseButton;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        
        _pauseButton = _uiDocument.rootVisualElement.Q<Button>("pause-button");
        _pauseButton.RegisterCallback<ClickEvent>(OnPauseButtonClicked);
    }

    private void OnDisable()
    {
        _pauseButton.UnregisterCallback<ClickEvent>(OnPauseButtonClicked);
    }
    
    private void OnPauseButtonClicked(ClickEvent evt)
    {
        Debug.Log("Pause button clicked");
        GameManager.Instance.UpdateGameState(GameState.InGame);
    }
}
