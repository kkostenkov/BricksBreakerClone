using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreMultiplicationButton : MonoBehaviour
{
    [SerializeField]
    private Button button;

    [SerializeField]
    private int multiplicatorValue;

    public int Muliplicator => this.multiplicatorValue;
    public Action MultiplicatorSelected;

    private void Awake()
    {
        this.button.onClick.AddListener(OnButtonClicked);
        var text = GetComponentInChildren<TextMeshProUGUI>();
        text.text = multiplicatorValue + "x";
    }

    private void OnDestroy()
    {
        this.button.onClick.RemoveAllListeners();
    }

    private void OnButtonClicked()
    {
        this.MultiplicatorSelected?.Invoke();
    }
}
