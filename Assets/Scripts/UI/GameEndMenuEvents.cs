using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class GameEndMenuEvents : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button _startNewButton;
    private Button _mainMenuButton;
    private CameraFollow _CameraFollow;

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
        _startNewButton = _uiDocument.rootVisualElement.Q<Button>("start-new-button");
        _startNewButton.RegisterCallback<ClickEvent>(OnStartNewButtonClicked);
        
        _mainMenuButton = _uiDocument.rootVisualElement.Q<Button>("main-menu-button");
        _mainMenuButton.RegisterCallback<ClickEvent>(OnMainMenuButtonClicked);
        
        _CameraFollow = FindObjectOfType<CameraFollow>();
    }

    private void OnDisable()
    {
        _startNewButton.UnregisterCallback<ClickEvent>(OnStartNewButtonClicked);
        _mainMenuButton.UnregisterCallback<ClickEvent>(OnMainMenuButtonClicked);
    }

    private void OnStartNewButtonClicked(ClickEvent evt)
    {
        Debug.Log("Start new game button clicked");
        RaceManager.Instance.ResetRace();
        GameManager.Instance.ResetRace();
    }
    
    private void OnMainMenuButtonClicked(ClickEvent evt)
    {
        Debug.Log("Main menu button clicked");
        RaceManager.Instance.ResetRace();
        // _CameraFollow.ResetCamera();
        GameManager.Instance.UpdateGameState(GameState.MainMenu);
        
    }
}
