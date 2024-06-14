using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerPositionUI : MonoBehaviour
{
    // Start is called before the first frame update
    private UIDocument _uiDocument;
    private Label _positionLabel;

    private void Awake()
    {
        
        _uiDocument = GetComponent<UIDocument>();
        _positionLabel = _uiDocument.rootVisualElement.Q<Label>("position-label");
        Console.WriteLine($"This object is active: {gameObject.activeSelf}");
    }

    private void Update()
    {
        _positionLabel.text = $"Position: {RaceManager.Instance.PlayerPosition}/{RaceManager.Instance.Racers.Count}";
    }
}
