using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private UIDocument _mainMenu;
    [SerializeField] private UIDocument _pauseMenu;
    [SerializeField] private UIDocument _playerPositionUI;
    [SerializeField] private UIDocument _countdownUI;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    private void GameManager_OnGameStateChanged(GameState state)
    {
        _mainMenu.rootVisualElement.style.display = (state == GameState.MainMenu) ? DisplayStyle.Flex : DisplayStyle.None;
        _pauseMenu.rootVisualElement.style.display = (state == GameState.Paused) ? DisplayStyle.Flex : DisplayStyle.None;
        _countdownUI.rootVisualElement.style.display = (state == GameState.CountDown) ? DisplayStyle.Flex : DisplayStyle.None;
        _playerPositionUI.rootVisualElement.style.display = (state == GameState.InGame) ? DisplayStyle.Flex : DisplayStyle.None;
    }
}
