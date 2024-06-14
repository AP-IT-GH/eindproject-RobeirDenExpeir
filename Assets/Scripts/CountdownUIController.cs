using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CountdownUIController : MonoBehaviour
{
    [SerializeField] private UIDocument _countdownUI;
    private Label _countdownText;
    private void Awake()
    {
        
        _countdownUI = GetComponent<UIDocument>();
        _countdownText = _countdownUI.rootVisualElement.Q<Label>("countdownText");
    }
    
    public IEnumerator StartCountdown()
    {
        // 3, 2, 1, GO!
        _countdownText.text = "3";
        yield return new WaitForSeconds(1f);
        _countdownText.text = string.Empty;
        yield return new WaitForSeconds(.5f);

        _countdownText.text = "2";
        yield return new WaitForSeconds(1f);
        _countdownText.text = string.Empty;
        yield return new WaitForSeconds(.5f);

        _countdownText.text = "1";
        yield return new WaitForSeconds(1f);
        _countdownText.text = string.Empty;
        yield return new WaitForSeconds(.5f);

        _countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        _countdownText.text = string.Empty;
    }
}
